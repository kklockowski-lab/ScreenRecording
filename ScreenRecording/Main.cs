using System;
using System.Drawing;
using System.Windows.Forms;
using SharpAvi;
using SharpAvi.Codecs;
using SharpAvi.Output;
using System.Threading;

namespace ScreenRecording
{

    public partial class Main : Form
    {
        private AviWriter writer;
        private IAviVideoStream videoStream;

        Recorder record;

        public Main()
        {
            InitializeComponent();
        }

        private void StartRecording()
        {
            writer = new AviWriter("recorded.avi")
            {
                FramesPerSecond = 30,
                EmitIndex1 = true
            };


        }

        private void StopRecording()
        {
            writer.Close();
        }

        private void btnStartRec_Click(object sender, EventArgs e)
        {
            FourCC selectedCodec = KnownFourCCs.Codecs.MotionJpeg;
            RecorderParams recparams = new RecorderParams("recordvideo.avi", 49, selectedCodec, 100);
            record = new Recorder(recparams);
        }

        private void btnStopRec_Click(object sender, EventArgs e)
        {
            record.Dispose();
        }
    }
}