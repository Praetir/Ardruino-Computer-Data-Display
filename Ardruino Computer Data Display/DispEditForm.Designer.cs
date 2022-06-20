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
            this.coreCPUText = new System.Windows.Forms.TextBox();
            this.coreCPULabel = new System.Windows.Forms.Label();
            this.tempCPUCL = new System.Windows.Forms.CheckedListBox();
            this.dispCPU = new System.Windows.Forms.CheckBox();
            this.GPUTab = new System.Windows.Forms.TabPage();
            this.coreCPUTip = new System.Windows.Forms.ToolTip(this.components);
            this.hardwareTabs.SuspendLayout();
            this.CPU.SuspendLayout();
            this.SuspendLayout();
            // 
            // hardwareTabs
            // 
            this.hardwareTabs.Controls.Add(this.CPU);
            this.hardwareTabs.Controls.Add(this.GPUTab);
            this.hardwareTabs.Location = new System.Drawing.Point(12, 12);
            this.hardwareTabs.Name = "hardwareTabs";
            this.hardwareTabs.SelectedIndex = 0;
            this.hardwareTabs.Size = new System.Drawing.Size(692, 296);
            this.hardwareTabs.TabIndex = 0;
            // 
            // CPU
            // 
            this.CPU.AutoScroll = true;
            this.CPU.Controls.Add(this.coreCPUText);
            this.CPU.Controls.Add(this.coreCPULabel);
            this.CPU.Controls.Add(this.tempCPUCL);
            this.CPU.Controls.Add(this.dispCPU);
            this.CPU.Location = new System.Drawing.Point(4, 22);
            this.CPU.Name = "CPU";
            this.CPU.Padding = new System.Windows.Forms.Padding(3);
            this.CPU.Size = new System.Drawing.Size(684, 270);
            this.CPU.TabIndex = 0;
            this.CPU.Text = "CPU";
            this.CPU.UseVisualStyleBackColor = true;
            // 
            // coreCPUText
            // 
            this.coreCPUText.Enabled = false;
            this.coreCPUText.Location = new System.Drawing.Point(83, 42);
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
            this.coreCPULabel.Location = new System.Drawing.Point(6, 45);
            this.coreCPULabel.Name = "coreCPULabel";
            this.coreCPULabel.Size = new System.Drawing.Size(71, 13);
            this.coreCPULabel.TabIndex = 1;
            this.coreCPULabel.Text = "Core Amount:";
            this.coreCPUTip.SetToolTip(this.coreCPULabel, "Choose how many cores you wish to show.\r\nWill always show the first (#) of cores." +
        " ");
            // 
            // tempCPUCL
            // 
            this.tempCPUCL.Enabled = false;
            this.tempCPUCL.FormattingEnabled = true;
            this.tempCPUCL.Items.AddRange(new object[] {
            "CPU Package",
            "CPU Core Average"});
            this.tempCPUCL.Location = new System.Drawing.Point(9, 68);
            this.tempCPUCL.Name = "tempCPUCL";
            this.tempCPUCL.Size = new System.Drawing.Size(120, 94);
            this.tempCPUCL.TabIndex = 0;
            // 
            // dispCPU
            // 
            this.dispCPU.AutoSize = true;
            this.dispCPU.Location = new System.Drawing.Point(6, 6);
            this.dispCPU.Name = "dispCPU";
            this.dispCPU.Size = new System.Drawing.Size(65, 17);
            this.dispCPU.TabIndex = 0;
            this.dispCPU.Text = "Enabled";
            this.dispCPU.UseVisualStyleBackColor = true;
            this.dispCPU.CheckedChanged += new System.EventHandler(this.DispCPU_CheckedChanged);
            // 
            // GPUTab
            // 
            this.GPUTab.Location = new System.Drawing.Point(4, 22);
            this.GPUTab.Name = "GPUTab";
            this.GPUTab.Padding = new System.Windows.Forms.Padding(3);
            this.GPUTab.Size = new System.Drawing.Size(684, 270);
            this.GPUTab.TabIndex = 1;
            this.GPUTab.Text = "GPU";
            this.GPUTab.UseVisualStyleBackColor = true;
            // 
            // DispEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.hardwareTabs);
            this.Name = "DispEditForm";
            this.Text = "Display Editing";
            this.hardwareTabs.ResumeLayout(false);
            this.CPU.ResumeLayout(false);
            this.CPU.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl hardwareTabs;
        private System.Windows.Forms.TabPage CPU;
        private System.Windows.Forms.TabPage GPUTab;
        private System.Windows.Forms.CheckedListBox tempCPUCL;
        private System.Windows.Forms.CheckBox dispCPU;
        private System.Windows.Forms.Label coreCPULabel;
        private System.Windows.Forms.TextBox coreCPUText;
        private System.Windows.Forms.ToolTip coreCPUTip;
    }
}