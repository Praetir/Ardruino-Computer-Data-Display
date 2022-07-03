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

namespace Arduino_Computer_Data_Display
{
    public partial class DispEditForm : Form
    {
        // Initialize data table for display information
        public DataTable dispTable;

        // Initialize trash can range (1st and 2nd values are for x, 3rd and 4th for y)
        public int[] trashRange = new int[4];

        // Initialize display area range
        public int[] dispRange = new int[4];

        // File path string for folderpaths.txt
        public string folderPaths;

        // Initialize dictionary for profile names and profile
        public Dictionary<string, string> profDict = new Dictionary<string, string>();

        public DispEditForm()
        {
            InitializeComponent();

            // Adjust size of display area if needed
            dispArea.Width = ACDDForm.numHorPix;
            dispArea.Height = ACDDForm.numVertPix;
            Console.WriteLine(dispArea.Width);
            Console.WriteLine(dispArea.Height);

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
            dispTable.Columns.Add(new DataColumn("DispArea"));
            dispTable.Columns.Add(new DataColumn("DispX"));
            dispTable.Columns.Add(new DataColumn("DispY"));

            // Get trash can range
            Point trashCorner = trashPicBox.Location;
            Size trashSize = trashPicBox.Size;
            trashRange[0] = trashCorner.X;
            trashRange[1] = trashRange[0] + trashSize.Width;
            trashRange[2] = trashCorner.Y;
            trashRange[3] = trashRange[2] + trashSize.Height;

            // Get display area range
            Point dispCorner = dispArea.Location;
            Size dispSize = dispArea.Size;
            dispRange[0] = dispCorner.X;
            dispRange[1] = dispRange[0] + dispSize.Width;
            dispRange[2] = dispCorner.Y;
            dispRange[3] = dispRange[2] + dispSize.Height;

            dataGridView1.DataSource = dispTable; // Temporary for viewing if data table is working properly

            // Get path to to folderpaths.txt
            folderPaths = System.IO.Path.Combine(ACDDForm.programFolder, "folderpaths.txt");

            // Add profile folders to combobox
            folderCB.Items.AddRange(System.IO.File.ReadAllLines(folderPaths));

            // Set folder paths combobox based on last set folder
            if (Properties.Settings.Default.LastProfileFolder != "")
            {
                folderCB.Text = Properties.Settings.Default.LastProfileFolder;
                folderCB.SelectedItem = folderCB.Text;
            }
        }
        
        private void DispEditForm_Load(object sender, EventArgs e)
        {
            
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
            
            dispTable.Rows.Add(clName, index, labelDisp.Name, labelDisp.Text, labelDisp.Font.Name, labelDisp.Font.Size, labelDisp.Location.X, labelDisp.Location.Y, false);

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

            // Update label coordinates to data table
            DataRow dragRow = tempRows[0];
            dragRow["LabelLocationX"] = labelPoint.X;
            dragRow["LabelLocationY"] = labelPoint.Y;

            // Size of label for display area determination
            Size labelSize = dragLabel.Size;

            // Determine if dragged label should be deleted
            if (trashRange[0] <= mouseLocX && mouseLocX <= trashRange[1] && trashRange[2] <= mouseLocY && mouseLocY <= trashRange[3])
            {
                Controls.Remove(dragLabel);
                dispTable.Rows.Remove(dragRow);
            }
            // Determine if label is within the display area by using the upper left and lower right points of the label
            else if (dispRange[0] <= labelPoint.X && (labelPoint.X + labelSize.Width) <= dispRange[1] && dispRange[2] <= labelPoint.Y && (labelPoint.Y + labelSize.Height) <= dispRange[3])
            {
                // Update if the label is within the display area
                dragRow["DispArea"] = true;

                // Add relative coordinates of label within display area
                dragRow["DispX"] = labelPoint.X - dispRange[0];
                dragRow["DispY"] = labelPoint.Y - dispRange[2];
            }
            // Set display area boolean to false
            else
            {
                dragRow["DispArea"] = false;
            }
            
        }

        private void FolderBrowserButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderPath = profileFolderBrowser;
            if (folderPath.ShowDialog() == DialogResult.OK)
            {
                folderCB.Items.Add(folderPath.SelectedPath);
                UpdateFolderTextFile();
            }
        }

        private void FolderEditSetCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (folderEditSetCheck.Checked)
            {
                // Enable combobox and delete button
                folderCB.Enabled = true;
                fileDeleteButton.Enabled = true;
            }
            else
            {
                // Disable combobox and delete button
                folderCB.Enabled = false;
                fileDeleteButton.Enabled = false;

                // Check if there is a valid profile path selected
                string currentFolder = (string)folderCB.SelectedItem;
                if (System.IO.Directory.Exists(currentFolder))
                {
                    // Load profile files into combobox if selected folder is different
                    if (currentFolder != Properties.Settings.Default.LastProfileFolder)
                    {
                        // Clear profiles and declare string for text lines
                        profileCB.Items.Clear();
                        profDict.Clear();
                        string[] lines;
                        string line = "";
                        string fileName = "";
                        string fileNameNoExt = "";

                        // Check each file and only add text files with specific header line
                        foreach (string filePath in System.IO.Directory.GetFiles(currentFolder))
                        {
                            // Get file name with extension
                            fileName = System.IO.Path.GetFileName(filePath);
                            if (fileName.Contains(".txt"))
                            {
                                lines = System.IO.File.ReadAllLines(filePath);
                                // Check if file has any lines before proceeding
                                if (lines.Length > 0)
                                {
                                    // Use first line to determine if this is a file to use for a profile
                                    line = lines[0];
                                    if (line.Contains("ACDD Profile"))
                                    {
                                        fileNameNoExt = System.IO.Path.GetFileNameWithoutExtension(filePath);
                                        // Add file path and file name to dictionary
                                        profDict.Add(fileNameNoExt, filePath);
                                        profileCB.Items.Add(fileNameNoExt);
                                    }
                                }

                            }
                        }
                    }

                    // Update last profile folder
                    Properties.Settings.Default.LastProfileFolder = currentFolder;
                    //Console.WriteLine(Properties.Settings.Default.LastProfileFolder);
                }
            }
        }

        private void FolderDeleteButton_Click(object sender, EventArgs e)
        {
            int checkIndex = folderCB.SelectedIndex;
            if (checkIndex != 0)
            {
                folderCB.Items.Remove(folderCB.SelectedItem);
                UpdateFolderTextFile();
            }
        }

        private void FolderCB_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Update combobox if enter key has been pressed and contents are a valid directory
            if ((e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Return) && System.IO.Directory.Exists(folderCB.Text))
            {
                folderCB.Items.Add(folderCB.Text);
                UpdateFolderTextFile();
            }
        }

        private void UpdateFolderTextFile()
        {
            // Only do if there is more than the default folder in the combobox
            if (folderCB.Items.Count > 1)
            {
                // Get first folder after default folder
                string firstFolder = folderCB.Items[1].ToString();

                // Overwrite with first new folder
                System.IO.File.WriteAllText(folderPaths, firstFolder, Encoding.UTF8);
                
                // Add rest of folders if there are more to add
                if (folderCB.Items.Count > 2)
                {
                    string[] folders = new string[folderCB.Items.Count];
                    for (int i = 2; i < folderCB.Items.Count; i++)
                    {
                        folders[i] = folderCB.Items[i].ToString();
                    }
                    System.IO.File.AppendAllLines(folderPaths, folders, Encoding.UTF8);
                }
            }
        }

        private void ProfileCB_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow numbers and letters in the text box
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                Console.WriteLine(e.KeyChar);
            }
            // Update combobox and create file if enter key has been pressed
            else if ((e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Return) && System.IO.Directory.Exists((string)folderCB.SelectedItem))
            {
                // Check is combobox contains profile of the same name and do nothing if it does
                if (!profileCB.Items.Contains(profileCB.Text))
                {
                    profileCB.Items.Add(profileCB.Text);
                    string filePath = System.IO.Path.Combine((string)folderCB.SelectedItem, profileCB.Text + ".txt");
                    System.IO.File.WriteAllText(filePath, "ACDD Profile", Encoding.UTF8);
                    profDict.Add(profileCB.Text, filePath);
                }
            }
        }

        private void DispDeleteButton_Click(object sender, EventArgs e)
        {
            // Get file path from dictionary
            string profileName = (string)profileCB.SelectedItem;
            System.IO.File.Delete(profDict[profileName]);
            profileCB.Items.Remove(profileName);
            profDict.Remove(profileName);
            profileCB.Text = "";
        }
    }
}