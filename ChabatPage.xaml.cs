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