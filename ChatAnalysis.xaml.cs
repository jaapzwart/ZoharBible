using System.Text;
using Newtonsoft.Json;

namespace ZoharBible;

/// <summary>
/// The Proverbs class represents a content page within the Zohar Bible application.
/// It provides functionality to retrieve and display a random proverb from the Bible.
/// </summary>
public partial class ChatAnalysis : ContentPage
{
    /// <summary>
    /// The ChatAnalysis class is a content page within the Zohar Bible application, designed
    /// to retrieve and display a text-based analysis of a specific proverb.
    /// </summary>
    public ChatAnalysis()
    {
        InitializeComponent();
        string responseText = "";
        if (GlobalVars.Amida_.Contains("Amida"))
        {
            if (GlobalVars.AiSelected.Contains("ChatGPT"))
            {
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    "https://bibleapje.azurewebsites.net/api/ChatGPT/"
                    + "First show the text of the Jewish Amida and after that give a deep analysis of the Jewish Amida.");
                this.ChatAnalysisText.Text = "ChatGPT: " + '\n' + responseText.TrimStart();
            }
            else if (GlobalVars.AiSelected.Contains("GroK"))
            {
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    "https://bibleapje.azurewebsites.net/api/ChatGrok/"
                    + "First show the text of the Jewish Amida and after that give a deep analysis of the Jewish Amida.");
                this.ChatAnalysisText.Text = "GroK: " + '\n' + responseText.TrimStart();
            }
            else if (GlobalVars.AiSelected.Contains("Gemini"))
            {
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    "https://bibleapje.azurewebsites.net/api/Google/"
                    + "First show the text of the Jewish Amida and after that give a deep analysis of the Jewish Amida.");
                this.ChatAnalysisText.Text = "Gemini: " + '\n' + responseText.TrimStart();
            }
            else if (GlobalVars.AiSelected.Contains("AllAI"))
            {
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    "https://bibleapje.azurewebsites.net/api/ChatGPT/"
                    + "First show the text of the Jewish Amida and after that give a deep analysis of the Jewish Amida.");
                this.ChatAnalysisText.Text = "ChatGPT: " + '\n' + responseText.TrimStart() + '\n' + '\n';
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    "https://bibleapje.azurewebsites.net/api/ChatGrok/"
                    + "Give a deep analysis of the Jewish Amida.");
                this.ChatAnalysisText.Text = "Grok: " + '\n' + responseText.TrimStart() + '\n' + '\n';
                responseText = GlobalVars.GetHttpReturnFromAPIRestLink(
                    "https://bibleapje.azurewebsites.net/api/Google/"
                    + "Give a deep analysis of the Jewish Amida.");
                this.ChatAnalysisText.Text = "Gemini: " + '\n' + responseText.TrimStart();
            }
            
        }
        else if (GlobalVars.AiSelected.Contains("ChatGPT"))
        {
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/ChatGPT/"
                                                                   + "Give an analysis on " +
                                                                   GlobalVars.ProverbToAnalyse +
                                                                   " from out the perspective of the " +
                                                                   GlobalVars.TypeOfProverbAnalysis + ".");
            this.ChatAnalysisText.Text = "ChatGPT: " + '\n' + responseText.TrimStart();
        }
        else if(GlobalVars.AiSelected.Contains("GroK"))
        {
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/ChatGrok/"
                                                                    + "Give an analysis on " +
                                                                    GlobalVars.ProverbToAnalyse +
                                                                    " from out the perspective of the " +
                                                                    GlobalVars.TypeOfProverbAnalysis + ".");
            this.ChatAnalysisText.Text = "Grok: " + '\n' + responseText.TrimStart();
        }
        else if(GlobalVars.AiSelected.Contains("Gemini"))
        {
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/Google/"
                                                                   + "Give an analysis on " +
                                                                   GlobalVars.ProverbToAnalyse +
                                                                   " from out the perspective of the " +
                                                                   GlobalVars.TypeOfProverbAnalysis + ".");
            this.ChatAnalysisText.Text = "Gemini: " + '\n' + responseText.TrimStart();
        }
        else if(GlobalVars.AiSelected.Contains("AllAI"))
        {
            this.ChatAnalysisText.Text = "All AI ANalysis: " + '\n' + '\n';
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/ChatGPT/"
                                                                   + "Give an analysis on " +
                                                                   GlobalVars.ProverbToAnalyse +
                                                                   " from out the perspective of the " +
                                                                   GlobalVars.TypeOfProverbAnalysis + ".");
            this.ChatAnalysisText.Text += "ChatGPT: " + '\n' + responseText.TrimStart() + '\n' + '\n';
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/ChatGrok/"
                                                                   + "Give an analysis on " +
                                                                   GlobalVars.ProverbToAnalyse +
                                                                   " from out the perspective of the " +
                                                                   GlobalVars.TypeOfProverbAnalysis + ".");
            this.ChatAnalysisText.Text += "Grok: " + '\n' + responseText.TrimStart() + '\n' + '\n';
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/Google/"
                                                                   + "Give an analysis on " +
                                                                   GlobalVars.ProverbToAnalyse +
                                                                   " from out the perspective of the " +
                                                                   GlobalVars.TypeOfProverbAnalysis + ".");
            this.ChatAnalysisText.Text += "Gemini: " + '\n' + responseText.TrimStart();
        }
    }
    
}