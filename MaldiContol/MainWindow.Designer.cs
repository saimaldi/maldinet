
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
            this.SuspendLayout();
            // 
            // LastMessage
            // 
            this.LastMessage.Location = new System.Drawing.Point(30, 91);
            this.LastMessage.Name = "LastMessage";
            this.LastMessage.Size = new System.Drawing.Size(571, 20);
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
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

