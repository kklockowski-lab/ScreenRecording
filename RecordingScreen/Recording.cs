using NAudio.Wave;
using System.Diagnostics;

namespace RecordingScreen
{
    public class Recording
    {

        private const string FfmpegPath = "ffmpeg.exe"; // Podaj pełną ścieżkę do pliku ffmpeg.exe
        private const string otupAviResult = "output\\result.avi"; // Ścieżka do pliku wynikowego
        private const string otupWavResult = "output\\result.wav"; // Ścieżka do pliku wynikowego
        private const string outputResultAvu = "output\\final_video.avi"; // Ścieżka do pliku wynikowego


        private WaveInEvent waveSource;
        private WaveFileWriter waveFile;
        Process process;

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
                    //Arguments = $"-f gdigrab -framerate 30 -i desktop -b:v 3000k -b:a 192k {otupAviResult}",
                    Arguments = $"-f gdigrab -framerate 30 -i desktop -q:v 2 -s 1920x1080 {otupAviResult}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                }
            };

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
                    Arguments = $"-i {otupAviResult} -i {otupWavResult} -c copy {outputResultAvu}",
                    //Arguments = $"-i output\\result.avi -i output\\result.wav -c copy output\\final_video.avi",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            process.Start();
            // process.WaitForExit();
            ;
        }

        public void Stop()
        {
            StopRecording();
            process.Kill();
            process.WaitForExit();

            JoinWavAndAvi();
        }

    }
}