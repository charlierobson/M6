using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using M6.Classes;

namespace M6
{
    public partial class M6Form : Form
    {
        [DllImport("libmad.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern int ToRaw(string input, string output);

        public M6Form()
        {
            InitializeComponent();
        }

        private void M6Form_Load(object sender, System.EventArgs e)
        {
            var error = ToRaw(@"C:\Users\Administrator\99s.mp3", @"c:\temp.raw");

            var builderFactory = new BuilderFactory();
            var builder = builderFactory.GetBuilderFor(@"c:\temp.raw");
            var waveData = builder.Build();
            var summary = new WaveSummary();
            var summaryData = summary.MakeSummaryData(waveData, 1024);

            var bitmap = new Bitmap(summaryData.Length, 250, PixelFormat.Format24bppRgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.CadetBlue);

            var x = 0;
            foreach (var m in summaryData.Left)
            {
                var h = 240*m;
                var d = (bitmap.Height - h) / 2;
                graphics.DrawLine(Pens.Black, x, d, x, bitmap.Height - d);
                ++x;
            }

            bitmap.Save(@"C:\Users\Administrator\99s.png");
        }
    }
}
