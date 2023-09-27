using System;
using System.Drawing;
using System.Windows.Forms;
using SharpAvi;
using SharpAvi.Codecs;
using SharpAvi.Output;
using System.Threading;
using NAudio.Wave;

namespace ScreenRecording
{

    public partial class Main : Form
    {
        private AviWriter writer;
        private IAviVideoStream videoStream;
        private IAviAudioStream audioStream;

        private WaveInEvent waveIn;


        private WaveFileWriter waveWriter;

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
            //  writer = new AviWriter("recorded.avi")
            //  {
            //      FramesPerSecond = 30,
            //      EmitIndex1 = true
            //  };

            //  //videoStream = writer.AddVideoStream(width: Screen.PrimaryScreen.Bounds.Width,
            //  //                                    height: Screen.PrimaryScreen.Bounds.Height,
            //  //                                        bitsPerPixel: BitsPerPixel.Bpp32);

            //  var waveFormat = new WaveFormat(44100, 16, 1); // Ustawienia formatu dŸwiêku (44.1 kHz, 16-bit, mono)
            //  audioStream = writer.AddAudioStream();

            //  waveIn = new WaveInEvent();
            //  waveIn.WaveFormat = waveFormat;
            //  waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
            //  waveIn.StartRecording();

            ////  writer.Open();
            ///
            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1); // Ustawienia formatu dŸwiêku (44.1 kHz, 16-bit, mono)
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);

            waveWriter = new WaveFileWriter("recordedAudio.wav", waveIn.WaveFormat);

            waveIn.StartRecording();
        }
        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // audioStream.WriteBlock(e.Buffer, 0, e.BytesRecorded);

            waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
        }

        private void btnStopRec_Click(object sender, EventArgs e)
        {
            //waveIn.StopRecording();
            //waveIn.DataAvailable -= new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
            //writer.Close();

            waveIn.StopRecording();
            waveIn.DataAvailable -= new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
            waveWriter.Close();
        }
    }
}