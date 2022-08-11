using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using OpenHardwareMonitor.Hardware;
using System.Threading;

namespace Arduino_Computer_Data_Display
{
    public partial class ACDDForm : Form
    {
        char mChar;
        bool estContact = false;
        bool nextChar = true;
        bool autoStart = false;
        bool ardConfig = false;
        float[] dataValues;
        int sizeSam; // Number of samples before the OLED display updates
        int curSam = 1;
        int waitTime;  // Amount of time before next communication verification cycle starts
        int dataTime; // Amount of time before the OLED display updates
        string prefPort; // Port set in pref.txt

        // Initialize dictionary for label names and values
        public Dictionary<string, ISensor> sensDict = new Dictionary<string, ISensor>();

        // Initialize list for label names (to handle averages)
        public List<string> send2Ard = new List<string>();

        readonly Computer c = new Computer()
        {
            GPUEnabled = true,
            CPUEnabled = true
        };

        // Declare display edit form
        public DispEditForm edit;

        public ACDDForm()
        {
            InitializeComponent();

            // Find path to Arduino Computer Data Display
            bool foundProgramFolder = false;
            string programFolder = System.IO.Directory.GetCurrentDirectory();
            while (!foundProgramFolder)
            {
                if (System.IO.Path.GetFileName(programFolder) == "Arduino Computer Data Display")
                {
                    foundProgramFolder = true;
                    Console.WriteLine(System.IO.Path.GetFileName(programFolder));
                    continue;
                }
                if (programFolder == null)
                {
                    // Do some error handling here
                }
                programFolder = System.IO.Directory.GetParent(programFolder).ToString();
            }
            Properties.Settings.Default.ProgramPath = programFolder;

            // Read preferences from text file
            string[] prefData = System.IO.File.ReadAllLines(System.IO.Path.Combine(programFolder, "pref.txt"));

            // Set preference values
            PrefSet(prefData);

            // Open computer to get hardware data
            c.Open();

            // Determine time interval timerCom (waitTime + 2 seconds)
            // waitTime will be read from a text file later
            timerCom.Interval = waitTime + 2000;

            // Determine timer interval for timerData
            timerData.Interval = dataTime / sizeSam;

            // Add all ports to combo box
            string[] ports = SerialPort.GetPortNames();
            portCBox.Items.AddRange(ports);

            // For automatic start
            if (autoStart)
            {
                ButtonOpenPort_Click(this, new EventArgs());
            }

            // Initialize display edit form
            edit = new DispEditForm();
        }

        private void ACDD_Load(object sender, EventArgs e)
        {

        }

        private void TimerData_Tick(object sender, EventArgs e)
        {
            // Make sure there are values to send
            if (send2Ard.Count() <= 0) return;

            // Update sensors
            foreach (var hardware in c.Hardware) hardware.Update();

            // Update values
            float value = 0;
            int avgNum = 0;
            for (int i = 0; i < send2Ard.Count(); i++)
            {
                // Check for average labels and handle
                if (send2Ard[i].Contains("Average"))
                {
                    // Add all sensors to be averaged
                    foreach (var pair in sensDict)
                    {
                        if (pair.Key.Contains(send2Ard[i].Substring(0, send2Ard[i].Length - (send2Ard[i].IndexOf("Average") - 1))))
                        {
                            value += pair.Value.Value.GetValueOrDefault();
                            avgNum++;
                        }
                    }

                    // Divide all sensors to be averaged
                    value /= avgNum;
                }
                else
                {
                    value = sensDict[send2Ard[i]].Value.GetValueOrDefault();
                }

                dataValues[i] += value;
            }

            // Index up
            curSam++;

            // Average then send to Arduino if at sample size
            if (curSam >= sizeSam)
            {
                // Update values and send to arduino
                for (int i = 0; i < send2Ard.Count(); i++)
                {
                    dataValues[i] /=  sizeSam;
                    ardPort.Write(dataValues[i].ToString() + ' ');
                }

                // Clear values and reset sample number
                dataValues = new float[send2Ard.Count()];
                curSam = 0;
            }
        }

        private void TimerCom_Tick(object sender, EventArgs e)
        {
            // Reset Arduino and wait a couple of seconds
            ardPort.WriteLine("@|");
            Thread.Sleep(waitTime);

            ardPort.WriteLine("!|");
        }

        private void ButtonOpenPort_Click(object sender, EventArgs e)
        {
            // Open selected port
            if (autoStart)
            {
                ardPort.PortName = prefPort;
            }
            else if (portCBox.Text.Length > 0)
            {
                ardPort.PortName = portCBox.Text;
            }
            else
            {
                return;
            }

            // Update status
            toolStripStatusLabel1.Text = ("Connecting to Arduino...");

            ardPort.Open();
            Thread.Sleep(1000);

            // Change buttons
            buttonOpenPort.Enabled = false;
            buttonClosePort.Enabled = true;

            timerCom.Start();
        }

        private void ButtonClosePort_Click(object sender, EventArgs e)
        {
            // Close selected port after sending disconnect line and disabling timerData
            timerData.Stop();
            estContact = false;

            if (ardPort.IsOpen)
            {
                ardPort.WriteLine("disconnect|");
            }
            ardPort.Close();

            // Change buttons
            buttonOpenPort.Enabled = true;
            buttonClosePort.Enabled = false;
        }

        private void ArdPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            mChar = (char)ardPort.ReadChar();
            Console.WriteLine(mChar);

            if (!estContact)
            {
                timerCom.Stop();
                ComVerify();
            }
            else if (mChar == '*')
            {
                ardConfig = true;
                // Call function to continue
                LoadButton_Click(new object(), new EventArgs());
            }
            else
            {
                if (ardConfig)
                {
                    ardConfig = false;
                    Console.WriteLine("Did not receive expected character.");
                }
            }
        }

        private void ArdPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            ErrorClose();
        }

        private void ACDD_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if serial port is open, close if true
            if (ardPort.IsOpen)
            {
                ardPort.WriteLine("_|");
                ardPort.Close();
            }

            timerData.Stop();
            c.Close();
        }

        private void EditDispButton_Click(object sender, EventArgs e)
        {
            edit.ShowDialog();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            // Set Arduino to configure
            timerData.Enabled = false;
            ardPort.WriteLine("config|");
            loadButton.Enabled = false;

            // Check if Arduino is in configure mode, return if not
            if (!ardConfig)
            {
                return;
            }

            // Clear sensor dictionary and labels list
            sensDict.Clear();
            send2Ard.Clear();

            // Check if there is a profile in the settings
            string path = Properties.Settings.Default.LastProfilePath;
            if (path == "")
            {
                // End configure mode
                ardPort.WriteLine("end|");
                ardConfig = false;
                Invoke(new Action(() => loadButton.Enabled = true));
                Console.WriteLine("No valid path");
                return;
            }

            // Read text file with label data
            string[] allInfo = System.IO.File.ReadAllLines(path);

            // Do not load labels if there are none
            if (allInfo.Length <= 1)
            {
                // End configure mode
                ardPort.WriteLine("end|");
                ardConfig = false;
                Invoke(new Action(() => loadButton.Enabled = true));
                Console.WriteLine("No labels in profile");
                return;
            }

            // Get info for each row and send what is needed to the Arduino (skip first line)
            string[] rowSplit;
            foreach (string row in allInfo)
            {
                // Check if boolean is false (indicates the label is not the display area), otherwise keep going
                if (row.Contains("False") || row.Contains("ACDD Profile"))
                {
                    continue;
                }

                // Split at the |
                rowSplit = row.Split('|');

                // Stick label name in dictionary and list
                sensDict.Add(rowSplit[2].Replace('_', ' '), null);
                send2Ard.Add(rowSplit[2].Replace('_', ' '));

                // Needed information: Label Text, Data Type(e.g. temp), Font Name, Font Size, Font Color, Cursor Position (DispX and DispY, see DispEditForm)
                // Need to make a translator for fonts here... probably
                // Order all needed information
                // Data type simplified to one letter, t for temperature, s for storage, l for load
                // Write to Arduino
                ardPort.WriteLine(rowSplit[3].Split(':')[0] + '|' + rowSplit[0][0] + '|' + rowSplit[4] + '|' + rowSplit[5] + '|' + "COLOR" + '|' + rowSplit[9] + '|' + rowSplit[10] + '|');
                Console.WriteLine(rowSplit[3].Split(':')[0] + '|' + rowSplit[0][0] + '|' + rowSplit[4] + '|' + rowSplit[5] + '|' + "COLOR" + '|' + rowSplit[9] + '|' + rowSplit[10] + '|');
            }

            // End configure mode
            ardPort.WriteLine("end|");
            ardConfig = false;
            timerData.Enabled = true;
            Invoke(new Action(() => loadButton.Enabled = true));

            // Get all desired sensors
            SensorGet();

            // Populate data value array with zeros
            dataValues = new float[send2Ard.Count()];
        }

        private void ComVerify()
        {
            // Rechecks character if arduino throws ? back after the reset 
            while (nextChar)
            {
                if (mChar == '?')
                {
                    mChar = (char)ardPort.ReadChar();
                }
                else
                {
                    nextChar = false;
                }
            }

            if (mChar == '&')
            {
                estContact = true;
                toolStripStatusLabel1.Text = "Connected to Arduino...";
                Invoke(new Action(() => loadButton.Enabled = true));
                Invoke(new Action(() => timerData.Start()));
            }
            else
            {
                Console.WriteLine("Did not receive contact symbol &");
                ErrorClose();
            }
        }

        private void ErrorClose()
        {
            toolStripStatusLabel1.Text = "Closing";
            Console.WriteLine("Some error caused the form to close.");
            Application.Exit();
        }

        private void SensorGet()
        {
            string currentSensor;
            string dataType;
            bool sensorAverage;

            // Easier sensor dictionary population
            foreach (var label in sensDict.Keys)
            {
                // Reset average boolean to false
                sensorAverage = false;

                // Get just the label's component
                currentSensor = label.Substring(label.IndexOf(' ') + 1);

                // Get data type
                dataType = label.Split(' ')[0];

                // Check if this is an average
                if (currentSensor.Contains("Average"))
                {
                    currentSensor = currentSensor.Substring(0, currentSensor.Length - (currentSensor.IndexOf("Average") - 1));
                    sensorAverage = true;
                }

                // Check each enabled hardware component
                foreach (var hardware in c.Hardware)
                {
                    // Check each sensor from each component
                    foreach (var sensor in hardware.Sensors)
                    {
                        // Check if name matches label
                        if (sensor.Name.Contains(currentSensor))
                        {
                            // Check if sensor type matches label data type
                            if (sensor.SensorType == SensorType.Temperature && label.Contains("temp"))
                            {
                                // Handle if average
                                if (sensorAverage)
                                {
                                    SensorAverageAdd(dataType, sensor);
                                    continue;
                                }

                                // Assign sensor to key
                                sensDict[currentSensor] = sensor;
                            }
                            else if (sensor.SensorType == SensorType.Load && label.Contains("load"))
                            {
                                // Handle if average
                                if (sensorAverage)
                                {
                                    SensorAverageAdd(dataType, sensor);
                                    continue;
                                }

                                // Assign sensor to key
                                sensDict[currentSensor] = sensor;
                            }

                            // HERE: Add more later
                        }

                    }
                }
            }

            //// Check if there are value to get
            //if (namesValues.Keys.Count() > 0)
            //{
            //    IEnumerable<IHardware> queryHardware;
            //    List<ISensor[]> sensors = new List<ISensor[]>();
            //    List<ISensor> querySensor = new List<ISensor>();
            //    IEnumerable<string> compList;
            //    string curSen;

            //    // Get any values for CPU
            //    if (namesValues.Keys.Contains("CPU"))
            //    {
            //        // Get list items that are for the CPU
            //        compList = namesValues.Keys.Where(label => label.Contains("CPU"));

            //        // Get CPU component(s)
            //        queryHardware = c.Hardware.Where(parts => parts.HardwareType == HardwareType.CPU);

            //        // Get sensors
            //        foreach (var parts in queryHardware)
            //        {
            //            // Update parts and throw all sensors into a list
            //            parts.Update();
            //            sensors.Add(parts.Sensors);
            //        }

            //        // Get values for temperature sensors
            //        if (compList.Contains("temp"))
            //        {
            //            // Narrow list of desired values
            //            IEnumerable<string> senList = compList.Where(label => label.Contains("temp"));

            //            // Add individual sensors to list
            //            foreach (var sensorSet in sensors)
            //            {
            //                foreach (var sensor in sensorSet)
            //                {
            //                    if (sensor.SensorType == SensorType.Temperature)
            //                    {
            //                        querySensor.Add(sensor);
            //                    }
            //                }
            //            }

            //            // Go through the narrowed label list and match them to sensors in the narrowed sensor list
            //            foreach (var label in senList)
            //            {
            //                curSen = label.Substring(label.IndexOf(' ')+1);
            //                namesValues[label] = querySensor.Find(sen => sen.Name == curSen).Value.GetValueOrDefault();
            //            }
            //        }
            //    }
            //}

            //// Experiment
            //if (saved)
            //{
            //    foreach (var hardware in c.Hardware)
            //    {
            //        hardware.Update();
            //    }
            //    Console.WriteLine(namesValues["CPU Package"].Value.GetValueOrDefault());
            //    return;
            //}


            //// Check each hardware part in c
            //foreach (var hardware in c.Hardware)
            //{
            //    //Console.WriteLine("Hardware Name: " + hardware.Name + "\r\n Hardware Type: " + hardware.HardwareType.ToString());
            //    if (hardware.HardwareType == HardwareType.CPU)
            //    {
            //        hardware.Update();
            //        foreach (var sensor in hardware.Sensors)
            //        {
            //            //Console.WriteLine("Sensor Name: " + sensor.Name + "\r\n Sensor Type: " + sensor.SensorType.ToString());
            //            if (sensor.SensorType == SensorType.Temperature)
            //            {
            //                if (sensor.Name == "CPU Package")
            //                {
            //                    namesValues.Add("CPU Package", sensor);
            //                    Console.WriteLine(namesValues["CPU Package"].Value.GetValueOrDefault());
            //                    saved = true;
            //                }
            //            }
            //        }
            //    }

            //    if (hardware.HardwareType == HardwareType.GpuNvidia)
            //    {
            //        hardware.Update();
            //        foreach (var sensor in hardware.Sensors)
            //        {
            //            if (sensor.SensorType == SensorType.Temperature)
            //            {
            //                Console.WriteLine("Getting GPU temp info");
            //                if (sensor.Name == "GPU Core")
            //                {
            //                    Console.WriteLine("Getting GPU Core info");
            //                    tempGPU = sensor.Value.GetValueOrDefault();
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private void  SensorAverageAdd(string type, ISensor sensor)
        {
            sensDict.Add(type + ' ' + sensor.Name, sensor);
        }

        // Set preferences and settings based on pref.txt
        private void PrefSet(string[] prefData)
        {
            sizeSam = int.Parse(SeparatePref(0, prefData));
            waitTime = int.Parse(SeparatePref(1, prefData));
            dataTime = int.Parse(SeparatePref(2, prefData));
            autoStart = bool.Parse(SeparatePref(3, prefData));
            prefPort = SeparatePref(4, prefData);
            Properties.Settings.Default.numHorPix = int.Parse(SeparatePref(5, prefData));
            Properties.Settings.Default.numVertPix = int.Parse(SeparatePref(6, prefData));
        }

        private string SeparatePref(int prefLineIndex, string[] prefData)
        {
            string prefLine = prefData[prefLineIndex];
            string[] prefBoth = prefLine.Split('=');
            return prefBoth[1];
        }
    }
}
