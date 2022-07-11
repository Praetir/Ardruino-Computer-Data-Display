namespace Arduino_Computer_Data_Display
{
    partial class ACDDForm
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerData = new System.Windows.Forms.Timer(this.components);
            this.timerCom = new System.Windows.Forms.Timer(this.components);
            this.ardPort = new System.IO.Ports.SerialPort(this.components);
            this.portCBox = new System.Windows.Forms.ComboBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.buttonOpenPort = new System.Windows.Forms.Button();
            this.buttonClosePort = new System.Windows.Forms.Button();
            this.editDispButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 34);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(604, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // timerData
            // 
            this.timerData.Tick += new System.EventHandler(this.TimerData_Tick);
            // 
            // timerCom
            // 
            this.timerCom.Tick += new System.EventHandler(this.TimerCom_Tick);
            // 
            // ardPort
            // 
            this.ardPort.DtrEnable = true;
            this.ardPort.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(this.ArdPort_ErrorReceived);
            this.ardPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.ArdPort_DataReceived);
            // 
            // portCBox
            // 
            this.portCBox.FormattingEnabled = true;
            this.portCBox.Location = new System.Drawing.Point(86, 6);
            this.portCBox.Name = "portCBox";
            this.portCBox.Size = new System.Drawing.Size(121, 21);
            this.portCBox.TabIndex = 3;
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(12, 9);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(68, 13);
            this.portLabel.TabIndex = 4;
            this.portLabel.Text = "Arduino Port:";
            // 
            // buttonOpenPort
            // 
            this.buttonOpenPort.Location = new System.Drawing.Point(219, 6);
            this.buttonOpenPort.Name = "buttonOpenPort";
            this.buttonOpenPort.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenPort.TabIndex = 5;
            this.buttonOpenPort.Text = "Open";
            this.buttonOpenPort.UseVisualStyleBackColor = true;
            this.buttonOpenPort.Click += new System.EventHandler(this.ButtonOpenPort_Click);
            // 
            // buttonClosePort
            // 
            this.buttonClosePort.Enabled = false;
            this.buttonClosePort.Location = new System.Drawing.Point(300, 6);
            this.buttonClosePort.Name = "buttonClosePort";
            this.buttonClosePort.Size = new System.Drawing.Size(75, 23);
            this.buttonClosePort.TabIndex = 6;
            this.buttonClosePort.Text = "Close";
            this.buttonClosePort.UseVisualStyleBackColor = true;
            this.buttonClosePort.Click += new System.EventHandler(this.ButtonClosePort_Click);
            // 
            // editDispButton
            // 
            this.editDispButton.Location = new System.Drawing.Point(401, 6);
            this.editDispButton.Name = "editDispButton";
            this.editDispButton.Size = new System.Drawing.Size(75, 23);
            this.editDispButton.TabIndex = 7;
            this.editDispButton.Text = "Edit Display";
            this.editDispButton.UseVisualStyleBackColor = true;
            this.editDispButton.Click += new System.EventHandler(this.EditDispButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Enabled = false;
            this.loadButton.Location = new System.Drawing.Point(501, 6);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(91, 23);
            this.loadButton.TabIndex = 8;
            this.loadButton.Text = "Load Settings";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // ACDDForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 56);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.editDispButton);
            this.Controls.Add(this.buttonClosePort);
            this.Controls.Add(this.buttonOpenPort);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.portCBox);
            this.Controls.Add(this.statusStrip1);
            this.Name = "ACDDForm";
            this.Text = "Arduino Computer Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ACDD_FormClosing);
            this.Load += new System.EventHandler(this.ACDD_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Timer timerData;
        private System.Windows.Forms.Timer timerCom;
        private System.IO.Ports.SerialPort ardPort;
        private System.Windows.Forms.ComboBox portCBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Button buttonOpenPort;
        private System.Windows.Forms.Button buttonClosePort;
        private System.Windows.Forms.Button editDispButton;
        private System.Windows.Forms.Button loadButton;
    }
}

