using System.Runtime.InteropServices.JavaScript;

namespace ZoharBible;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        GlobalVars.AiSelected = "GroK";
    }
    private void OnLanguagePickerChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null)
        {
            GlobalVars.lLanguage_ = picker.SelectedItem as string;
            // Voor debuggen of verdere verwerking
            Console.WriteLine($"Geselecteerde taal: {GlobalVars.lLanguage_}");
        }
    }

    /// <summary>
    /// Handles the Click event of the "Get a random Proverb" button.
    /// Navigates the user to the Proverbs page.
    /// </summary>
    /// <param name="sender">The source of the event. Typically, this is the button that was clicked.</param>
    /// <param name="e">An EventArgs that contains the event data.</param>
    private async void OnGetProverbButtonClicked(object sender, EventArgs e)
    {
        GlobalVars.Amida_ = "";
        await Navigation.PushAsync(new Proverbs());
    }
    private async void OnGetAmidaButtonClicked(object sender, EventArgs e)
    {
        GlobalVars.Amida_ = "Amida";
        this.MessageLabel.IsVisible = true;
        UpdateLabel("Preparing Analysis");
        await Task.Delay(1000);
        await Navigation.PushAsync(new ChatAnalysis());
        UpdateLabel("...");
    }
    private async void OnGetShemaButtonClicked(object sender, EventArgs e)
    {
        GlobalVars.Amida_ = "Shema";
        this.MessageLabel.IsVisible = true;
        UpdateLabel("Preparing Analysis");
        await Task.Delay(1000);
        await Navigation.PushAsync(new ChatAnalysis());
        UpdateLabel("...");
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
    private async void OnShemaTimeButtonClicked(object sender, EventArgs e)
    {
        string _shema = "SHEMA TIME: " + GlobalVars.GetHttpReturnFromAPIRestLink(
            "https://bibleapje.azurewebsites.net/api/Google/"
            + "Give ONLY the time the Shema should be prayed during the day");
        await DisplayAlert("Shema Time", _shema, "OK");
    }

    private async void OnAmidaTimeButtonClicked(object sender, EventArgs e)
    {
        string _amida = "AMIDA TIME: " + GlobalVars.GetHttpReturnFromAPIRestLink(
            "https://bibleapje.azurewebsites.net/api/Google/"
            + "Give ONLY the time the Amida should be prayed during the day") + '\n';
        await DisplayAlert("Amida Time", _amida, "OK");
    }
    private async void OnParshatButtonClicked(object sender, EventArgs e)
    {
        GlobalVars.Amida_ = "Parshat";
        string _today = GlobalVars.GetHttpReturnFromAPIRestLink(
            "https://bibleapje.azurewebsites.net/api/ChatGrok/Give de date of TODAY in this EXACTLY format"
            + " Monday 10 Cheshvan 5785 November 11 2024, but adjusted and updated for this date" +
        DateTime.Now.Year.ToString() + " - " + DateTime.Now.Month.ToString() + " - " +  DateTime.Now.Day.ToString());
        string _parshatname = GlobalVars.GetHttpReturnFromAPIRestLink(
            "https://bibleapje.azurewebsites.net/api/ChatGrok/Give ONLY the name of the Weekly Parshat for"
            + " " + _today + "?");
            //DateTime.Now.Year.ToString() + " - " + DateTime.Now.Month.ToString() + " - " +  DateTime.Now.Day.ToString());
            _parshatname = _parshatname.Replace("***", "").Replace("###", "")
                .Replace("**", "").Replace("*", "").Replace("\n\n", " ").Replace("\n", " ");    
        GlobalVars._pPortion = GlobalVars.GetHttpReturnFromAPIRestLink(
            "https://bibleapje.azurewebsites.net/api/ChatGrok/"
            + "Give the specific DAILY portion of the Weekly Parshat " + _parshatname + " for " +
            _today);
        GlobalVars._pPortion = GlobalVars._pPortion.Replace("***", "").Replace("###", "")
            .Replace("**", "").Replace("*", "");
        
        this.MessageLabel.IsVisible = true;
        UpdateLabel("Preparing Parshat Portion");
        await Task.Delay(1000);
        await Navigation.PushAsync(new ChatAnalysis());
        UpdateLabel("...");
        
    }
}