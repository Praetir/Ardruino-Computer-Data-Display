namespace Ardruino_Computer_Data_Display
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
            this.hardwareTabs = new System.Windows.Forms.TabControl();
            this.CPU = new System.Windows.Forms.TabPage();
            this.tempCPULabel = new System.Windows.Forms.Label();
            this.coreCPUText = new System.Windows.Forms.TextBox();
            this.coreCPULabel = new System.Windows.Forms.Label();
            this.tempCPUCL = new System.Windows.Forms.CheckedListBox();
            this.dispCPUCheck = new System.Windows.Forms.CheckBox();
            this.GPUTab = new System.Windows.Forms.TabPage();
            this.dispSetTips = new System.Windows.Forms.ToolTip(this.components);
            this.dispArea = new System.Windows.Forms.PictureBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dispSaveButton = new System.Windows.Forms.Button();
            this.trashPicBox = new System.Windows.Forms.PictureBox();
            this.hardwareTabs.SuspendLayout();
            this.CPU.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dispArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trashPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // hardwareTabs
            // 
            this.hardwareTabs.Controls.Add(this.CPU);
            this.hardwareTabs.Controls.Add(this.GPUTab);
            this.hardwareTabs.Location = new System.Drawing.Point(16, 15);
            this.hardwareTabs.Margin = new System.Windows.Forms.Padding(4);
            this.hardwareTabs.Name = "hardwareTabs";
            this.hardwareTabs.SelectedIndex = 0;
            this.hardwareTabs.Size = new System.Drawing.Size(813, 230);
            this.hardwareTabs.TabIndex = 0;
            // 
            // CPU
            // 
            this.CPU.AutoScroll = true;
            this.CPU.Controls.Add(this.tempCPULabel);
            this.CPU.Controls.Add(this.coreCPUText);
            this.CPU.Controls.Add(this.coreCPULabel);
            this.CPU.Controls.Add(this.tempCPUCL);
            this.CPU.Controls.Add(this.dispCPUCheck);
            this.CPU.Location = new System.Drawing.Point(4, 25);
            this.CPU.Margin = new System.Windows.Forms.Padding(4);
            this.CPU.Name = "CPU";
            this.CPU.Padding = new System.Windows.Forms.Padding(4);
            this.CPU.Size = new System.Drawing.Size(805, 201);
            this.CPU.TabIndex = 0;
            this.CPU.Text = "CPU";
            this.CPU.UseVisualStyleBackColor = true;
            // 
            // tempCPULabel
            // 
            this.tempCPULabel.AutoSize = true;
            this.tempCPULabel.Location = new System.Drawing.Point(19, 52);
            this.tempCPULabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tempCPULabel.Name = "tempCPULabel";
            this.tempCPULabel.Size = new System.Drawing.Size(90, 17);
            this.tempCPULabel.TabIndex = 3;
            this.tempCPULabel.Text = "Temperature";
            // 
            // coreCPUText
            // 
            this.coreCPUText.Enabled = false;
            this.coreCPUText.Location = new System.Drawing.Point(225, 5);
            this.coreCPUText.Margin = new System.Windows.Forms.Padding(4);
            this.coreCPUText.MaxLength = 2;
            this.coreCPUText.Name = "coreCPUText";
            this.coreCPUText.ShortcutsEnabled = false;
            this.coreCPUText.Size = new System.Drawing.Size(28, 22);
            this.coreCPUText.TabIndex = 2;
            this.coreCPUText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CoreCPUText_KeyPress);
            // 
            // coreCPULabel
            // 
            this.coreCPULabel.AutoSize = true;
            this.coreCPULabel.Location = new System.Drawing.Point(123, 9);
            this.coreCPULabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.coreCPULabel.Name = "coreCPULabel";
            this.coreCPULabel.Size = new System.Drawing.Size(94, 17);
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
            this.tempCPUCL.Location = new System.Drawing.Point(23, 71);
            this.tempCPUCL.Margin = new System.Windows.Forms.Padding(4);
            this.tempCPUCL.Name = "tempCPUCL";
            this.tempCPUCL.Size = new System.Drawing.Size(183, 106);
            this.tempCPUCL.TabIndex = 0;
            this.tempCPUCL.Tag = "temp";
            this.tempCPUCL.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CL_ItemCheck);
            // 
            // dispCPUCheck
            // 
            this.dispCPUCheck.AutoSize = true;
            this.dispCPUCheck.Location = new System.Drawing.Point(8, 7);
            this.dispCPUCheck.Margin = new System.Windows.Forms.Padding(4);
            this.dispCPUCheck.Name = "dispCPUCheck";
            this.dispCPUCheck.Size = new System.Drawing.Size(82, 21);
            this.dispCPUCheck.TabIndex = 0;
            this.dispCPUCheck.Text = "Enabled";
            this.dispCPUCheck.UseVisualStyleBackColor = true;
            this.dispCPUCheck.CheckedChanged += new System.EventHandler(this.DispCPUCheck_CheckedChanged);
            // 
            // GPUTab
            // 
            this.GPUTab.Location = new System.Drawing.Point(4, 25);
            this.GPUTab.Margin = new System.Windows.Forms.Padding(4);
            this.GPUTab.Name = "GPUTab";
            this.GPUTab.Padding = new System.Windows.Forms.Padding(4);
            this.GPUTab.Size = new System.Drawing.Size(805, 201);
            this.GPUTab.TabIndex = 1;
            this.GPUTab.Text = "GPU";
            this.GPUTab.UseVisualStyleBackColor = true;
            // 
            // dispArea
            // 
            this.dispArea.BackColor = System.Drawing.Color.Transparent;
            this.dispArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dispArea.Location = new System.Drawing.Point(27, 271);
            this.dispArea.Margin = new System.Windows.Forms.Padding(4);
            this.dispArea.Name = "dispArea";
            this.dispArea.Size = new System.Drawing.Size(170, 157);
            this.dispArea.TabIndex = 1;
            this.dispArea.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(587, 271);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(320, 185);
            this.dataGridView1.TabIndex = 2;
            // 
            // dispSaveButton
            // 
            this.dispSaveButton.Location = new System.Drawing.Point(27, 435);
            this.dispSaveButton.Name = "dispSaveButton";
            this.dispSaveButton.Size = new System.Drawing.Size(170, 30);
            this.dispSaveButton.TabIndex = 3;
            this.dispSaveButton.Text = "Save Display";
            this.dispSaveButton.UseVisualStyleBackColor = true;
            // 
            // trashPicBox
            // 
            this.trashPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.trashPicBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.trashPicBox.Image = global::Ardruino_Computer_Data_Display.Properties.Resources.trash_can_icon_28692;
            this.trashPicBox.Location = new System.Drawing.Point(500, 370);
            this.trashPicBox.Name = "trashPicBox";
            this.trashPicBox.Size = new System.Drawing.Size(77, 83);
            this.trashPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.trashPicBox.TabIndex = 4;
            this.trashPicBox.TabStop = false;
            // 
            // DispEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 479);
            this.Controls.Add(this.trashPicBox);
            this.Controls.Add(this.dispSaveButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.dispArea);
            this.Controls.Add(this.hardwareTabs);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DispEditForm";
            this.Text = "Display Settings";
            this.Load += new System.EventHandler(this.DispEditForm_Load);
            this.hardwareTabs.ResumeLayout(false);
            this.CPU.ResumeLayout(false);
            this.CPU.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dispArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trashPicBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl hardwareTabs;
        private System.Windows.Forms.TabPage CPU;
        private System.Windows.Forms.TabPage GPUTab;
        private System.Windows.Forms.CheckedListBox tempCPUCL;
        private System.Windows.Forms.CheckBox dispCPUCheck;
        private System.Windows.Forms.Label coreCPULabel;
        private System.Windows.Forms.TextBox coreCPUText;
        private System.Windows.Forms.ToolTip dispSetTips;
        private System.Windows.Forms.Label tempCPULabel;
        private System.Windows.Forms.PictureBox dispArea;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button dispSaveButton;
        private System.Windows.Forms.PictureBox trashPicBox;
    }
}