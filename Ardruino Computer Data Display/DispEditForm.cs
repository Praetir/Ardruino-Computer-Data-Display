using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ardruino_Computer_Data_Display
{
    public partial class DispEditForm : Form
    {
        

        public DispEditForm()
        {
            InitializeComponent();
        }

        private void DispCPU_CheckedChanged(object sender, EventArgs e)
        {
            if (dispCPU.Checked)
            {
                tempCPUCL.Enabled = true;
                coreCPUText.Enabled = true;
            }
            else
            {
                tempCPUCL.Enabled = false;
                coreCPUText.Enabled = false;
            }
            
        }

        private void CoreCPUText_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits into text box
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            // Check if enter key has been pressed
            else if (e.KeyChar == (char)Keys.Enter)
            {
                UpdateChecklist(tempCPUCL, "CPU Core");
            }
        }

        // Remove and add checklist items to desired box
        private void UpdateChecklist(CheckedListBox list, string part)
        {
            // Remove CPU core temperature options from check list
            int listNum = list.Items.Count;
            if (listNum > 2)
            {
                for (int i = 2; i < listNum; i++)
                {
                    list.Items.RemoveAt(2);
                }
            }

            // Add updated number of CPU core temperature options to check list
            for (int i = 1; i <= int.Parse(coreCPUText.Text); i++)
            {
                list.Items.Add(String.Format("{0} {1}", part, i), false);
            }
        }
    }
}
