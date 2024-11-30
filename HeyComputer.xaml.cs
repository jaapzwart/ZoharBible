using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ZoharBible
{
    public partial class HeyComputer : ContentPage
    {
        private readonly IAudioService _audioService;
        public HeyComputer()
        {
            InitializeComponent();
            IAudioService audioService = new AudioService();
            _audioService = audioService;
            _audioService.IncreaseMicrophoneSensitivity();
            object sender = this.BlinkingButton;
            EventArgs e = new EventArgs();
            OnButtonEntryClicked(sender, e);
        }
        private async void OnButtonEntryClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            if (GlobalVars.HeyComputerStartTalk)
            {
                GlobalVars.HeyComputerStartTalk = false;
                button.BackgroundColor = Color.FromArgb("#00FF00"); 
                button.TextColor = Color.FromArgb("#00008B"); 
                button.BorderColor = Color.FromArgb("#FFFF00");
                button.Text = "Start Talking";
                StatusLabel.Text = "Press button to start recording.";
                while (true)
                {
                    await button.AnimateBorderColor(Color.FromArgb("#00FFFFFF"), 500);
                    await button.AnimateBorderColor(Color.FromArgb("#FFFF00"), 500); // Animeren naar geel
                }
                
            }
            else
            {
                GlobalVars.HeyComputerStartTalk = true;
                button.BackgroundColor = Color.FromArgb("#FF0000");
                button.TextColor = Color.FromArgb("#FFA500");
                button.Text = "Stop Talking";
                StatusLabel.Text = "Press button to stop recording";
                while (true)
                {
                    await button.AnimateBorderColor(Color.FromArgb("#00FFFFFF"), 500);
                    await button.AnimateBorderColor(Color.FromArgb("#FFA500"), 500);
                }
            }
            
        }
        private async void OnButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            if (GlobalVars.HeyComputerStartTalk)
            {
                _audioService.StopRecording();
                GlobalVars.HeyComputerStartTalk = false;
                button.BackgroundColor = Color.FromArgb("#00FF00"); 
                button.TextColor = Color.FromArgb("#00008B"); 
                button.BorderColor = Color.FromArgb("#FFFF00");
                button.Text = "Start Talking";
                StatusLabel.Text = "Press button to start recording.";
                VoiceLabel.Text = "Transcribing";
                string _filePathIn = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "recording.wav");
                List<string> keywords = new List<string>();
                keywords.Add("X");
                keywords.Add("Google");
                keywords.Add("Microsoft");
                keywords.Add("Elon");
                keywords.Add("Gemini");
                keywords.Add("Bill");
                string returN = await AzureSpeechToText.TranscribeAudioAsync(_filePathIn, keywords);
                VoiceLabel.Text = returN;
                while (true)
                {
                    await button.AnimateBorderColor(Color.FromArgb("#00FFFFFF"), 500);
                    await button.AnimateBorderColor(Color.FromArgb("#FFFF00"), 500); // Animeren naar geel
                }
                
            }
            else
            {
                GlobalVars.HeyComputerStartTalk = true;
                button.BackgroundColor = Color.FromArgb("#FF0000");
                button.TextColor = Color.FromArgb("#FFA500");
                button.Text = "Stop Talking";
                _audioService.StartRecording();
                StatusLabel.Text = "Press button to stop recording";
                while (true)
                {
                    await button.AnimateBorderColor(Color.FromArgb("#00FFFFFF"), 500);
                    await button.AnimateBorderColor(Color.FromArgb("#FFA500"), 500);
                }
            }
            
        }
        #region Audio
        private async void OnStopSpeakClicked(object sender, EventArgs e)
        {
            try
            {
                await GlobalVars.ttsService.StopSpeakingAsync();
            }
            catch (Exception ex)
            {
                 await DisplayAlert("Error", $"An error occurred: {ex.Message}","OK");
            }
        }
        #endregion
    }

    public static class AnimationExtensions
    {
        public static async Task AnimateBorderColor(this Button button, Color targetColor, uint duration)
        {
            var animation = new Animation(v =>
            {
                button.BorderColor = Color.FromRgba(v, button.BorderColor.Green, button.BorderColor.Blue, button.BorderColor.Alpha);
            }, button.BorderColor.Red, targetColor.Red);

            animation.Commit(button, "BorderColorAnimation", 16, duration, Easing.Linear, (v, c) => button.BorderColor = targetColor);
            await Task.Delay((int)duration);
        }
    }
}