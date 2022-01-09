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

namespace Ardruino_Computer_Monitor
{
    public partial class Form1 : Form
    {
        Char mChar;
        Boolean estContact = false;
        float tempCPU, tempGPU;
        int waitTime = 2000;  // Will be read from text file later
        int dataTime = 1000; // Will be read from text file later

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

            // Determine time interval timerCom (waitTime + 1 seconds)
            // waitTime will be read from a text file later
            timerCom.Interval = waitTime + 1000;

            // Set timer interval for timerData
            timerData.Interval = dataTime;

            // Add all ports to combo box
            string[] ports = SerialPort.GetPortNames();
            portCBox.Items.AddRange(ports);
        }

        private void timerData_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Timer1 ticked");
            
            // Get hardware data
            numsGet();

            // Send to Arduino
            sendArd();
        }

        private void timerCom_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Timer2 ticked");
            // Reset Arduino and wait a couple of seconds
            ardPort.WriteLine("@");
            Thread.Sleep(waitTime);

            ardPort.WriteLine("!");
        }

        private void buttonOpenPort_Click(object sender, EventArgs e)
        {
            try
            {
                // Open selected port
                ardPort.PortName = portCBox.Text;
                ardPort.Open();

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

        private void buttonClosePort_Click(object sender, EventArgs e)
        {
            try
            {
                // Close selected port after sending disconnect line and disabling timer1
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

        private void ardPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            mChar = (char)ardPort.ReadChar();
            Console.WriteLine(mChar);
            if (!estContact)
            {
                timerCom.Stop();
                comVerify();
            }
        }
        private void ardPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            errorClose();
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

        private void comVerify()
        {
            // Rechecks character if arduino throws ⸮ back after the reset
            if (mChar == '⸮')
            {
                mChar = (char)ardPort.ReadChar();
            }

            if (mChar == '&')
            {
                estContact = true;
                toolStripStatusLabel1.Text = "Connected to Arduino...";
                timerData.Start();
                Console.WriteLine("Communication verified");
            }
            else
            {
                Console.WriteLine("Did not receive contact symbol &");
                errorClose();
            }
        }

        private void errorClose()
        {
            toolStripStatusLabel1.Text = "Closing";
            Console.WriteLine("Some error caused the form to close.");
            Application.Exit();
        }

        private void numsGet()
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

        private void sendArd()
        {
            Console.WriteLine("<" + tempCPU + ")" + tempGPU + ">");
            ardPort.WriteLine("<" + tempCPU + ")" + tempGPU + ">");
        }
    }
}
