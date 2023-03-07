
namespace Notepad
{
    partial class ColorTheme
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
            this.buttonLight = new System.Windows.Forms.RadioButton();
            this.buttonDark = new System.Windows.Forms.RadioButton();
            this.buttonBlue = new System.Windows.Forms.RadioButton();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonLight
            // 
            this.buttonLight.AutoSize = true;
            this.buttonLight.Location = new System.Drawing.Point(12, 12);
            this.buttonLight.Name = "buttonLight";
            this.buttonLight.Size = new System.Drawing.Size(52, 19);
            this.buttonLight.TabIndex = 0;
            this.buttonLight.TabStop = true;
            this.buttonLight.Text = "Light";
            this.buttonLight.UseVisualStyleBackColor = true;
            this.buttonLight.Click += new System.EventHandler(this.ButtonLight_Click);
            // 
            // buttonDark
            // 
            this.buttonDark.AutoSize = true;
            this.buttonDark.Location = new System.Drawing.Point(12, 37);
            this.buttonDark.Name = "buttonDark";
            this.buttonDark.Size = new System.Drawing.Size(49, 19);
            this.buttonDark.TabIndex = 1;
            this.buttonDark.TabStop = true;
            this.buttonDark.Text = "Dark";
            this.buttonDark.UseVisualStyleBackColor = true;
            this.buttonDark.Click += new System.EventHandler(this.ButtonDark_Click);
            // 
            // buttonBlue
            // 
            this.buttonBlue.AutoSize = true;
            this.buttonBlue.Location = new System.Drawing.Point(12, 62);
            this.buttonBlue.Name = "buttonBlue";
            this.buttonBlue.Size = new System.Drawing.Size(48, 19);
            this.buttonBlue.TabIndex = 2;
            this.buttonBlue.TabStop = true;
            this.buttonBlue.Text = "Blue";
            this.buttonBlue.UseVisualStyleBackColor = true;
            this.buttonBlue.Click += new System.EventHandler(this.ButtonBlue_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(161, 65);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // ColorTheme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 100);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonBlue);
            this.Controls.Add(this.buttonDark);
            this.Controls.Add(this.buttonLight);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ColorTheme";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Color Theme";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton buttonLight;
        private System.Windows.Forms.RadioButton buttonDark;
        private System.Windows.Forms.RadioButton buttonBlue;
        private System.Windows.Forms.Button buttonOk;
    }
}