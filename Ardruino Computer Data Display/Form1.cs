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

namespace Ardruino_Computer_Data_Display
{
    public partial class Form1 : Form
    {
        char mChar;
        bool estContact = false;
        float tempCPU, tempGPU;
        float sumCPU, sumGPU;
        bool nextChar = true;
        int sizeSam = 10; // Number of samples before the OLED display updates
        int curSam = 1;
        int waitTime = 2000;  // Amount of time before next communication verification cycle starts
        int dataTime = 1000; // Amount of time before the OLED display updates

        readonly Computer c = new Computer()
        {
            GPUEnabled = true,
            CPUEnabled = true
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Open computer to get hardware data
            c.Open();

            // Determine time interval timerCom (waitTime + 2 seconds)
            // waitTime will be read from a text file later
            timerCom.Interval = waitTime + 2000;

            // Determine timer interval for timerData
            timerData.Interval = dataTime/sizeSam;

            // Add all ports to combo box
            string[] ports = SerialPort.GetPortNames();
            portCBox.Items.AddRange(ports);
        }

        private void TimerData_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Timer1 ticked");
            
            // Get hardware data
            NumsGet();

            // Add up the values and index
            sumCPU = sumCPU + tempCPU;
            sumGPU = sumGPU + tempGPU;
            curSam = curSam + 1;

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
            Console.WriteLine("Timer2 ticked");

            // Reset Arduino and wait a couple of seconds
            ardPort.WriteLine("@");
            Thread.Sleep(waitTime);

            ardPort.WriteLine("!");
        }

        private void ButtonOpenPort_Click(object sender, EventArgs e)
        {
            try
            {
                // Update status
                toolStripStatusLabel1.Text = ("Connecting to Arduino...");

                // Open selected port
                ardPort.PortName = portCBox.Text;
                ardPort.Open();
                Thread.Sleep(1000);

                // Change buttons
                buttonOpenPort.Enabled = false;
                buttonClosePort.Enabled = true;

                timerCom.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonClosePort_Click(object sender, EventArgs e)
        {
            try
            {
                // Close selected port after sending disconnect line and disabling timerData
                timerData.Stop();
                ardPort.WriteLine("_");
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

        private void ArdPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            mChar = (char)ardPort.ReadChar();
            Console.WriteLine(mChar);
            if (!estContact)
            {
                timerCom.Stop();
                ComVerify();
            }
        }

        private void ArdPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            ErrorClose();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if serial port is open, close if true
            if (ardPort.IsOpen)
            {
                ardPort.WriteLine("_");
                ardPort.Close();
            }

            timerData.Stop();
            c.Close();
        }

        private void ComVerify()
        {
            // Rechecks character if arduino throws ? back after the reset
            while (nextChar)
            {
                if (mChar == '?')
                {
                    mChar = (char)ardPort.ReadChar();
                    Console.WriteLine(mChar);
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
                Invoke(new Action(() => timerData.Start()));
                Console.WriteLine("Communication verified");
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
            Console.WriteLine("Getting hardware info");
            foreach (var hardware in c.Hardware)
            {
                Console.WriteLine("Got hardware");
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    Console.WriteLine("Getting CPU info");
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
                    Console.WriteLine("Getting GPU info");
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
            Console.WriteLine("<" + tempCPU + ")" + tempGPU + ">");
            ardPort.WriteLine("<" + tempCPU + ")" + tempGPU + ">");
        }
    }
}
