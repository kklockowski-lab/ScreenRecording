using System;
using System.Diagnostics;
using NAudio.Wave;

namespace ScreenRecording
{

    public partial class Main : Form
    {

        private const string FfmpegPath = "ffmpeg.exe"; // Podaj pe³n¹ œcie¿kê do pliku ffmpeg.exe
        private const string otupAviResult = "output\\result.avi"; // Œcie¿ka do pliku wynikowego
        private const string otupWavResult = "output\\result.wav"; // Œcie¿ka do pliku wynikowego

        public Main()
        {
            InitializeComponent();
        }
        private WaveInEvent waveSource;
        private WaveFileWriter waveFile;

        public void StartRecording(string filePath)
        {
            waveSource = new WaveInEvent();
            waveSource.WaveFormat = new WaveFormat(44100, 1); // Ustawienia formatu audio (44100 Hz, mono)

            waveFile = new WaveFileWriter(filePath, waveSource.WaveFormat);

            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

            waveSource.StartRecording();
        }

        private void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            waveFile.Write(e.Buffer, 0, e.BytesRecorded);
            waveFile.Flush();
        }

        private void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            waveSource.Dispose();
            waveFile.Dispose();
        }

        public void StopRecording()
        {
            waveSource.StopRecording();
        }

        Process process;
        private void btnStartRec_Click(object sender, EventArgs e)
        {

            process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-f gdigrab -framerate 30 -i desktop {otupAviResult}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                }
            };

            process.Start();
            StartRecording(otupWavResult);

        }


        private void btnStopRec_Click(object sender, EventArgs e)
        {
            StopRecording();
            process.Kill();
            process.WaitForExit();

        }
    }
}