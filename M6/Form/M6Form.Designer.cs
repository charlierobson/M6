namespace M6.Form
{
    partial class M6Form
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
            this.buttonFit = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonFit
            // 
            this.buttonFit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFit.Location = new System.Drawing.Point(969, 462);
            this.buttonFit.Name = "buttonFit";
            this.buttonFit.Size = new System.Drawing.Size(75, 23);
            this.buttonFit.TabIndex = 0;
            this.buttonFit.Text = "Fit";
            this.buttonFit.UseVisualStyleBackColor = true;
            this.buttonFit.Click += new System.EventHandler(this.buttonFit_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlay.Location = new System.Drawing.Point(969, 433);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(75, 23);
            this.buttonPlay.TabIndex = 1;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(969, 404);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // M6Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 497);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.buttonFit);
            this.Name = "M6Form";
            this.Text = "M6";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.M6Form_FormClosing);
            this.Load += new System.EventHandler(this.M6Form_Load);
            this.ResizeEnd += new System.EventHandler(this.M6Form_ResizeEnd);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.M6Form_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.M6Form_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.M6Form_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.M6Form_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonFit;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button button1;
    }
}

