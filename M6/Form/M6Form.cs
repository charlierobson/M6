using System;
using System.Windows.Forms;

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

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            On_buttonPlay_Click(sender);
        }

        private void M6Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_w == null) return;

            _w.Stop();
            _w.Dispose();
            _w = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            On_TestButton();
        }
    }
}
