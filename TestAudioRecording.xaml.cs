using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoharBible;

public partial class TestAudioRecording : ContentPage
{
    private readonly IAudioService _audioService;
    public TestAudioRecording()
    {
        InitializeComponent();
        IAudioService audioService = new AudioService();
        _audioService = audioService;
        _audioService.IncreaseMicrophoneSensitivity();
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
            
            if (returN.Contains("Dialogue", StringComparison.OrdinalIgnoreCase) 
                && returN.Contains("Started", StringComparison.OrdinalIgnoreCase))
            {
                VoiceLabel.Text = returN;
                string dSentiment = await GlobalVars.GetHttpReturnFromAPIRestLink(
                    Secrets.RESTAPI + @"ChatGPT/"
                                    + "Tell the user you will start the checkbox called AI dialoque" +
                                    " to enable a dialoque between the AI providers.");
                await GlobalVars.ttsService.ConvertTextToSpeechAsync(dSentiment);
                
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
                
            }
            if (returN.Contains("Command not found"))
            {
                VoiceLabel.Text = returN;
                string dSentiment = await GlobalVars.GetHttpReturnFromAPIRestLink(
                    Secrets.RESTAPI + @"ChatGPT/"
                                    + "Tell the user you did not find the the command valid " +
                                    " and you can not help the user.");
                await GlobalVars.ttsService.ConvertTextToSpeechAsync(dSentiment);
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