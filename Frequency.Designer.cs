namespace Notepad
{
    partial class Frequency
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
            this.oneMinute = new System.Windows.Forms.RadioButton();
            this.twoMinutes = new System.Windows.Forms.RadioButton();
            this.fiveMinutes = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // oneMinute
            // 
            this.oneMinute.AutoSize = true;
            this.oneMinute.Location = new System.Drawing.Point(13, 13);
            this.oneMinute.Name = "oneMinute";
            this.oneMinute.Size = new System.Drawing.Size(74, 19);
            this.oneMinute.TabIndex = 0;
            this.oneMinute.TabStop = true;
            this.oneMinute.Text = "1 минута";
            this.oneMinute.UseVisualStyleBackColor = true;
            this.oneMinute.Click += new System.EventHandler(this.OneMinute_Click);
            // 
            // twoMinutes
            // 
            this.twoMinutes.AutoSize = true;
            this.twoMinutes.Location = new System.Drawing.Point(13, 39);
            this.twoMinutes.Name = "twoMinutes";
            this.twoMinutes.Size = new System.Drawing.Size(77, 19);
            this.twoMinutes.TabIndex = 1;
            this.twoMinutes.TabStop = true;
            this.twoMinutes.Text = "2 минуты";
            this.twoMinutes.UseVisualStyleBackColor = true;
            this.twoMinutes.Click += new System.EventHandler(this.TwoMinutes_Click);
            // 
            // fiveMinutes
            // 
            this.fiveMinutes.AutoSize = true;
            this.fiveMinutes.Location = new System.Drawing.Point(13, 65);
            this.fiveMinutes.Name = "fiveMinutes";
            this.fiveMinutes.Size = new System.Drawing.Size(68, 19);
            this.fiveMinutes.TabIndex = 2;
            this.fiveMinutes.TabStop = true;
            this.fiveMinutes.Text = "5 минут";
            this.fiveMinutes.UseVisualStyleBackColor = true;
            this.fiveMinutes.Click += new System.EventHandler(this.FiveMinutes_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(157, 78);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // Frequency
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 113);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.fiveMinutes);
            this.Controls.Add(this.twoMinutes);
            this.Controls.Add(this.oneMinute);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Frequency";
            this.Text = "Frequency";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton oneMinute;
        private System.Windows.Forms.RadioButton twoMinutes;
        private System.Windows.Forms.RadioButton fiveMinutes;
        private System.Windows.Forms.Button button1;
    }
}