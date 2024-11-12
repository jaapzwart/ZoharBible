using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;
using AVFoundation;

namespace ZoharBible;

/// <summary>
/// The Proverbs class represents a content page within the Zohar Bible application.
/// It provides functionality to retrieve and display a random proverb from the Bible.
/// </summary>
public partial class ChatAnalysis : ContentPage
{
    private string translatedText = "";
    /// <summary>
    /// The ChatAnalysis class is a content page within the Zohar Bible application, designed
    /// to retrieve and display a text-based analysis of a specific proverb.
    /// </summary>
    public ChatAnalysis()
    {
        InitializeComponent();
        OnButtonClicked(this, EventArgs.Empty);
        if(GlobalVars.Amida_.Contains("Parshat"))
            this.messageLabel.Text = GlobalVars._pPortion;
        ConfigureAudioSession();
    }
    private void ConfigureAudioSession()
    {
        var audioSession = AVAudioSession.SharedInstance();
        audioSession.SetCategory(AVAudioSessionCategory.Playback);
        //audioSession.SetMode(AVAudioSessionMode.SpokenAudio);
        audioSession.SetActive(true);
    }
    private async void OnButtonClicked(object sender, EventArgs args)
    {
        await GetTranslatedText();
    }
    private async Task GetTranslatedText()
    {
        string responseText = "";
        string qq = "";
        if (GlobalVars.Amida_.Contains("Amida") || GlobalVars.Amida_.Contains("Shema"))
        {
            qq = "Show all the prayers with content of the Jewish " + GlobalVars.Amida_ + " and after that give a deep analysis of the Jewish" +
                 GlobalVars.Amida_ +
                 " and add to it how you can apply it to your behavior for the day, towards your family, friends and other people.";
        }
        else if (GlobalVars.Amida_.Contains("Parshat"))
        {
            qq = "Give a deep thorough analysis of:" + GlobalVars._pPortion + " from out the perspective of the: " + GlobalVars.Amida_;
        }

        string qp = "Give an analysis on " +
                    GlobalVars.ProverbToAnalyse +
                    " from out the perspective of the " +
                    GlobalVars.TypeOfProverbAnalysis;
        if (GlobalVars.Amida_.Contains("Amida") || GlobalVars.Amida_.Contains("Shema")
            || GlobalVars.Amida_.Contains("Parshat"))
        {
            if (GlobalVars.AiSelected.Contains("ChatGPT"))
            {
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    "https://bibleapje.azurewebsites.net/api/ChatGPT/"
                    +  qq);
                translatedText = await Translator.TranslateTextToGiven(responseText);
                this.ChatAnalysisText.Text = "ChatGPT: " + '\n' + translatedText;
            }
            else if (GlobalVars.AiSelected.Contains("GroK"))
            {
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    "https://bibleapje.azurewebsites.net/api/ChatGrok/"
                    + qq);
                translatedText = await Translator.TranslateTextToGiven(responseText);
                this.ChatAnalysisText.Text = "GroK: " + '\n' + translatedText.TrimStart().Replace("***", "").Replace("###", "")
                    .Replace("**", "").Replace("*","");
            }
            else if (GlobalVars.AiSelected.Contains("Gemini"))
            {
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    "https://bibleapje.azurewebsites.net/api/ChatGPT/"
                    + qq);
                translatedText = await Translator.TranslateTextToGiven(responseText);
                this.ChatAnalysisText.Text = "Gemini: " + '\n' + translatedText.TrimStart();
            }
            else if (GlobalVars.AiSelected.Contains("AllAI"))
            {
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    "https://bibleapje.azurewebsites.net/api/ChatGPT/"
                    + qq);
                translatedText = await Translator.TranslateTextToGiven(responseText);
                this.ChatAnalysisText.Text = "ChatGPT: " + '\n' + translatedText.TrimStart() + '\n' + '\n';
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    "https://bibleapje.azurewebsites.net/api/ChatGrok/"
                    + qq);
                translatedText = await Translator.TranslateTextToGiven(responseText);
                this.ChatAnalysisText.Text += "Grok: " + '\n' + translatedText.TrimStart() + '\n' + '\n';
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    "https://bibleapje.azurewebsites.net/api/ChatGPT/"
                    + qq);
                translatedText = await Translator.TranslateTextToGiven(responseText);
                this.ChatAnalysisText.Text += "Gemini: " + '\n' + translatedText.TrimStart();
            }
            
        }
        else if (GlobalVars.AiSelected.Contains("ChatGPT"))
        {
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/ChatGPT/"
                                                                   + qp);
            translatedText = await Translator.TranslateTextToGiven(responseText);
            this.ChatAnalysisText.Text = "ChatGPT: " + '\n' + translatedText.TrimStart();
        }
        else if(GlobalVars.AiSelected.Contains("GroK"))
        {
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/ChatGrok/"
                                                                    + qp);
            translatedText = await Translator.TranslateTextToGiven(responseText);
            this.ChatAnalysisText.Text = "Grok: " + '\n' + translatedText.TrimStart().Replace("***", "").Replace("###", "")
                .Replace("**", "").Replace("*","");
        }
        else if(GlobalVars.AiSelected.Contains("Gemini"))
        {
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/Google/"
                                                                   + qp);
            translatedText = await Translator.TranslateTextToGiven(responseText);
            this.ChatAnalysisText.Text = "Gemini: " + '\n' + translatedText.TrimStart();
        }
        else if(GlobalVars.AiSelected.Contains("AllAI"))
        {
            this.ChatAnalysisText.Text = "All AI ANalysis: " + '\n' + '\n';
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/ChatGPT/"
                                                                   + qp);
            translatedText = await Translator.TranslateTextToGiven(responseText);
            this.ChatAnalysisText.Text += "ChatGPT: " + '\n' + translatedText.TrimStart() + '\n' + '\n';
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/ChatGrok/"
                                                                   + qp);
            translatedText = await Translator.TranslateTextToGiven(responseText);
            this.ChatAnalysisText.Text += "Grok: " + '\n' + translatedText.TrimStart() + '\n' + '\n';
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/Google/"
                                                                   + qp); 
            translatedText = await Translator.TranslateTextToGiven(responseText);
            this.ChatAnalysisText.Text += "Gemini: " + '\n' + translatedText.TrimStart();
        }
    }
   
    private async void OnSpeakButtonClicked(object sender, EventArgs e)
    {
        await GlobalVars.ttsService.ConvertTextToSpeechAsync(translatedText);
    }
    
    private async void OnStopSpeakButtonClicked(object sender, EventArgs e)
    {
        await GlobalVars.ttsService.StopSpeakingAsync();
    }
}

public static class Translatetext
{
    public static async Task<string> GetTranslatedText(string tt)
    {
        string translatedText = await Translator.TranslateTextToGiven(tt);
        return translatedText;
    }
}