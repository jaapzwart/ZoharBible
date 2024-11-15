using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoharBible;

public partial class ChabatPage : ContentPage
{
    public ChabatPage()
    {
        InitializeComponent();
    }
    private void OnLoadUrlClicked(object sender, EventArgs e)
    {
        // Get the URL from the entry
        string url = UrlEntry.Text;
        if(UrlEntry.Text.Contains("throwcards"))
            url = "https://throwcards.azurewebsites.net";
        if(UrlEntry.Text.Contains("kabbalah"))
            url = "https://www.chabad.org/kabbalah/article_cdo/aid/1270227/jewish/Daily-Zohar-Hok-LYisrael.htm";

        // Validate the URL
        if (!string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            // Load the URL in the WebView
            Browser.Source = url;
        }
        else
        {
            DisplayAlert("Invalid URL", "Please enter a valid URL.", "OK");
        }
    }
}