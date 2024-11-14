using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using AVFoundation;

namespace ZoharBible;

/// <summary>
/// The Proverbs class represents a content page within the Zohar Bible application.
/// It provides functionality to retrieve and display a random proverb from the Bible.
/// </summary>
public partial class Proverbs : ContentPage
{
    public static string translatedText = "";
    public Proverbs()
    {
        InitializeComponent();
        ConfigureAudioSession();
        if (GlobalVars.AiSelected.Contains("GroK"))
        {
            ChatGPTCheckBox.IsChecked = false;
            GroKCheckBox.IsChecked = true;
            GeminiCheckBox.IsChecked = false;
            AllAICheckBox.IsChecked = false;
        }
        if (GlobalVars.AiSelected.Contains("Gemini"))
        {
            ChatGPTCheckBox.IsChecked = false;
            GroKCheckBox.IsChecked = false;
            GeminiCheckBox.IsChecked = true;
            AllAICheckBox.IsChecked = false;
        }
        if (GlobalVars.AiSelected.Contains("ChatGPT"))
        {
            ChatGPTCheckBox.IsChecked = true;
            GroKCheckBox.IsChecked = false;
            GeminiCheckBox.IsChecked = false;
            AllAICheckBox.IsChecked = false;
        }
        if (GlobalVars.AiSelected.Contains("AllAI"))
        {
            ChatGPTCheckBox.IsChecked = false;
            GroKCheckBox.IsChecked = false;
            GeminiCheckBox.IsChecked = false;
            AllAICheckBox.IsChecked = true;
        }
        UpdateLabel("...");
        PartCheckBox.IsChecked = true;
        OnOptionButtonClicked(KabbalahButton, EventArgs.Empty);
    }
    private void ConfigureAudioSession()
    {
        var audioSession = AVAudioSession.SharedInstance();
        audioSession.SetCategory(AVAudioSessionCategory.Playback);
        //audioSession.SetMode(AVAudioSessionMode.SpokenAudio);
        audioSession.SetActive(true);
    }
    private void OnProverbEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        var editor = sender as Editor;
        if (editor == null) return;

        var lineCount = editor.Text.Length;
        GetAnalysisButton.IsEnabled = lineCount > 100;
        StartSpeak.IsEnabled = lineCount > 100;
        StopSpeak.IsEnabled = lineCount > 100;
    }
    /// <summary>
    /// Handles the click event of the "Get Zohar Explanation" button.
    /// When clicked, it displays a message indicating that a proverb is being retrieved,
    /// waits for 2 seconds, and then calls a REST API to get and display a random proverb from the Bible.
    /// </summary>
    /// <param name="sender">The source of the event, typically the button that was clicked.</param>
    /// <param name="e">An EventArgs object that contains the event data.</param>
    private async void OnGetZoharExplanationButtonClicked(object? sender, EventArgs e)
    {
        this.MessageLabel.IsVisible = true;
        UpdateLabel("Getting Verse");
        await Task.Delay(1000);
        string responseText = "";
        this.ProverbEditor.Text = "";
        if (this.ProverbNumberEntry.Text.Equals("0"))
        {
            if (PartCheckBox.IsChecked)
            {
                if(GlobalVars._ProverbOrPsalm.Contains("Proverbs"))
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/BibleProverbsPart");
                else
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/BiblePsalmsPart");
                translatedText = await Translator.TranslateTextToGiven(responseText);
            }
            else
            {
                if(GlobalVars._ProverbOrPsalm.Contains("Proverbs"))
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/BibleProverbs");
                else
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/BiblePsalms");
                    
                
                translatedText = await Translator.TranslateTextToGiven(responseText);
            }
            
        }
        else
        {
            if (PartCheckBox.IsChecked)
            {
                if(GlobalVars._ProverbOrPsalm.Contains("Proverbs"))
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/BibleProverbsPart/EN" +
                                                                       this.ProverbNumberEntry.Text);
                else
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/BiblePsalmsPart/EN" +
                                                                           this.ProverbNumberEntry.Text);
                    
                
                translatedText = await Translator.TranslateTextToGiven(responseText);
            }
            else
            {
                if(GlobalVars._ProverbOrPsalm.Contains("Proverbs"))
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/BibleProverbs/EN" +
                                                                       this.ProverbNumberEntry.Text); 
                else
                    responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/BiblePsalms/EN" +
                                                                           this.ProverbNumberEntry.Text);
                translatedText = await Translator.TranslateTextToGiven(responseText);
            }
            
        }
        this.ProverbEditor.Text = translatedText;
        GlobalVars.ProverbToAnalyse = ExtractProverbsAndNumber(responseText); // Save proverb number to analyse
        UpdateLabel("...");
    }
    static string ExtractProverbsAndNumber(string input)
    {
        string ww = "";
        ww = GlobalVars._ProverbOrPsalm.Contains("Proverbs") ? "Proverbs" : "Psalms";
        // Regular expression to match "Proverbs" followed by a space and a number
        Regex regex = new Regex(@"(" + ww + @")\s(\d+)");
        var match = regex.Match(input);

        return match.Success ?
            // Return the matched string
            match.Value : "No match found";
    }

    /// <summary>
    /// Updates the text of the MessageLabel on the UI thread and ensures the label is visible.
    /// </summary>
    /// <param name="text">The new text to be updated in the MessageLabel.</param>
    private async void UpdateLabel(string text)
    {
        Device.BeginInvokeOnMainThread(() => 
        {
            this.MessageLabel.IsVisible = true;
            this.MessageLabel.Text = text;
        });
        await Task.Yield();
        
    }

    private void OnOptionButtonClicked(object sender, EventArgs e)
    {
        var clickedButton = sender as Button;
        GlobalVars.TypeOfProverbAnalysis = clickedButton.Text; // Save the type of analysis for Proverbs
        if (GlobalVars.TypeOfProverbAnalysis.Contains(("All")))
            GlobalVars.TypeOfProverbAnalysis = " Kabbalah and the Zohar and the Mishna";
        foreach (var child in (clickedButton.Parent as StackLayout).Children)
        {
            if (child is Button button && button != clickedButton && button != ResetButton)
            {
                button.IsEnabled = false;
            }
        }
    }

    private void OnResetButtonClicked(object sender, EventArgs e)
    {
        KabbalahButton.IsEnabled = true;
        ZoharButton.IsEnabled = true;
        MishnaButton.IsEnabled = true;
        AllButton.IsEnabled = true;
    }
    private async void OnGetAnalysisButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            ChangeButtonColorTemporarily(button, System.Drawing.Color.DarkRed, System.Drawing.Color.LightBlue);
            // Voeg hier de overige logica voor de knop toe.
        }
        this.MessageLabel.IsVisible = true;
        UpdateLabel("Preparing Analysis");
        await Task.Delay(1000);
        await Navigation.PushAsync(new ChatAnalysis());
        UpdateLabel("...");
    }
    private async void OnStartSpeakClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            ChangeButtonColorTemporarily(button, System.Drawing.Color.DarkRed, System.Drawing.Color.LightBlue);
            // Voeg hier de overige logica voor de knop toe.
        }
        await GlobalVars.ttsService.ConvertTextToSpeechAsync(translatedText);
    }
    
    private async void OnStopSpeakClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            ChangeButtonColorTemporarily(button, System.Drawing.Color.DarkRed, System.Drawing.Color.LightBlue);
            // Voeg hier de overige logica voor de knop toe.
        }
        await GlobalVars.ttsService.StopSpeakingAsync();
    }
    private async void ChangeButtonColorTemporarily(Button button, System.Drawing.Color temporaryColor, System.Drawing.Color originalColor)
    {
        button.BackgroundColor = Colors.DarkRed;
        await Task.Delay(1000); // Wacht een seconde.
        button.BackgroundColor = Colors.LightBlue;
    }
    private void OnProverbNumberEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(e.NewTextValue, out int number))
        {
            if (number > 31)
            {
                ProverbNumberEntry.Text = "31";
            }
        }
        else
        {
            // Optioneel: Handelen in geval van ongeldige invoer, bijvoorbeeld:
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                ProverbNumberEntry.Text = "0";
            }
        }
    }
    void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender == PartCheckBox && PartCheckBox.IsChecked)
        {
            FullCheckBox.IsChecked = false;
        }
        else if (sender == FullCheckBox && FullCheckBox.IsChecked)
        {
            PartCheckBox.IsChecked = false;
        }
    }
    private void OnChatbotCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender == ChatGPTCheckBox && ChatGPTCheckBox.IsChecked)
        {
            GroKCheckBox.IsChecked = false;
            GeminiCheckBox.IsChecked = false;
            AllAICheckBox.IsChecked = false;
            GlobalVars.AiSelected = "ChatGPT";
        }
        else if (sender == GroKCheckBox && GroKCheckBox.IsChecked)
        {
            ChatGPTCheckBox.IsChecked = false;
            GeminiCheckBox.IsChecked = false;
            AllAICheckBox.IsChecked = false;
            GlobalVars.AiSelected = "GroK";
        }
        else if (sender == GeminiCheckBox && GeminiCheckBox.IsChecked)
        {
            ChatGPTCheckBox.IsChecked = false;
            GroKCheckBox.IsChecked = false;
            AllAICheckBox.IsChecked = false;
            GlobalVars.AiSelected = "Gemini";
        }
        else if (sender == AllAICheckBox && AllAICheckBox.IsChecked)
        {
            ChatGPTCheckBox.IsChecked = false;
            GroKCheckBox.IsChecked = false;
            GeminiCheckBox.IsChecked = false;
            GlobalVars.AiSelected = "AllAI";
        }
    }
}