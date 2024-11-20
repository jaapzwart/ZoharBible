using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;
using AVFoundation;

namespace ZoharBible;

/// <summary>
/// The ChatAnalysis class is a content page within the Zohar Bible application, designed
/// to retrieve and display a text-based analysis of a specific proverb.
/// </summary>
public partial class ChatAnalysis : ContentPage
{
    /// <summary>
    /// Holds the translated text returned from the translation service.
    /// This variable is used to store the content which is to be displayed
    /// in the ChatAnalysis text editor after being translated from the
    /// original response obtained from the AI service APIs.
    /// </summary>
    private string translatedText = "";

    /// <summary>
    /// The ChatAnalysis class is a content page within the Zohar Bible application.
    /// It provides functionality to retrieve and display a text-based analysis of a specific proverb.
    /// </summary>
    public ChatAnalysis()
    {
        InitializeComponent();
        OnButtonClicked(this, EventArgs.Empty);
        if (GlobalVars.Amida_.Contains("Parshat"))
            this.messageLabel.Text = GlobalVars._pPortion;
        ConfigureAudioSession();
    }

    /// <summary>
    /// Configures the audio session for the application, setting the category to Playback
    /// and activating the session. This is essential for proper audio functionality within the app,
    /// particularly for playback scenarios.
    /// </summary>
    private void ConfigureAudioSession()
    {
        try
        {
            var audioSession = AVAudioSession.SharedInstance();
            audioSession.SetCategory(AVAudioSessionCategory.Playback);
            //audioSession.SetMode(AVAudioSessionMode.SpokenAudio);
            audioSession.SetActive(true);
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "Failed to configure audio session: " + ex.Message, "OK");
        }
    }

    /// <summary>
    /// Event handler for button click events in the ChatAnalysis page.
    /// This method is triggered when a button is clicked, and it initiates the process
    /// of fetching the translated text analysis of a specified proverb.
    /// </summary>
    /// <param name="sender">The source of the event. Typically, this is the Button that was clicked.</param>
    /// <param name="args">Provides data for the event.</param>
    private async void OnButtonClicked(object sender, EventArgs args)
    {
        try
        {
            await GetTranslatedText();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
        }
    }
    /// <summary>
    /// Retrieves and translates text-based analysis of a specific Jewish prayer or proverb.
    /// The text is fetched from an external API based on the selected AI service and
    /// parameters defined in global variables.
    /// </summary>
    /// <returns>Task that represents the asynchronous operation of fetching and translating the text.</returns>
    private async Task GetTranslatedText()
    {
        try
        {
            string responseText = "";
            string qq = "";
            
            #region Init Amida Shema and Proverb
            if (GlobalVars.Amida_.Contains("Amida") || GlobalVars.Amida_.Contains("Shema"))
            {
                qq = "Show all the prayers with content of the Jewish " + GlobalVars.Amida_ +
                     " and after that give a deep analysis of the Jewish" +
                     GlobalVars.Amida_ +
                     " and add to it how you can apply it to your behavior for the day, towards your family, friends and other people.";
            }

            string qp = "";
            string hAdd = "";
            if (GlobalVars._pPortion.Contains("Horoscope"))
            {
                qp = "Give an analysis on " +
                            GlobalVars._pPortion + " for the current date" +
                            " and for the period of a " + GlobalVars.HPeriod;
                hAdd = "(" + GlobalVars._pPortion + " - " + GlobalVars.HPeriod + ")";
            }
            else
            {
                qp = "Give an analysis on " +
                     GlobalVars.ProverbToAnalyse +
                     " from out the perspective of the " +
                     GlobalVars.TypeOfProverbAnalysis;
            }

            #endregion
            
            #region Amida and Shema
            if (GlobalVars.Amida_.Contains("Amida") || GlobalVars.Amida_.Contains("Shema"))
            {
                if (GlobalVars.AiSelected.Contains("ChatGPT"))
                {
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                        Secrets.RESTAPI + @"ChatGPT/"
                        + qq);
                    translatedText = await Translator.TranslateTextToGiven(responseText);
                    this.ChatAnalysisText.Text = "ChatGPT: " + '\n' + translatedText;
                }
                else if (GlobalVars.AiSelected.Contains("GroK"))
                {
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                        Secrets.RESTAPI + @"ChatGrok/"
                        + qq);
                    translatedText = await Translator.TranslateTextToGiven(responseText);
                    this.ChatAnalysisText.Text = "GroK: " + '\n' + translatedText.TrimStart().Replace("***", "")
                        .Replace("###", "")
                        .Replace("**", "").Replace("*", "");
                }
                else if (GlobalVars.AiSelected.Contains("Gemini"))
                {
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                        Secrets.RESTAPI + @"ChatGPT/"
                        + qq);
                    translatedText = await Translator.TranslateTextToGiven(responseText);
                    this.ChatAnalysisText.Text = "Gemini: " + '\n' + translatedText.TrimStart();
                }
                else if (GlobalVars.AiSelected.Contains("AllAI"))
                {
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                        Secrets.RESTAPI + @"ChatGPT/"
                        + qq);
                    translatedText = await Translator.TranslateTextToGiven(responseText);
                    this.ChatAnalysisText.Text = "ChatGPT: " + '\n' + translatedText.TrimStart() + '\n' + '\n';
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                        Secrets.RESTAPI + @"ChatGrok/"
                        + qq);
                    translatedText = await Translator.TranslateTextToGiven(responseText);
                    this.ChatAnalysisText.Text += "Grok: " + '\n' + translatedText.TrimStart() + '\n' + '\n';
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                        Secrets.RESTAPI + @"ChatGPT/"
                        + qq);
                    translatedText = await Translator.TranslateTextToGiven(responseText);
                    this.ChatAnalysisText.Text += "Gemini: " + '\n' + translatedText.TrimStart();
                }
            }
            #endregion
            
            #region Proverb analysis
            else if (GlobalVars.AiSelected.Contains("ChatGPT"))
            {
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    Secrets.RESTAPI + @"ChatGPT/"
                    + qp);
                translatedText = await Translator.TranslateTextToGiven(responseText);
                this.ChatAnalysisText.Text = "ChatGPT: " + hAdd + '\n' + translatedText.TrimStart();
            }
            else if (GlobalVars.AiSelected.Contains("GroK"))
            {
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    Secrets.RESTAPI + @"ChatGrok/"
                    + qp);
                translatedText = await Translator.TranslateTextToGiven(responseText);
                this.ChatAnalysisText.Text = "Grok: " + hAdd + '\n' + translatedText.TrimStart().Replace("***", "")
                    .Replace("###", "")
                    .Replace("**", "").Replace("*", "");
            }
            else if (GlobalVars.AiSelected.Contains("Gemini"))
            {
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(Secrets.RESTAPI + @"Google/"
                                                                       + qp);
                translatedText = await Translator.TranslateTextToGiven(responseText);
                this.ChatAnalysisText.Text = "Gemini: " + hAdd + '\n' + translatedText.TrimStart();
            }
            else if (GlobalVars.AiSelected.Contains("AllAI"))
            {
                this.ChatAnalysisText.Text = "All AI ANalysis: " + '\n' + '\n';
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    Secrets.RESTAPI + @"ChatGPT/"
                    + qp);
                translatedText = await Translator.TranslateTextToGiven(responseText);
                this.ChatAnalysisText.Text += "ChatGPT: " + hAdd + '\n' + translatedText.TrimStart() + '\n' + '\n';
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    Secrets.RESTAPI + @"ChatGrok/"
                    + qp);
                translatedText = await Translator.TranslateTextToGiven(responseText);
                this.ChatAnalysisText.Text += "Grok: " + hAdd + '\n' + translatedText.TrimStart() + '\n' + '\n';
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(Secrets.RESTAPI + @"Google/"
                                                                       + qp);
                translatedText = await Translator.TranslateTextToGiven(responseText);
                this.ChatAnalysisText.Text += "Gemini: " + hAdd + '\n' + translatedText.TrimStart();
            }
            #endregion
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred while fetching or translating the text: " + ex.Message,
                "OK");
        }
    }

    /// <summary>
    /// Handles the Click event of the Speak button, converting the translated text
    /// to speech using the TextToSpeechService.
    /// </summary>
    /// <param name="sender">The source of the event, typically the button.</param>
    /// <param name="e">Event arguments associated with the Click event.</param>
    private async void OnSpeakButtonClicked(object sender, EventArgs e)
    {
        try
        {
            await GlobalVars.ttsService.ConvertTextToSpeechAsync(translatedText);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred while trying to speak the text: " + ex.Message, "OK");
        }
    }

    /// <summary>
    /// Handles the event when the stop speaking button is clicked to stop the Text-to-Speech service.
    /// </summary>
    /// <param name="sender">The sender object that raised the event.</param>
    /// <param name="e">Event data associated with the event.</param>
    private async void OnStopSpeakButtonClicked(object sender, EventArgs e)
    {
        try
        {
            await GlobalVars.ttsService.StopSpeakingAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred while trying to stop the speech: " + ex.Message, "OK");
        }
    }
}

/// <summary>
/// The Translatetext class provides functionality to translate given text
/// into the language specified in the GlobalVars configuration.
/// </summary>
public static class Translatetext
{
    /// <summary>
    /// Retrieves the translated text for a given input string.
    /// </summary>
    /// <param name="tt">The input text that needs to be translated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the translated text.</returns>
    public static async Task<string> GetTranslatedText(string tt)
    {
        try
        {
            string translatedText = await Translator.TranslateTextToGiven(tt);
            return translatedText;
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "An error occurred while translating the text: " + ex.Message, "OK");
            throw;
        }
    }
}