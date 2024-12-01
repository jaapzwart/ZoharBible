using System.Globalization;
using Microsoft.VisualBasic;

namespace ZoharBible;

public partial class Intro : ContentPage
{
    
    public Intro()
    {
        InitializeComponent();
        GlobalVars._StandardTheme = true;
        GlobalVars._IntroPage = true;
        this.DialogueCheckBox.IsChecked = GlobalVars._Dialogue;
    }
   
    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.MessageLabel.Text = "...";
        GlobalVars._IntroPage = true;
        this.DialogueCheckBox.IsChecked = GlobalVars._Dialogue;
    }
    private void OnCreativitySliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        double newValue = e.NewValue;
        GlobalVars.ChatGPTTemp = newValue;
        // Update de tekst van het bijbehorende label om de nieuwe waarde van de slider weer te geven
        CreativitySliderValueLabel.Text = $"Creativity: {newValue:F1}";
    }
    private async void OnNavigateToStarterPageClicked(object sender, EventArgs e)
    {
        await GlobalVars.writeFileToBlob(GlobalVars.ChatGPTTemp.ToString("0.0", CultureInfo.InvariantCulture),
            "temperatureChatGPT");
        if(this.StandardThemeCheckBox.IsChecked == false)
            Themes.SetMoodColors(Convert.ToInt32(GlobalVars.moodOMeter));
        await GlobalVars.SetClickedColor(sender);
        UpdateLabel("Opening Starting Page");
        await Task.Delay(1000);
        await Navigation.PushAsync(new StartPage());
    }
    private async void OnNavigateToTestAudioPageClicked(object sender, EventArgs e)
    {
        if(this.StandardThemeCheckBox.IsChecked == false)
            Themes.SetMoodColors(Convert.ToInt32(GlobalVars.moodOMeter));
        await GlobalVars.SetClickedColor(sender);
        UpdateLabel("Opening Test Audio Page");
        await Task.Delay(1000);
        await Navigation.PushAsync(new TestAudioRecording());
    }
    private async void OnNavigateToHeyComputerPageClicked(object sender, EventArgs e)
    {
        if(this.StandardThemeCheckBox.IsChecked == false)
            Themes.SetMoodColors(Convert.ToInt32(GlobalVars.moodOMeter));
        GlobalVars.HeyComputerStartTalk = true;
        await GlobalVars.SetClickedColor(sender);
        UpdateLabel("Opening 'Hey Computer' Page");
        await Task.Delay(1000);
        await Navigation.PushAsync(new HeyComputer());
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

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        int roundedValue = (int)Math.Round(e.NewValue / 10.0) * 10;
        GlobalVars.moodOMeter = $"{roundedValue}";
        SliderValueLabel.Text = $"Slider Value: {roundedValue}";
        
    }
    private void OnStandardThemeCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            GlobalVars._StandardTheme = true;
            Themes.SetStandardTheme();
        }
        else
        {
            GlobalVars._StandardTheme = false;
            Themes.SetMoodColors(Convert.ToInt32(GlobalVars.moodOMeter));
        }
    }

    private void DialogueCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        
        GlobalVars._Dialogue = e.Value;
    }
}