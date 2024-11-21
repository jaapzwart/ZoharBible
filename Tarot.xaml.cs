using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoharBible;

/// <summary>
/// The Tarot class represents a page in the ZoharBible application dedicated to Tarot card readings.
/// </summary>
public partial class Tarot : ContentPage
{
    /// <summary>
    /// Represents the Tarot page of the application.
    /// </summary>
    public Tarot()
    {
        InitializeComponent();
        UpdateCheckBoxes(GlobalVars.AiSelected);
        Onload();
        UpdateLabel("...");
        CheckCardInfo();
        GlobalVars._pPortion = "Tarot";
    }

    /// <summary>
    /// Updates the state of multiple checkboxes based on the provided selection string.
    /// </summary>
    /// <param name="aiSelected">A string containing the names of the selected AI components.</param>
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

    /// <summary>
    /// Updates the text and visibility of the label on the main thread.
    /// </summary>
    /// <param name="text">The text to display in the label.</param>
    private async void UpdateLabel(string text)
    {
        try
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.MessageLabel.IsVisible = true;
                this.MessageLabel.Text = text;
            });
            await Task.Yield();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error message","Error", $"An error occurred while updating the label: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles the event when a tarot card is tapped.
    /// </summary>
    /// <param name="sender">The source of the event, expected to be a Frame with a TapGestureRecognizer.</param>
    /// <param name="e">The event arguments associated with the tap event.</param>
    private async void OnCardTapped(object sender, EventArgs e)
    {
        if (GlobalVars.firstThrow)
        {
            DisplayAlert("First Shuffle", "Shuffle the Cards first", "OK");
            return;
        }

        var frame = sender as Frame;
        if (frame != null && frame.GestureRecognizers[0] is TapGestureRecognizer tapGesture)
        {
            var position = tapGesture.CommandParameter.ToString();
            GlobalVars.thePositionT = position;
            string cc = "";
            if (position == "1")
                cc = card1Image.Source.ToString();
            else if (position == "2")
                cc = card2Image.Source.ToString();
            else if (position == "3")
                cc = card3Image.Source.ToString();
            
            if (GlobalVars.TAIInfo) // Get AI card info.
            {
                UpdateLabel("Getting AI card information");
                await Task.Delay(1000);
                await Navigation.PushAsync(new ChatAnalysis());
                UpdateLabel("...");
            }
            else if (GlobalVars.TOnlineInfo) // Get online card info
            {
                UpdateLabel("Getting online card information");
                await Task.Delay(1000);
                await Navigation.PushAsync(new ChabatPage(WhatMeaningToSee(cc))); 
                UpdateLabel("...");
            }
        }
    }

    /// <summary>
    /// Updates the state of the information checkboxes based on global variables.
    /// This method checks the values of <c>GlobalVars.TAIInfo</c> and <c>GlobalVars.TOnlineInfo</c>,
    /// and sets the corresponding checkboxes (<c>AIInfoCheckBox</c> and <c>OnlineInfoCheckBox</c>)
    /// to reflect those values.
    /// </summary>
    private void CheckCardInfo()
    {
        AIInfoCheckBox.IsChecked = GlobalVars.TAIInfo;
        OnlineInfoCheckBox.IsChecked = GlobalVars.TOnlineInfo;
    }

    /// <summary>
    /// Handles the CheckedChanged event for the info checkboxes.
    /// Updates the state of the checkbox that was not directly altered by the user,
    /// ensuring only one of the checkboxes can be selected at a time.
    /// </summary>
    /// <param name="sender">The checkbox control that triggered the event.</param>
    /// <param name="e">Event data containing the old and new checked states.</param>
    private void OnInfoCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender == OnlineInfoCheckBox && OnlineInfoCheckBox.IsChecked)
        {
            AIInfoCheckBox.IsChecked = false;
            GlobalVars.TAIInfo = false;
            GlobalVars.TOnlineInfo = true;
        }
        else if (sender == AIInfoCheckBox && AIInfoCheckBox.IsChecked)
        {
            OnlineInfoCheckBox.IsChecked = false;
            GlobalVars.TOnlineInfo = false;
            GlobalVars.TAIInfo = true;
        }
        CheckCardInfo();
    }

    /// <summary>
    /// Handles the CheckedChanged event for chatbot-related checkboxes.
    /// Updates the state of checkboxes and sets the selected AI globally.
    /// </summary>
    /// <param name="sender">The source of the event, which is a CheckBox.</param>
    /// <param name="e">The event data containing the new state of the CheckBox.</param>
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


    /// <summary>
    /// Determines the meaning of a Tarot card based on the provided card image identifier.
    /// </summary>
    /// <param name="inCard">The identifier of the card image, typically a file path or URL segment.</param>
    /// <returns>A string containing the name and URL of the card's meaning, if found; otherwise, an empty string.</returns>
    public string WhatMeaningToSee(string inCard)
    {
        Dictionary<string, (string cardName, string url)> cardMappings = new Dictionary<string, (string, string)>
        {
            { "/1.jpg", ("THE FOOL", "http://www.learntarot.com/maj00.htm") },
            { "/2.jpg", ("THE MAGICIAN", "http://www.learntarot.com/maj01.htm") },
            { "/3.jpg", ("THE HIGH PRIESTESS", "http://www.learntarot.com/maj02.htm") },
            { "/4.jpg", ("THE EMPRESS", "http://www.learntarot.com/maj03.htm") },
            { "/5.jpg", ("THE EMPEROR", "http://www.learntarot.com/maj04.htm") },
            { "/6.jpg", ("THE HIEROPHANT", "http://www.learntarot.com/maj05.htm") },
            { "/7.jpg", ("THE LOVERS", "http://www.learntarot.com/maj06.htm") },
            { "/8.jpg", ("THE CHARIOT", "http://www.learntarot.com/maj07.htm") },
            { "/9.jpg", ("STRENGTH", "http://www.learntarot.com/maj08.htm") },
            { "/10.jpg", ("THE HERMIT", "http://www.learntarot.com/maj09.htm") },
            { "/11.jpg", ("WHEEL OF FORTUNE", "http://www.learntarot.com/maj10.htm") },
            { "/12.jpg", ("JUSTICE", "http://www.learntarot.com/maj11.htm") },
            { "/13.jpg", ("THE HANGED MAN", "http://www.learntarot.com/maj12.htm") },
            { "/14.jpg", ("DEATH", "http://www.learntarot.com/maj13.htm") },
            { "/15.jpg", ("TEMPERANCE", "http://www.learntarot.com/maj14.htm") },
            { "/16.jpg", ("THE DEVIL", "http://www.learntarot.com/maj15.htm") },
            { "/17.jpg", ("THE TOWER", "http://www.learntarot.com/maj16.htm") },
            { "/18.jpg", ("THE STAR", "http://www.learntarot.com/maj17.htm") },
            { "/19.jpg", ("THE MOON", "http://www.learntarot.com/maj18.htm") },
            { "/20.jpg", ("THE SUN", "http://www.learntarot.com/maj19.htm") },
            { "/21.jpg", ("JUDGEMENT", "http://www.learntarot.com/maj20.htm") },
            { "/22.jpg", ("THE WORLD", "http://www.learntarot.com/maj21.htm") },
            { "/23.jpg", ("ACE OF WANDS", "http://www.learntarot.com/wa.htm") },
            { "/24.jpg", ("TWO OF WANDS", "http://www.learntarot.com/w2.htm") },
            { "/25.jpg", ("THREE OF WANDS", "http://www.learntarot.com/w3.htm") },
            { "/26.jpg", ("FOUR OF WANDS", "http://www.learntarot.com/w4.htm") },
            { "/27.jpg", ("FIVE OF WANDS", "http://www.learntarot.com/w5.htm") },
            { "/28.jpg", ("SIX OF WANDS", "http://www.learntarot.com/w6.htm") },
            { "/29.jpg", ("SEVEN OF WANDS", "http://www.learntarot.com/w7.htm") },
            { "/30.jpg", ("EIGHT OF WANDS", "http://www.learntarot.com/w8.htm") }
        };

        foreach (var cardMapping in cardMappings)
        {
            if (inCard.Contains(cardMapping.Key))
            {
                GlobalVars.theCardT = cardMapping.Value.cardName;
                GlobalVars.cardChosen = GlobalVars.theCardT;
                return cardMapping.Value.url;
            }
        }

        return string.Empty; // of een andere standaardwaarde
    }

    /// <summary>
    /// Shuffles the deck of tarot cards and assigns three unique cards to global variables.
    /// </summary>
    /// <remarks>
    /// The method generates a list of card URLs, shuffles them randomly, selects three unique cards,
    /// and assigns them to the global variables <c>GlobalVars.TCard1</c>, <c>GlobalVars.TCard2</c>,
    /// and <c>GlobalVars.TCard3</c>. It then triggers the <c>Onload</c> method.
    /// </remarks>
    private void ShuffleDeck()
    {
        GlobalVars.firstThrow = false;
        // Generate the deck of card URLs
        var deck = Enumerable.Range(0, 79) // Cards numbered from 0 to 78
            .Select(i => $"{Secrets.cardSite}{i}.jpg")
            .ToList();

        // Shuffle the deck
        Random rng = new Random();
        var shuffledDeck = deck.OrderBy(_ => rng.Next()).ToList();

        // Select three unique cards from the shuffled deck
        var selectedCards = shuffledDeck.Take(3).ToList();

        // Assign the cards to three separate strings
        GlobalVars.TCard1 = selectedCards[0];
        GlobalVars.TCard2 = selectedCards[1];
        GlobalVars.TCard3 = selectedCards[2];
        Onload();
    }

    /// <summary>
    /// Updates the images of the three Tarot cards displayed in the UI
    /// using the URLs stored in the GlobalVars class.
    /// </summary>
    private void Onload()
    {
        card1Image.Source = GlobalVars.TCard1;
        card2Image.Source = GlobalVars.TCard2;
        card3Image.Source = GlobalVars.TCard3;
    }

    /// <summary>
    /// Handles the click event of the Shuffle button. Shuffles the tarot deck and updates the global variables with selected cards.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void OnShuffleClicked(object sender, EventArgs e)
    {
        ShuffleDeck();
    }
}