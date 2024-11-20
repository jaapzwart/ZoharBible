namespace ZoharBible;

public partial class StartPage : ContentPage
{
    public StartPage()
    {
        InitializeComponent();
        GlobalVars.AiSelected = "ChatGPT";
        UpdateCheckBoxes(GlobalVars.AiSelected);
        SpeechSpeedSlider.Value = 90;
        
        // Stel de initiële waarde van de Label in
        SpeechSpeedValueLabel.Text = GlobalVars.SpeechSpeed.ToString() + "%";
    }

    #region Lifecycle Methods

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LanguagePicker.SelectedItem = "en";
        this.MessageLabel.Text = "...";
    }

    #endregion

    #region Event Handlers

    private void OnSpeechSpeedSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        GlobalVars.SpeechSpeed = $"{e.NewValue}";
        SpeechSpeedValueLabel.Text = e.NewValue.ToString("F0") + "%";
    }

    private async void OnNavigateToMainPageClicked(object sender, EventArgs e)
    {
        UpdateLabel("Opening Starting Page");
        await Task.Delay(1000);
        await Navigation.PushAsync(new MainPage());
    }
    private async void OnNavigateToHoroPageClicked(object sender, EventArgs e)
    {
        UpdateLabel("Opening Horoscope Page");
        await Task.Delay(1000);
        await Navigation.PushAsync(new Horoscope());
    }
    private async Task DisplayAlertAsync(string title, string message)
    {
        await DisplayAlert(title, message, "OK");
    }
    private void OnLanguagePickerChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null)
        {
            GlobalVars.lLanguage_ = picker.SelectedItem as string;
        }
    }
    private async void OnChatbotCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            await DisplayAlert("Fout", $"Er is een fout opgetreden: {ex.Message}", "OK");
        }
    }
    #endregion

    #region Helper Methods
    private void UpdateCheckBoxes(string aiSelected)
    {
        ChatGPTCheckBox.IsChecked = aiSelected.Contains("ChatGPT");
        GroKCheckBox.IsChecked = aiSelected.Contains("GroK");
        GeminiCheckBox.IsChecked = aiSelected.Contains("Gemini");
        AllAICheckBox.IsChecked = aiSelected.Contains("AllAI");

        if (!aiSelected.Contains("AllAI"))
        {
            AllAICheckBox.IsChecked = false;
        }
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

    #endregion
}