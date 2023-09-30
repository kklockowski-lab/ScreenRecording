using NAudio.Wave;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RecordingScreen
{
    public class Recording
    {

        private const string FfmpegPath = "ffmpeg.exe"; // Podaj pełną ścieżkę do pliku ffmpeg.exe
        private const string otupAviResult = "output\\result.mp4"; // Ścieżka do pliku wynikowego
        private const string otupWavResult = "output\\result.wav"; // Ścieżka do pliku wynikowego
        private const string outputResultAvu = "output\\final_video.mp4"; // Ścieżka do pliku wynikowego


        private WaveInEvent waveSource;
        private WaveFileWriter waveFile;
        Process process;

        internal const int CTRL_C_EVENT = 0;
        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);
        // Delegate type to be used as the Handler Routine for SCCH
        delegate Boolean ConsoleCtrlDelegate(uint CtrlType);


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


        public void Start()
        {
            if (File.Exists(otupAviResult)) File.Delete(otupAviResult);
            if (File.Exists(otupWavResult)) File.Delete(otupWavResult);
            if (File.Exists(outputResultAvu)) File.Delete(outputResultAvu);

            process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = FfmpegPath,
                    Arguments = $"-f gdigrab -framerate 30 -i desktop -c:v libopenh264 -crf 18 -s 1920x1080 {otupAviResult}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                }
            };
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            StartRecording(otupWavResult);
        }

        private void JoinWavAndAvi()
        {
            process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = FfmpegPath,
                    Arguments = $"-i {otupAviResult} -i {otupWavResult} -c:v copy -c:a aac -strict experimental {outputResultAvu}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                }
            };

            process.Start();
            process.WaitForExit();
            
        }

        public void Stop()
        {
            StopRecording();


            if (AttachConsole((uint)process.Id))
            {
                SetConsoleCtrlHandler(null, true);
                try
                {
                    if (!GenerateConsoleCtrlEvent(CTRL_C_EVENT, 0))
                        process.WaitForExit();
                }
                finally
                {
                    SetConsoleCtrlHandler(null, false);
                    FreeConsole();
                }
            }

            JoinWavAndAvi();
        }

        public void KillFFMpegProcess()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = "/F /IM ffmpeg.exe",
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(psi);
        }

    }
}