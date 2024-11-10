namespace ZoharBible;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        GlobalVars.AiSelected = "GroK";
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
}