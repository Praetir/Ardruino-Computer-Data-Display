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
        float tempCPU, tempGPU;
        float sumCPU, sumGPU;
        int sizeSam; // Number of samples before the OLED display updates
        int curSam = 1;
        int waitTime;  // Amount of time before next communication verification cycle starts
        int dataTime; // Amount of time before the OLED display updates
        string prefPort; // Port set in pref.txt

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
            // Get hardware data
            NumsGet();

            // Add up the values and index
            sumCPU += tempCPU;
            sumGPU += tempGPU;
            curSam += 1;

            // Average then send to Arduino if at sample size
            if (curSam >= sizeSam)
            {
                tempCPU = sumCPU / 10;
                tempGPU = sumGPU  / 10;
                SendArd();
                sumCPU = 0;
                sumGPU = 0;
                curSam = 1;
            }
        }

        private void TimerCom_Tick(object sender, EventArgs e)
        {
            // Reset Arduino and wait a couple of seconds
            ardPort.WriteLine("@");
            Thread.Sleep(waitTime);

            ardPort.WriteLine("!");
        }

        private void ButtonOpenPort_Click(object sender, EventArgs e)
        {
                // Update status
                toolStripStatusLabel1.Text = ("Connecting to Arduino...");

                // Open selected port
                if (autoStart)
                {
                    ardPort.PortName = prefPort;
                } 
                else
                {
                    ardPort.PortName = portCBox.Text;
                }
                
                ardPort.Open();
                Thread.Sleep(1000);

                // Change buttons
                buttonOpenPort.Enabled = false;
                buttonClosePort.Enabled = true;

                timerCom.Start();
        }

        private void ButtonClosePort_Click(object sender, EventArgs e)
        {
            try
            {
                // Close selected port after sending disconnect line and disabling timerData
                timerData.Stop();
                
                if (ardPort.IsOpen)
                {
                    ardPort.WriteLine("disconnect");
                }
                ardPort.Close();

                // Change buttons
                buttonOpenPort.Enabled = true;
                buttonClosePort.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ArdPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            mChar = (char)ardPort.ReadChar();
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
                ardConfig = false;
                Console.WriteLine("Did not receive expected character.");
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
            
            // Read text file with label data
            string[] allInfo = System.IO.File.ReadAllLines(Properties.Settings.Default.LastProfilePath);

            // Do not load labels if there are none
            if (allInfo.Length <= 1)
            {
                // End configure mode
                ardPort.WriteLine("end|");
                ardConfig = false;
                loadButton.Enabled = true;
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
               
                // Needed information: Label Text, Data Type(e.g. temp), Font Name, Font Size, Font Color, Cursor Position (DispX and DispY, see DispEditForm)
                // Need to make a translator for fonts here... probably
                // Order all needed information
                // Data type simplified to one letter, t for temperature, s for storage, l for load
                // Write to Arduino
                Console.WriteLine(rowSplit[3].Split(':')[0] + '|' + rowSplit[0][0] + '|' + rowSplit[4] + '|' + rowSplit[5] + '|' + "COLOR" + '|' + rowSplit[9] + '|' + rowSplit[10] + '|');
            }

            // End configure mode
            ardPort.WriteLine("end|");
            ardConfig = false;
            timerData.Enabled = true;
            loadButton.Enabled = true;
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

        private void NumsGet()
        {
            // Check each hardware part in c
            foreach (var hardware in c.Hardware)
            {
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    hardware.Update();
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            Console.WriteLine("Getting CPU temp info");
                            if (sensor.Name == "CPU Package")
                            {
                                Console.WriteLine("Getting CPU Package info");
                                tempCPU = sensor.Value.GetValueOrDefault();
                            }
                        }
                    }
                }

                if (hardware.HardwareType == HardwareType.GpuNvidia)
                {
                    hardware.Update();
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            Console.WriteLine("Getting GPU temp info");
                            if (sensor.Name == "GPU Core")
                            {
                                Console.WriteLine("Getting GPU Core info");
                                tempGPU = sensor.Value.GetValueOrDefault();
                            }
                        }
                    }
                }
            }
        }

        private void SendArd()
        {
            ardPort.WriteLine("<" + tempCPU + ")" + tempGPU + ">");
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
