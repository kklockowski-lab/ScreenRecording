using Gma.System.MouseKeyHook;
using RecordingScreen;
using System.Diagnostics;

namespace ScreenRecording
{

    public partial class Main : Form
    {

        Recording recording;
        private static IKeyboardMouseEvents m_GlobalHook;

        Thread thr = new Thread(new ThreadStart(startMouse));

        private static void startMouse()
        {
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point clickedPoint = new Point(e.X, e.Y);
                    Marker.DrawMarker(clickedPoint, 50);
                }
            };
        }

        public Main()
        {
            InitializeComponent();

           startMouse();

        }




        private void btnStartRec_Click(object sender, EventArgs e)
        {
            recording = new Recording();
            recording.Start();

            WindowHooker.StartHook();
        }


        private void btnStopRec_Click(object sender, EventArgs e)
        {
            recording.Stop();
            m_GlobalHook?.Dispose();
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}