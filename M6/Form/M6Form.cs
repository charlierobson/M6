using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using M6.Classes;

namespace M6.Form
{
    public partial class M6Form : System.Windows.Forms.Form
    {
        public M6Form()
        {
            InitializeComponent();
        }

        private void M6Form_Load(object sender, EventArgs e)
        {
            On_Load();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // stop the flicker
        }

        private void M6Form_Paint(object sender, PaintEventArgs e)
        {
            On_Paint(e);
        }

        private void M6Form_ResizeEnd(object sender, EventArgs e)
        {
            On_ResizeEnd();
        }

        private void M6Form_MouseDown(object sender, MouseEventArgs e)
        {
            On_MouseDown(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            On_MouseWheel(e);
        }

        private void M6Form_MouseMove(object sender, MouseEventArgs e)
        {
            On_MouseMove(e);
        }

        private void M6Form_MouseUp(object sender, MouseEventArgs e)
        {
            On_MouseUp();
        }

        private void buttonFit_Click(object sender, EventArgs e)
        {
            On_buttonFit_Click();
        }
    }
}
