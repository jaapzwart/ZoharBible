namespace ZoharBible;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the Click event of the "Get a random Proverb" button.
    /// Navigates the user to the Proverbs page.
    /// </summary>
    /// <param name="sender">The source of the event. Typically, this is the button that was clicked.</param>
    /// <param name="e">An EventArgs that contains the event data.</param>
    private async void OnGetProverbButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Proverbs());
    }
}