using Gma.System.MouseKeyHook;
using RecordingScreen;
using System.Diagnostics;

namespace ScreenRecording
{

    public partial class Main : Form
    {

        static Recording recording;
        private static IKeyboardMouseEvents m_GlobalHook;

        Thread thr = new Thread(new ThreadStart(startRecording));

        static object syncObj = new object();

        private static void startRecording()
        {
            recording = new Recording();
            recording.Start();
        }

        private void startMouse()
        {
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point clickedPoint = new Point(e.X, e.Y);
                    Marker.DrawMarker(clickedPoint, 30);
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
            thr.Start();
        }


        private void btnStopRec_Click(object sender, EventArgs e)
        {
            lock (syncObj)
            {
                recording.Stop();
            }
            thr.Abort();
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