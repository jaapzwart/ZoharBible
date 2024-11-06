namespace ZoharBible;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    private async void OnGetProverbButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Proverbs());
    }
}