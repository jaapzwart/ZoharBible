using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using System.IO;

namespace ZoharBible;

public partial class Intro : ContentPage
{
    private readonly IAudioService _audioService;
    public Intro()
    {
        InitializeComponent();
        GlobalVars._StandardTheme = true;
        GlobalVars._IntroPage = true;
    }
    public Intro(IAudioService audioService)
    {
        InitializeComponent();
        GlobalVars._StandardTheme = true;
        GlobalVars._IntroPage = true;
        _audioService = audioService;
        _audioService.IncreaseMicrophoneSensitivity();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        GlobalVars._IntroPage = true;
    }
    private void OnCreativitySliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        double newValue = e.NewValue;
        GlobalVars.ChatGPTTemp = newValue;
        // Update de tekst van het bijbehorende label om de nieuwe waarde van de slider weer te geven
        CreativitySliderValueLabel.Text = $"Creativity: {newValue:F1}";
    }
    private async void OnNavigateToStarterPageClicked(object sender, EventArgs e)
    {
        await GlobalVars.writeFileToBlob(GlobalVars.ChatGPTTemp.ToString("0.0", CultureInfo.InvariantCulture),
            "temperatureChatGPT");
        if(this.StandardThemeCheckBox.IsChecked == false)
            Themes.SetMoodColors(Convert.ToInt32(GlobalVars.moodOMeter));
        await GlobalVars.SetClickedColor(sender);
        UpdateLabel("Opening Starting Page");
        await Task.Delay(1000);
        await Navigation.PushAsync(new StartPage());
    }
    private async void UpdateLabel(string text)
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            this.MessageLabel.IsVisible = true;
            this.MessageLabel.Text = text;
        });
        await Task.Yield();
    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        int roundedValue = (int)Math.Round(e.NewValue / 10.0) * 10;
        GlobalVars.moodOMeter = $"{roundedValue}";
        SliderValueLabel.Text = $"Slider Value: {roundedValue}";
        
    }
    private void OnStandardThemeCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            GlobalVars._StandardTheme = true;
            Themes.SetStandardTheme();
        }
        else
        {
            GlobalVars._StandardTheme = false;
            Themes.SetMoodColors(Convert.ToInt32(GlobalVars.moodOMeter));
        }
    }

    private void DialogueCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        
        GlobalVars._Dialogue = e.Value;
    }
    
    #region recording
    private void OnRecordButtonClicked(object sender, EventArgs e)
    {
        try
        {
            _audioService.StartRecording();
            StatusLabel.Text = "Recording...";
            RecordButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            PlayButton.IsEnabled = false;
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Error: {ex.Message}";
        }
    }

    private void OnStopButtonClicked(object sender, EventArgs e)
    {
        try
        {
            _audioService.StopRecording();
            StatusLabel.Text = "Recording stopped.";
            RecordButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            PlayButton.IsEnabled = true;
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Error: {ex.Message}";
        }
    }

    private void OnPlayButtonClicked(object sender, EventArgs e)
    {
        try
        {
            _audioService.PlayRecording();
            StatusLabel.Text = "Playing recording...";
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Error: {ex.Message}";
        }
    }
    private async void OnTranscribeClicked(object sender, EventArgs e)
    {
        try
        {
            string _filePathIn = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "recording.wav");

            string returN = await AzureSpeechToText.TranscribeAudioAsync(_filePathIn, "James");
            VoiceLabel.Text = returN;
        }
        catch (Exception ex)
        {
            VoiceLabel.Text = $"Error: {ex.Message}";
        }
    }
    #endregion
}