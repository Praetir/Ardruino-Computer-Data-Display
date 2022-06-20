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
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
