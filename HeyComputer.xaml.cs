using System.Data;
using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Maui;
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
            // var viewModel = new ImageGeneratorViewModel();
            // this.BindingContext = viewModel;
            
            //MauiApp.CreateBuilder().UseMauiApp<App>().UseMauiCommunityToolkitMediaElement();
            
            GeneratedImage.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / 2;
            VideoPlayer.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / 2;
            
            var viewModel = new MainViewModelChatGPT();
            this.BindingContext = viewModel;
            
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
            this.VideoPlayer.IsVisible = false;
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
                SetImageSource(returN);
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

        private void SetImageSource(string imageReturn)
        {
            if (imageReturn.Contains("Talked Bill"))
            {
                GlobalVars.DallE_Image_string = "Create an image of Bill Gates as the weird Einstein";
                GlobalVars.videoTalked = "Bill";
                if (GlobalVars.AIInteractive)
                {
                    GlobalVars.videoTalked = "Interactive";
                    GlobalVars.videoTalkedText = GlobalVars.AIInteractiveText;
                }
                else
                    GlobalVars.videoTalkedText = "This is the return from Bill Gates";
                OnCreateVideoClipClicked(null, EventArgs.Empty);
            }

            if (imageReturn.Contains("Talked Elon"))
            {
                GlobalVars.DallE_Image_string = "Create an image of Elon Musk as the weird Einstein";
                GlobalVars.videoTalked = "Elon";
                if(GlobalVars.AIInteractive)
                {
                    GlobalVars.videoTalked = "Interactive";
                    GlobalVars.videoTalkedText = GlobalVars.AIInteractiveText;
                }
                else
                    GlobalVars.videoTalkedText = "This is the return from Elon Musk";
                OnCreateVideoClipClicked(null, EventArgs.Empty);
            }

            if (imageReturn.Contains("Talked Gemini"))
            {
                GlobalVars.DallE_Image_string = "Create an image of he crazy Einstein";
                GlobalVars.videoTalked = "Google";
                if(GlobalVars.AIInteractive)
                {
                    GlobalVars.videoTalked = "Interactive";
                    GlobalVars.videoTalkedText = GlobalVars.AIInteractiveText;
                }
                else
                    GlobalVars.videoTalkedText = "This is the return from Google";
                OnCreateVideoClipClicked(null, EventArgs.Empty);
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

        private async void OnAIInteractiveChanged(object? sender, EventArgs e)
        {
            if (GlobalVars.AIInteractive)
            {
                GlobalVars.AIInteractive = false;
                this.CheckboxAIInteractive.IsChecked = false;
            }
            else
            {
                GlobalVars.AIInteractive = true;
                this.CheckboxAIInteractive.IsChecked = true;
            }
        }

        private async void OnCreateVideoClipClicked(object? sender, EventArgs e)
        {
            try
            {
                //-----------------------------------------------------------
                // Down needed to create an animation image on a new device.
                //-----------------------------------------------------------
                this.VideoPlayer.IsVisible = true;
                //GlobalVars.videoTalked = "Bill";
                //GlobalVars.anim = true;
                //-----------------------------------------------------------
                
                string theVideo = "";
                if (GlobalVars.anim)
                    theVideo = await DoVideoClipLipSync.CreateAnimationAvatar(GlobalVars.videoTalkedText);
                else
                    theVideo = await DoVideoClipLipSync.CreateClipAvatar(GlobalVars.videoTalkedText);
                if (GlobalVars.videoFileExists)
                {
                    await PlayVideosInSequenceAsync();
                }
                else
                {
                    string urll = await DoVideoClipLipSync.GetVideoClip(theVideo);
                    await LoadAndPlayVideo(urll);    
                }
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}","OK");
            }
        }
        private async Task PlayVideosInSequenceAsync()
        {
            VideoPlayer.MediaEnded += OnFirstVideoEnded;

            // Start de eerste video
            VideoPlayer.Source = await DoVideoClipLipSync.PlayExistingVideo(GlobalVars.videoFilePath);
            GlobalVars.videoFileExists = false;
            GlobalVars.videoFilePath = "";
            GlobalVars.anim = true;
        }

        private async void OnFirstVideoEnded(object sender, EventArgs e)
        {
            VideoPlayer.MediaEnded -= OnFirstVideoEnded;

            await DoVideoClipLipSync.CreateAnimationAvatar(GlobalVars.videoTalkedText);

            // Start de tweede video
            var localFilePath = await DoVideoClipLipSync.PlayExistingVideo(GlobalVars.videoFilePath);
            VideoPlayer.Source = localFilePath;
            GlobalVars.videoFileExists = false;
            GlobalVars.videoFilePath = "";
            GlobalVars.anim = false;
        }
        
        
        private async void UpdateLabel(string text)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.MessageLabel.IsVisible = true;
                    this.MessageLabel.Text = text;
                });
                await Task.Yield();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Message:" + ex.Message, "OK");
            }
        }

        private async Task LoadAndPlayVideo(string vidUrl)
        {
            try
            {
                string localFilePath = await DoVideoClipLipSync.DownloadVideoAsync(vidUrl);
                VideoPlayer.Source = localFilePath;
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load video: {ex.Message}", "OK");
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