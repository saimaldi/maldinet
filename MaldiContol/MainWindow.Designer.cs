
namespace MaldiContol
{
    partial class MainWindow
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
            this.LastMessage = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.PumpSampleInletButton = new System.Windows.Forms.Button();
            this.XMotorPositionTextBox = new System.Windows.Forms.TextBox();
            this.XMotorPositionLabel = new System.Windows.Forms.Label();
            this.YMotorPositionTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LockStatusTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PumpingStateTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LastMessage
            // 
            this.LastMessage.Location = new System.Drawing.Point(54, 51);
            this.LastMessage.Name = "LastMessage";
            this.LastMessage.Size = new System.Drawing.Size(717, 20);
            this.LastMessage.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(54, 204);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Exchange Sample";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ExchangeSampleClicked);
            // 
            // PumpSampleInletButton
            // 
            this.PumpSampleInletButton.Location = new System.Drawing.Point(231, 204);
            this.PumpSampleInletButton.Name = "PumpSampleInletButton";
            this.PumpSampleInletButton.Size = new System.Drawing.Size(142, 23);
            this.PumpSampleInletButton.TabIndex = 2;
            this.PumpSampleInletButton.Text = "Pump Sample Inlet";
            this.PumpSampleInletButton.UseVisualStyleBackColor = true;
            this.PumpSampleInletButton.Click += new System.EventHandler(this.PumpSampleInletButton_Click);
            // 
            // XMotorPositionTextBox
            // 
            this.XMotorPositionTextBox.Location = new System.Drawing.Point(54, 153);
            this.XMotorPositionTextBox.Name = "XMotorPositionTextBox";
            this.XMotorPositionTextBox.Size = new System.Drawing.Size(168, 20);
            this.XMotorPositionTextBox.TabIndex = 3;
            // 
            // XMotorPositionLabel
            // 
            this.XMotorPositionLabel.AutoSize = true;
            this.XMotorPositionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XMotorPositionLabel.Location = new System.Drawing.Point(83, 137);
            this.XMotorPositionLabel.Name = "XMotorPositionLabel";
            this.XMotorPositionLabel.Size = new System.Drawing.Size(100, 13);
            this.XMotorPositionLabel.TabIndex = 4;
            this.XMotorPositionLabel.Text = "X Motor Position";
            // 
            // YMotorPositionTextBox
            // 
            this.YMotorPositionTextBox.Location = new System.Drawing.Point(280, 153);
            this.YMotorPositionTextBox.Name = "YMotorPositionTextBox";
            this.YMotorPositionTextBox.Size = new System.Drawing.Size(168, 20);
            this.YMotorPositionTextBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(321, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Y Motor Position";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(51, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Latest Message";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(51, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Lock Seal Status";
            // 
            // LockStatusTextBox
            // 
            this.LockStatusTextBox.Location = new System.Drawing.Point(54, 103);
            this.LockStatusTextBox.Name = "LockStatusTextBox";
            this.LockStatusTextBox.Size = new System.Drawing.Size(221, 20);
            this.LockStatusTextBox.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(317, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Pumping State";
            // 
            // PumpingStateTextBox
            // 
            this.PumpingStateTextBox.Location = new System.Drawing.Point(320, 103);
            this.PumpingStateTextBox.Name = "PumpingStateTextBox";
            this.PumpingStateTextBox.Size = new System.Drawing.Size(221, 20);
            this.PumpingStateTextBox.TabIndex = 11;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PumpingStateTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LockStatusTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.YMotorPositionTextBox);
            this.Controls.Add(this.XMotorPositionLabel);
            this.Controls.Add(this.XMotorPositionTextBox);
            this.Controls.Add(this.PumpSampleInletButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LastMessage);
            this.Name = "MainWindow";
            this.Text = "MainWIndow";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LastMessage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button PumpSampleInletButton;
        private System.Windows.Forms.TextBox XMotorPositionTextBox;
        private System.Windows.Forms.Label XMotorPositionLabel;
        private System.Windows.Forms.TextBox YMotorPositionTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox LockStatusTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PumpingStateTextBox;
    }
}

