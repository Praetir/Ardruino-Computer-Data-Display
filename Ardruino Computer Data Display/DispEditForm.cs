using System;
using System.Collections;
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
        // Initialize data table
        public DataTable dispTable;

        // Initialize trash can range (1st and 2nd values are for x, 3rd and 4th for y)
        public int[] trashRange = new int[4];

        public DispEditForm()
        {
            InitializeComponent();
        }
        
        private void DispEditForm_Load(object sender, EventArgs e)
        {
            // Adjust size of display area if needed
            dispArea.Width = ACDDForm.numHorPix;
            dispArea.Height = ACDDForm.numVertPix;

            // Make new data table and add columns
            dispTable = new DataTable();
            dispTable.Columns.Add(new DataColumn("ChecklistName"));
            dispTable.Columns.Add(new DataColumn("CheckIndex"));
            dispTable.Columns.Add(new DataColumn("LabelName"));
            dispTable.Columns.Add(new DataColumn("LabelText"));
            dispTable.Columns.Add(new DataColumn("LabelFont"));
            dispTable.Columns.Add(new DataColumn("LabelFontSize"));
            dispTable.Columns.Add(new DataColumn("LabelLocationX"));
            dispTable.Columns.Add(new DataColumn("LabelLocationY"));

            // Get trash can range
            Point trashCorner = trashPicBox.Location;
            Size trashSize = trashPicBox.Size;
            trashRange[0] = trashCorner.X;
            trashRange[1] = trashRange[0] + trashSize.Width;
            trashRange[2] = trashCorner.Y;
            trashRange[3] = trashRange[2] + trashSize.Height;
            //Console.WriteLine(String.Format("{0}<x<{1} {2}>x>{3}", trashRange[0], trashRange[1], trashRange[2], trashRange[3]));

            dataGridView1.DataSource = dispTable; // Temporary for viewing if data table is working properly
        }

        private void DispCPUCheck_CheckedChanged(object sender, EventArgs e)
        {
            // Enable/disable CPU controls
            if (dispCPUCheck.Checked)
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

        private void CL_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Get checklist
            CheckedListBox myCL = (CheckedListBox)sender;

            // Get checklist information
            int index = myCL.SelectedIndex;
            bool checkedItem = myCL.GetItemChecked(index);
            string checkName = (string)myCL.Items[index];

            if (!checkedItem)
            {
                // Add label to form/data table, and add checklist/index to data table
                DispAdd(myCL.Name, index, checkName, (string)myCL.Tag);

                //Console.WriteLine("Added");
            }
            else
            {
                // Remove label from form/data table, and remove corresponding row of data table
                DispRemove(myCL.Name, index);
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

            return;
        }

        // Add text label to form to represent display items
        private void DispAdd(string clName, int index, string checkName, string dataType)
        {
            // Get desired label name and text
            string[] labelInfo = MakeLabelTextName(checkName, dataType);

            // Create label
            Label labelDisp = new Label
            {
                Text = labelInfo[0],
                Name = labelInfo[1],
                Location = new Point(300, 280),
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 6.0f)
            };

            // Store label and checklist information in datatable
            
            dispTable.Rows.Add(clName, index, labelDisp.Name, labelDisp.Text, labelDisp.Font.Name, labelDisp.Font.Size, labelDisp.Location.X, labelDisp.Location.Y);

            // Make label draggable, add to form, and put it to front
            ControlExtension.Draggable(labelDisp, true);
            Controls.Add(labelDisp);
            labelDisp.BringToFront();

            // Add dragging event to label
            labelDisp.MouseUp += LabelLocationAction;

            // Return nothing
            return;
        }

        // Remove particular text label from form
        private void DispRemove(string checklistName, int index)
        {
            // Get the desired data row from the data table
            DataRow[] remRow = dispTable.Select(string.Format("ChecklistName = '{0}' AND CheckIndex = '{1}'", checklistName, index));

            // Check if the label has been deleted already
            if (remRow.Length == 0)
            {
                return;
            }
            DataRow removeRow = remRow[0];

            // Cast the label name data to a string, get the label, and remove it from the form
            string labelDisp = (string)removeRow["LabelName"];
            Controls.Remove(Controls.OfType<Label>().FirstOrDefault(c => c.Name == labelDisp));
            dispTable.Rows.Remove(removeRow);

            // Nothing to return
            return;
        }

        // Determine text and name for label
        private string[] MakeLabelTextName (string checkName, string dataType)
        {
            // Allocate strings
            string labelText = "";
            string labelName = "";

            // Add to label text and name based on the computer component
            if (checkName.Contains("CPU"))
            {
                labelText = "CPU";
                labelName = "CPU";
            }
            else if (checkName.Contains("GPU"))
            {
                labelText = "GPU";
                labelName = "GPU";
            }

            // Add to label text and name based on the component part
            if (checkName.Contains("Core Average"))
            {
                labelText += " Core Avg";
                labelName += "CoreAvg";
            }
            else if (checkName.Contains("Package"))
            {
                labelText += " Package";
                labelName += "Package";
            }
            else if (checkName.Contains("Core"))
            {
                labelText += " Core";
                labelName += "Core";
                string[] checkSplit = checkName.Split(' ');
                labelText += " ";
                labelText += checkSplit[2];
                labelName += checkSplit[2];
            }

            // Add to label text and name based on the type of computer data
            switch (dataType)
            {
                case "temp":
                    labelText += " Temp: ##.#°C";
                    labelName += "Temp";
                    break;
                case "storage":
                    labelText += " Storage: ####.#GB";
                    labelName += "Storage";
                    break;
                case "load":
                    labelText += " Load: ##%";
                    labelName += "Load";
                    break;
                default:
                    break;
            }

            labelName += "Label";

            //Console.WriteLine(labelText);
            //Console.WriteLine(labelName);

            string[] labelInfo = new string[2] {labelText, labelName};

            // Return label name and text
            return labelInfo;
        }

        private void LabelLocationAction(object sender, MouseEventArgs e)
        {
            // Get cursor position by adding the label postion and cursor position(relative to label)
            Label dragLabel = (Label)sender;
            Point labelPoint = dragLabel.Location;
            Point cursorPoint = e.Location;
            int mouseLocX = labelPoint.X + cursorPoint.X;
            int mouseLocY = labelPoint.Y + cursorPoint.Y;

            // Update label point in checklist
            DataRow[] tempRows = dispTable.Select(string.Format("LabelName = '{0}'", dragLabel.Name));

            // Check for error
            if (tempRows.Length == 0)
            {
                Controls.Remove(dragLabel);
                return;
            }

            DataRow dragRow = tempRows[0];
            dragRow["LabelLocationX"] = labelPoint.X;
            dragRow["LabelLocationY"] = labelPoint.Y;

            // Determine if dragged label should be deleted
            if (trashRange[0] <= mouseLocX && mouseLocX <= trashRange[1] && trashRange[2] <= mouseLocY && mouseLocY <= trashRange[3])
            {
                Controls.Remove(dragLabel);
                dispTable.Rows.Remove(dragRow);
            }
            
        }

        private void FileBrowserButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog filePath = profileFileBrowser;
            if (filePath.ShowDialog() == DialogResult.OK)
            {
                fileCB.Items.Add(filePath.SelectedPath);
            }
        }

        private void FileEditSetCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (fileEditSetCheck.Checked)
            {
                fileCB.Enabled = true;
            }
            else
            {
                fileCB.Enabled = false;
            }
        }
    }
}