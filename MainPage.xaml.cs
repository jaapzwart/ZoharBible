using System.Runtime.InteropServices.JavaScript;

namespace ZoharBible;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        GlobalVars.AiSelected = "GroK";
        OnOptionButtonClicked(KabbalahButton, EventArgs.Empty);
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
        GlobalVars._ProverbOrPsalm = "Proverbs";
        await Navigation.PushAsync(new Proverbs());
    }
    private async void OnGetPsalmsButtonClicked(object sender, EventArgs e)
    {
        GlobalVars.Amida_ = "";
        GlobalVars._ProverbOrPsalm = "Psalms";
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
        GlobalVars._pPortion = this.TopEntryBox.Text;
        this.MessageLabel.IsVisible = true;
        UpdateLabel("Preparing Analysis of the given text");
        await Task.Delay(1000);
        await Navigation.PushAsync(new ChatAnalysis());
        UpdateLabel("..."); 
    }
    private void TopEntryBox_Focused(object sender, FocusEventArgs e)
    {
        TopEntryBox.Text = string.Empty; // Clear text when Entry gets focus
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
    private async void OnChabatButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChabatPage());
    }
}