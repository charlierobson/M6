namespace M6
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
            // M6Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 497);
            this.Controls.Add(this.buttonFit);
            this.Name = "M6Form";
            this.Text = "M6";
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
    }
}

