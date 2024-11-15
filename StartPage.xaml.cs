namespace ZoharBible;

public partial class StartPage : ContentPage
{
    public StartPage()
    {
        InitializeComponent();
        GlobalVars.AiSelected = "ChatGPT";
        SpeechSpeedSlider.Value = 90;
            
        // Stel de initiële waarde van de Label in
        SpeechSpeedValueLabel.Text = GlobalVars.SpeechSpeed.ToString() + "%";
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LanguagePicker.SelectedItem = "en";
        this.MessageLabel.Text = "...";
    }
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
    private void OnLanguagePickerChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null)
        {
            GlobalVars.lLanguage_ = picker.SelectedItem as string;
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
}