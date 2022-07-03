namespace Arduino_Computer_Data_Display
{
    partial class DispEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTabs = new System.Windows.Forms.TabControl();
            this.CPU = new System.Windows.Forms.TabPage();
            this.tempCPULabel = new System.Windows.Forms.Label();
            this.coreCPUText = new System.Windows.Forms.TextBox();
            this.coreCPULabel = new System.Windows.Forms.Label();
            this.tempCPUCL = new System.Windows.Forms.CheckedListBox();
            this.dispCPUCheck = new System.Windows.Forms.CheckBox();
            this.GPUTab = new System.Windows.Forms.TabPage();
            this.ProfilesTab = new System.Windows.Forms.TabPage();
            this.folderEditSetCheck = new System.Windows.Forms.CheckBox();
            this.folderCB = new System.Windows.Forms.ComboBox();
            this.fileBrowserButton = new System.Windows.Forms.Button();
            this.fileDeleteButton = new System.Windows.Forms.Button();
            this.fileLabel = new System.Windows.Forms.Label();
            this.dispDeleteButton = new System.Windows.Forms.Button();
            this.dispLoadButton = new System.Windows.Forms.Button();
            this.profileLabel = new System.Windows.Forms.Label();
            this.profileCB = new System.Windows.Forms.ComboBox();
            this.dispSaveButton = new System.Windows.Forms.Button();
            this.dispSetTips = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.profileFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.trashPicBox = new System.Windows.Forms.PictureBox();
            this.dispArea = new System.Windows.Forms.PictureBox();
            this.toolTabs.SuspendLayout();
            this.CPU.SuspendLayout();
            this.ProfilesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trashPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dispArea)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTabs
            // 
            this.toolTabs.Controls.Add(this.CPU);
            this.toolTabs.Controls.Add(this.GPUTab);
            this.toolTabs.Controls.Add(this.ProfilesTab);
            this.toolTabs.Location = new System.Drawing.Point(12, 12);
            this.toolTabs.Name = "toolTabs";
            this.toolTabs.SelectedIndex = 0;
            this.toolTabs.Size = new System.Drawing.Size(610, 187);
            this.toolTabs.TabIndex = 0;
            // 
            // CPU
            // 
            this.CPU.AutoScroll = true;
            this.CPU.Controls.Add(this.tempCPULabel);
            this.CPU.Controls.Add(this.coreCPUText);
            this.CPU.Controls.Add(this.coreCPULabel);
            this.CPU.Controls.Add(this.tempCPUCL);
            this.CPU.Controls.Add(this.dispCPUCheck);
            this.CPU.Location = new System.Drawing.Point(4, 22);
            this.CPU.Name = "CPU";
            this.CPU.Padding = new System.Windows.Forms.Padding(3);
            this.CPU.Size = new System.Drawing.Size(602, 161);
            this.CPU.TabIndex = 0;
            this.CPU.Text = "CPU";
            this.CPU.UseVisualStyleBackColor = true;
            // 
            // tempCPULabel
            // 
            this.tempCPULabel.AutoSize = true;
            this.tempCPULabel.Location = new System.Drawing.Point(14, 42);
            this.tempCPULabel.Name = "tempCPULabel";
            this.tempCPULabel.Size = new System.Drawing.Size(67, 13);
            this.tempCPULabel.TabIndex = 3;
            this.tempCPULabel.Text = "Temperature";
            // 
            // coreCPUText
            // 
            this.coreCPUText.Enabled = false;
            this.coreCPUText.Location = new System.Drawing.Point(169, 4);
            this.coreCPUText.MaxLength = 2;
            this.coreCPUText.Name = "coreCPUText";
            this.coreCPUText.ShortcutsEnabled = false;
            this.coreCPUText.Size = new System.Drawing.Size(22, 20);
            this.coreCPUText.TabIndex = 2;
            this.coreCPUText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CoreCPUText_KeyPress);
            // 
            // coreCPULabel
            // 
            this.coreCPULabel.AutoSize = true;
            this.coreCPULabel.Location = new System.Drawing.Point(92, 7);
            this.coreCPULabel.Name = "coreCPULabel";
            this.coreCPULabel.Size = new System.Drawing.Size(71, 13);
            this.coreCPULabel.TabIndex = 1;
            this.coreCPULabel.Text = "Core Amount:";
            this.dispSetTips.SetToolTip(this.coreCPULabel, "Choose how many cores you wish to show.\r\nWill always show the first (#) of cores." +
        " \r\nPress enter to update.");
            // 
            // tempCPUCL
            // 
            this.tempCPUCL.Enabled = false;
            this.tempCPUCL.FormattingEnabled = true;
            this.tempCPUCL.Items.AddRange(new object[] {
            "CPU Package",
            "CPU Core Average"});
            this.tempCPUCL.Location = new System.Drawing.Point(17, 58);
            this.tempCPUCL.Name = "tempCPUCL";
            this.tempCPUCL.Size = new System.Drawing.Size(138, 79);
            this.tempCPUCL.TabIndex = 0;
            this.tempCPUCL.Tag = "temp";
            this.tempCPUCL.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CL_ItemCheck);
            // 
            // dispCPUCheck
            // 
            this.dispCPUCheck.AutoSize = true;
            this.dispCPUCheck.Location = new System.Drawing.Point(6, 6);
            this.dispCPUCheck.Name = "dispCPUCheck";
            this.dispCPUCheck.Size = new System.Drawing.Size(65, 17);
            this.dispCPUCheck.TabIndex = 0;
            this.dispCPUCheck.Text = "Enabled";
            this.dispCPUCheck.UseVisualStyleBackColor = true;
            this.dispCPUCheck.CheckedChanged += new System.EventHandler(this.DispCPUCheck_CheckedChanged);
            // 
            // GPUTab
            // 
            this.GPUTab.Location = new System.Drawing.Point(4, 22);
            this.GPUTab.Name = "GPUTab";
            this.GPUTab.Padding = new System.Windows.Forms.Padding(3);
            this.GPUTab.Size = new System.Drawing.Size(602, 161);
            this.GPUTab.TabIndex = 1;
            this.GPUTab.Text = "GPU";
            this.GPUTab.UseVisualStyleBackColor = true;
            // 
            // ProfilesTab
            // 
            this.ProfilesTab.Controls.Add(this.folderEditSetCheck);
            this.ProfilesTab.Controls.Add(this.folderCB);
            this.ProfilesTab.Controls.Add(this.fileBrowserButton);
            this.ProfilesTab.Controls.Add(this.fileDeleteButton);
            this.ProfilesTab.Controls.Add(this.fileLabel);
            this.ProfilesTab.Controls.Add(this.dispDeleteButton);
            this.ProfilesTab.Controls.Add(this.dispLoadButton);
            this.ProfilesTab.Controls.Add(this.profileLabel);
            this.ProfilesTab.Controls.Add(this.profileCB);
            this.ProfilesTab.Controls.Add(this.dispSaveButton);
            this.ProfilesTab.Location = new System.Drawing.Point(4, 22);
            this.ProfilesTab.Margin = new System.Windows.Forms.Padding(2);
            this.ProfilesTab.Name = "ProfilesTab";
            this.ProfilesTab.Padding = new System.Windows.Forms.Padding(2);
            this.ProfilesTab.Size = new System.Drawing.Size(602, 161);
            this.ProfilesTab.TabIndex = 2;
            this.ProfilesTab.Text = "Profiles";
            this.ProfilesTab.UseVisualStyleBackColor = true;
            // 
            // folderEditSetCheck
            // 
            this.folderEditSetCheck.AutoSize = true;
            this.folderEditSetCheck.Location = new System.Drawing.Point(326, 102);
            this.folderEditSetCheck.Margin = new System.Windows.Forms.Padding(2);
            this.folderEditSetCheck.Name = "folderEditSetCheck";
            this.folderEditSetCheck.Size = new System.Drawing.Size(90, 17);
            this.folderEditSetCheck.TabIndex = 17;
            this.folderEditSetCheck.Text = "Edit/Set Path";
            this.folderEditSetCheck.UseVisualStyleBackColor = true;
            this.folderEditSetCheck.CheckedChanged += new System.EventHandler(this.FolderEditSetCheck_CheckedChanged);
            // 
            // folderCB
            // 
            this.folderCB.Enabled = false;
            this.folderCB.FormattingEnabled = true;
            this.folderCB.Items.AddRange(new object[] {
            "C:\\Users\\William\\source\\repos\\Arduino-Computer-Data-Display\\Arduino Computer Data" +
                " Display\\Profiles"});
            this.folderCB.Location = new System.Drawing.Point(95, 100);
            this.folderCB.Margin = new System.Windows.Forms.Padding(2);
            this.folderCB.Name = "folderCB";
            this.folderCB.Size = new System.Drawing.Size(218, 21);
            this.folderCB.TabIndex = 16;
            this.folderCB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FolderCB_KeyPress);
            // 
            // fileBrowserButton
            // 
            this.fileBrowserButton.Location = new System.Drawing.Point(415, 97);
            this.fileBrowserButton.Margin = new System.Windows.Forms.Padding(2);
            this.fileBrowserButton.Name = "fileBrowserButton";
            this.fileBrowserButton.Size = new System.Drawing.Size(76, 24);
            this.fileBrowserButton.TabIndex = 15;
            this.fileBrowserButton.Text = "Browser";
            this.fileBrowserButton.UseVisualStyleBackColor = true;
            this.fileBrowserButton.Click += new System.EventHandler(this.FolderBrowserButton_Click);
            // 
            // fileDeleteButton
            // 
            this.fileDeleteButton.Enabled = false;
            this.fileDeleteButton.Location = new System.Drawing.Point(504, 97);
            this.fileDeleteButton.Margin = new System.Windows.Forms.Padding(2);
            this.fileDeleteButton.Name = "fileDeleteButton";
            this.fileDeleteButton.Size = new System.Drawing.Size(76, 24);
            this.fileDeleteButton.TabIndex = 13;
            this.fileDeleteButton.Text = "Delete Path";
            this.fileDeleteButton.UseVisualStyleBackColor = true;
            this.fileDeleteButton.Click += new System.EventHandler(this.FolderDeleteButton_Click);
            // 
            // fileLabel
            // 
            this.fileLabel.AutoSize = true;
            this.fileLabel.Location = new System.Drawing.Point(14, 102);
            this.fileLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fileLabel.Name = "fileLabel";
            this.fileLabel.Size = new System.Drawing.Size(76, 13);
            this.fileLabel.TabIndex = 11;
            this.fileLabel.Text = "Profile Folders:";
            // 
            // dispDeleteButton
            // 
            this.dispDeleteButton.Location = new System.Drawing.Point(476, 39);
            this.dispDeleteButton.Margin = new System.Windows.Forms.Padding(2);
            this.dispDeleteButton.Name = "dispDeleteButton";
            this.dispDeleteButton.Size = new System.Drawing.Size(76, 24);
            this.dispDeleteButton.TabIndex = 10;
            this.dispDeleteButton.Text = "Delete Profile";
            this.dispDeleteButton.UseVisualStyleBackColor = true;
            this.dispDeleteButton.Click += new System.EventHandler(this.DispDeleteButton_Click);
            // 
            // dispLoadButton
            // 
            this.dispLoadButton.Location = new System.Drawing.Point(386, 39);
            this.dispLoadButton.Margin = new System.Windows.Forms.Padding(2);
            this.dispLoadButton.Name = "dispLoadButton";
            this.dispLoadButton.Size = new System.Drawing.Size(74, 24);
            this.dispLoadButton.TabIndex = 6;
            this.dispLoadButton.Text = "Load Profile";
            this.dispLoadButton.UseVisualStyleBackColor = true;
            // 
            // profileLabel
            // 
            this.profileLabel.AutoSize = true;
            this.profileLabel.Location = new System.Drawing.Point(13, 45);
            this.profileLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.profileLabel.Name = "profileLabel";
            this.profileLabel.Size = new System.Drawing.Size(44, 13);
            this.profileLabel.TabIndex = 5;
            this.profileLabel.Text = "Profiles:";
            // 
            // profileCB
            // 
            this.profileCB.FormattingEnabled = true;
            this.profileCB.Location = new System.Drawing.Point(58, 42);
            this.profileCB.Margin = new System.Windows.Forms.Padding(2);
            this.profileCB.Name = "profileCB";
            this.profileCB.Size = new System.Drawing.Size(218, 21);
            this.profileCB.TabIndex = 4;
            this.dispSetTips.SetToolTip(this.profileCB, "Choose your profile here.\r\nPress Enter to create profile.\r\n^^^ This does not save" +
        " anything to the profile.");
            this.profileCB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ProfileCB_KeyPress);
            // 
            // dispSaveButton
            // 
            this.dispSaveButton.Location = new System.Drawing.Point(297, 39);
            this.dispSaveButton.Margin = new System.Windows.Forms.Padding(2);
            this.dispSaveButton.Name = "dispSaveButton";
            this.dispSaveButton.Size = new System.Drawing.Size(74, 24);
            this.dispSaveButton.TabIndex = 3;
            this.dispSaveButton.Text = "Save Profile";
            this.dispSaveButton.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(440, 220);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 2;
            // 
            // trashPicBox
            // 
            this.trashPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.trashPicBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.trashPicBox.Image = global::Arduino_Computer_Data_Display.Properties.Resources.trash_can_icon_28692;
            this.trashPicBox.Location = new System.Drawing.Point(375, 301);
            this.trashPicBox.Margin = new System.Windows.Forms.Padding(2);
            this.trashPicBox.Name = "trashPicBox";
            this.trashPicBox.Size = new System.Drawing.Size(58, 67);
            this.trashPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.trashPicBox.TabIndex = 4;
            this.trashPicBox.TabStop = false;
            // 
            // dispArea
            // 
            this.dispArea.BackColor = System.Drawing.Color.Transparent;
            this.dispArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.dispArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dispArea.Location = new System.Drawing.Point(12, 205);
            this.dispArea.Name = "dispArea";
            this.dispArea.Size = new System.Drawing.Size(128, 128);
            this.dispArea.TabIndex = 5;
            this.dispArea.TabStop = false;
            // 
            // DispEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 389);
            this.Controls.Add(this.dispArea);
            this.Controls.Add(this.trashPicBox);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolTabs);
            this.Name = "DispEditForm";
            this.Text = "Display Settings";
            this.Load += new System.EventHandler(this.DispEditForm_Load);
            this.toolTabs.ResumeLayout(false);
            this.CPU.ResumeLayout(false);
            this.CPU.PerformLayout();
            this.ProfilesTab.ResumeLayout(false);
            this.ProfilesTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trashPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dispArea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl toolTabs;
        private System.Windows.Forms.TabPage CPU;
        private System.Windows.Forms.TabPage GPUTab;
        private System.Windows.Forms.CheckedListBox tempCPUCL;
        private System.Windows.Forms.CheckBox dispCPUCheck;
        private System.Windows.Forms.Label coreCPULabel;
        private System.Windows.Forms.TextBox coreCPUText;
        private System.Windows.Forms.ToolTip dispSetTips;
        private System.Windows.Forms.Label tempCPULabel;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.PictureBox trashPicBox;
        private System.Windows.Forms.TabPage ProfilesTab;
        private System.Windows.Forms.Label profileLabel;
        private System.Windows.Forms.ComboBox profileCB;
        private System.Windows.Forms.Button dispSaveButton;
        private System.Windows.Forms.Button dispLoadButton;
        private System.Windows.Forms.Button dispDeleteButton;
        private System.Windows.Forms.Button fileDeleteButton;
        private System.Windows.Forms.Label fileLabel;
        private System.Windows.Forms.Button fileBrowserButton;
        private System.Windows.Forms.FolderBrowserDialog profileFolderBrowser;
        private System.Windows.Forms.ComboBox folderCB;
        private System.Windows.Forms.CheckBox folderEditSetCheck;
        private System.Windows.Forms.PictureBox dispArea;
    }
}