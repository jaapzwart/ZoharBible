using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using System.IO;
using Azure;
using Azure.AI.TextAnalytics;

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
    private async void OnRecordButtonClicked(object sender, EventArgs e)
    {
        try
        {
            _audioService.StartRecording();
            StatusLabel.Text = "Recording...";
            await Task.Delay(1000);
            StatusLabel.Text = "...";
            RecordButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            PlayButton.IsEnabled = false;
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Error: {ex.Message}";
        }
    }

    private async void OnStopButtonClicked(object sender, EventArgs e)
    {
        try
        {
            _audioService.StopRecording();
            StatusLabel.Text = "Recording stopped.";
            await Task.Delay(1000);
            StatusLabel.Text = "...";
            RecordButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            PlayButton.IsEnabled = true;
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Error: {ex.Message}";
        }
    }

    private async void OnPlayButtonClicked(object sender, EventArgs e)
    {
        try
        {
            _audioService.PlayRecording();
            StatusLabel.Text = "Playing recording...";
            await Task.Delay(1000);
            StatusLabel.Text = "...";
            
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
            StatusLabel.Text = "Transcribing...";
            await Task.Delay(1000);
            string _filePathIn = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "recording.wav");
            List<string> keywords = new List<string>();
            keywords.Add("Dialogue");
            keywords.Add("dialogue");
            keywords.Add("Dialog");
            keywords.Add("dialog");
            keywords.Add("Who");
            keywords.Add("who");
            string returN = await AzureSpeechToText.TranscribeAudioAsync(_filePathIn, keywords);
            
            //******************************************************
            // This this be the sensing of the content of the text
            //******************************************************
            // string endpoint = Secrets.wToTClientAzure;
            // string apiKey = Secrets.wToTSubscription;
            // var credential = new AzureKeyCredential(apiKey);
            // var textAnalyticsClient = new TextAnalyticsClient(new Uri(endpoint), credential);
            // var tt = new AngryWordDetector(textAnalyticsClient);
            // await tt.AnalyzeText(returN);
            //******************************************************

            if (returN.Contains("Dialogue", StringComparison.OrdinalIgnoreCase) 
                && returN.Contains("Started", StringComparison.OrdinalIgnoreCase))
            {
                VoiceLabel.Text = returN;
                string dSentiment = await GlobalVars.GetHttpReturnFromAPIRestLink(
                    Secrets.RESTAPI + @"ChatGPT/"
                                    + "Tell the user you will start the checkbox called AI dialoque" +
                                    " to enable a dialoque between the AI providers.");
                await GlobalVars.ttsService.ConvertTextToSpeechAsync(dSentiment);
                this.DialogueCheckBox.IsChecked = true;
            }
            if (returN.Contains("Dialogue", StringComparison.OrdinalIgnoreCase) 
                && returN.Contains("Stopped", StringComparison.OrdinalIgnoreCase))
            {
                VoiceLabel.Text = returN;
                string dSentiment = await GlobalVars.GetHttpReturnFromAPIRestLink(
                    Secrets.RESTAPI + @"ChatGPT/"
                                    + "Tell the user you will stop the checkbox called AI dialoque" +
                                    " to disable the dialoque between the AI providers.");
                await GlobalVars.ttsService.ConvertTextToSpeechAsync(dSentiment);
                this.DialogueCheckBox.IsChecked = false;
            }
            if (returN.Contains("Hello", StringComparison.OrdinalIgnoreCase) 
                && returN.Contains("World", StringComparison.OrdinalIgnoreCase))
            {
                VoiceLabel.Text = returN;
                string dSentiment = await GlobalVars.GetHttpReturnFromAPIRestLink(
                    Secrets.RESTAPI + @"ChatGPT/"
                                    + "Tell the user that this is a new era of the old example of the 'Hello World'" +
                                    " example program that was always used when new programming languages were introduced." +
                                    " Now this current version of the 'Hello Work' is the door to a new world, a world " +
                                    " where AI is your companion.");
                await GlobalVars.ttsService.ConvertTextToSpeechAsync(dSentiment);
                this.DialogueCheckBox.IsChecked = false;
            }
            if (returN.Contains("Command not found"))
            {
                VoiceLabel.Text = returN;
                string dSentiment = await GlobalVars.GetHttpReturnFromAPIRestLink(
                    Secrets.RESTAPI + @"ChatGPT/"
                                    + "Tell the user you did not find the the command valid " +
                                    " and you can not help the user.");
                await GlobalVars.ttsService.ConvertTextToSpeechAsync(dSentiment);
                this.DialogueCheckBox.IsChecked = false;
            }

            VoiceLabel.Text = "...";
            StatusLabel.Text = "...";
        }
        catch (Exception ex)
        {
            VoiceLabel.Text = $"Error: {ex.Message}";
        }
    }
    #endregion
}