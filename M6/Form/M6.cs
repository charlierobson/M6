using System;
using System.Windows.Forms;

namespace M6.Form
{
    static class M6
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new M6Form());
        }
    }
}
