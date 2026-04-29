using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Logai
{
    public partial class Form1 : Form
    {
        private const int CornerRadius = 18;
        private const int CsDropShadow = 0x00020000;
        private const int HitCaption = 0x2;
        private const int HitClient = 0x1;
        private const int WmNchitTest = 0x84;

        private static readonly Color ThemeColor = Color.FromArgb(15, 15, 15);
        private static readonly Color TextColor = Color.FromArgb(240, 240, 240);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        public Form1()
        {
            InitializeComponent();
            ApplyModernWindowStyle();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ClassStyle |= CsDropShadow;
                return createParams;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ApplyRoundedCorners();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WmNchitTest && m.Result == (IntPtr)HitClient)
            {
                m.Result = (IntPtr)HitCaption;
            }
        }

        private void ApplyModernWindowStyle()
        {
            BackColor = ThemeColor;
            ForeColor = TextColor;
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            ApplyRoundedCorners();
        }

        private void ApplyRoundedCorners()
        {
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            IntPtr regionHandle = CreateRoundRectRgn(
                0,
                0,
                Width + 1,
                Height + 1,
                CornerRadius,
                CornerRadius);

            if (regionHandle == IntPtr.Zero)
            {
                return;
            }

            try
            {
                Region roundedRegion = Region.FromHrgn(regionHandle);
                Region previousRegion = Region;

                Region = roundedRegion;
                previousRegion?.Dispose();
            }
            finally
            {
                DeleteObject(regionHandle);
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
