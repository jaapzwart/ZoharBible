using AVFoundation;
using Foundation;
using System.IO;
using NAudio.Wave;


namespace ZoharBible
{
    public interface IAudioService
    {
        void StartRecording();
        void StopRecording();
        void PlayRecording();
        void StopPlayback();
        string GetFilePath();

        void IncreaseMicrophoneSensitivity();
        void ConvertM4AtoWAV(string filePath, string outfilePath);
    }
    
    public class AudioService : IAudioService
    {
        private readonly string _filePath;
        private AVAudioRecorder _recorder;
        private AVAudioPlayer _player;

        public AudioService()
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, "recording.wav");
        }

        public void StartRecording()
        {
            var audioSession = AVAudioSession.SharedInstance();
            audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
            audioSession.SetActive(true);

            // Update settings for WAV recording
            var settings = new AudioSettings
            {
                SampleRate = 44100, // Standard WAV sample rate
                Format = AudioToolbox.AudioFormatType.LinearPCM, // Linear PCM format for WAV
                NumberChannels = 1, // Mono recording
                AudioQuality = AVAudioQuality.High, // High-quality audio
                LinearPcmBitDepth = 16, // 16-bit samples
                LinearPcmBigEndian = false, // Little-endian byte order
                LinearPcmFloat = false // PCM integer format
            };

            // Create the recorder with the updated settings
            _recorder = AVAudioRecorder.Create(new NSUrl(_filePath, false), settings, out var error);

            if (error != null)
            {
                throw new Exception($"Failed to start recording: {error.LocalizedDescription}");
            }

            _recorder.Record();
        }

        public void StopRecording()
        {
            if (_recorder != null)
            {
                _recorder.Stop();
                _recorder.Dispose();
                _recorder = null;
            }
        }

        public void PlayRecording()
        {
            if (File.Exists(_filePath))
            {
                _player = AVAudioPlayer.FromUrl(new NSUrl(_filePath, false));
                _player.PrepareToPlay();
                _player.Play();
            }
            else
            {
                throw new FileNotFoundException("Recording file not found.");
            }
        }

        public void StopPlayback()
        {
            if (_player != null)
            {
                _player.Stop();
                _player.Dispose();
                _player = null;
            }
        }
        public void IncreaseMicrophoneSensitivity()
        {
            var audioSession = AVAudioSession.SharedInstance();
            audioSession.SetCategory(AVAudioSessionCategory.Record, AVAudioSessionCategoryOptions.DefaultToSpeaker);
            audioSession.SetMode(AVAudioSession.ModeMeasurement, out var error);

            if (error != null)
            {
                throw new Exception(error.LocalizedDescription);
            }

            audioSession.SetActive(true);
        }
        public void ConvertM4AtoWAV(string inputFile, string outputFile)
        {
            using var reader = new MediaFoundationReader(inputFile);
            using var waveStream = WaveFormatConversionStream.CreatePcmStream(reader);
            WaveFileWriter.CreateWaveFile(outputFile, waveStream);
        }

        public string GetFilePath() => _filePath;
    }
}
