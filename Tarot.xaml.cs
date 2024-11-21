using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoharBible;

public partial class Tarot : ContentPage
{
    public Tarot()
    {
        InitializeComponent();
        UpdateCheckBoxes(GlobalVars.AiSelected);
        Onload();
        UpdateLabel("...");
        CheckCardInfo();
        GlobalVars._pPortion = "Tarot";
    }
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
    private async void OnCardTapped(object sender, EventArgs e)
    {
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
            UpdateLabel("Getting Card information");
            if (AIInfoCheckBox.IsChecked) // Get AI card info.
            {
                UpdateLabel("Preparing Analysis Tarot");
                await Task.Delay(1000);
                await Navigation.PushAsync(new ChatAnalysis());
                UpdateLabel("...");
            }
            else // Get online card info
            {
                await Navigation.PushAsync(new ChabatPage(WhatMeaningToSee(cc))); 
            }
        }
    }
    private void CheckCardInfo()
    {
        AIInfoCheckBox.IsChecked = GlobalVars.TAIInfo;
        OnlineInfoCheckBox.IsChecked = GlobalVars.TOnlineInfo;
    }
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
    public string WhatMeaningToSee(string inCard)
    {
        // The majors
        if (inCard.Contains("/1.jpg"))
        {
            GlobalVars.theCardT = "THE FOOL";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj00.htm";
        }

        if (inCard.Contains("/2.jpg"))
        {
            GlobalVars.theCardT = "THE MAGICIAN";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj01.htm";
        }

        if (inCard.Contains("/3.jpg"))
        {
            GlobalVars.theCardT = "THE HIGH PRIESTESS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj02.htm";
        }

        if (inCard.Contains("/4.jpg"))
        {
            GlobalVars.theCardT = "THE EMPRESS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj03.htm";
        }

        if (inCard.Contains("/5.jpg"))
        {
            GlobalVars.theCardT = "THE EMPEROR";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj04.htm";
        }

        if (inCard.Contains("/6.jpg"))
        {
            GlobalVars.theCardT = "THE HIEROPHANT";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj05.htm";
        }

        if (inCard.Contains("/7.jpg"))
        {
            GlobalVars.theCardT = "THE LOVERS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj06.htm";
        }

        if (inCard.Contains("/8.jpg"))
        {
            GlobalVars.theCardT = "THE CHARIOT";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj07.htm";
        }

        if (inCard.Contains("/9.jpg"))
        {
            GlobalVars.theCardT = "STRENGTH";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj08.htm";
        }

        if (inCard.Contains("/10.jpg"))
        {
            GlobalVars.theCardT = "THE HERMIT";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj09.htm";
        }

        if (inCard.Contains("/11.jpg"))
        {
            GlobalVars.theCardT = "WHEEL OF FORTUNE";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj10.htm";
        }

        if (inCard.Contains("/12.jpg"))
        {
            GlobalVars.theCardT = "JUSTICE";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj11.htm";
        }

        if (inCard.Contains("/13.jpg"))
        {
            GlobalVars.theCardT = "THE HANGED MAN";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj12.htm";
        }

        if (inCard.Contains("/14.jpg"))
        {
            GlobalVars.theCardT = "DEATH";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj13.htm";
        }

        if (inCard.Contains("/15.jpg"))
        {
            GlobalVars.theCardT = "TEMPERANCE";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj14.htm";
        }

        if (inCard.Contains("/16.jpg"))
        {
            GlobalVars.theCardT = "THE DEVIL";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj15.htm";
        }

        if (inCard.Contains("/17.jpg"))
        {
            GlobalVars.theCardT = "THE TOWER";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj16.htm";
        }

        if (inCard.Contains("/18.jpg"))
        {
            GlobalVars.theCardT = "THE STAR";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj17.htm";
        }

        if (inCard.Contains("/19.jpg"))
        {
            GlobalVars.theCardT = "THE MOON";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj18.htm";
        }

        if (inCard.Contains("/20.jpg"))
        {
            GlobalVars.theCardT = "THE SUN";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj19.htm";
        }

        if (inCard.Contains("/21.jpg"))
        {
            GlobalVars.theCardT = "JUDGEMENT";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj20.htm";
        }

        if (inCard.Contains("/22.jpg"))
        {
            GlobalVars.theCardT = "THE WORLD";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/maj21.htm";
        }

        // The wants
        if (inCard.Contains("/23.jpg"))
        {
            GlobalVars.theCardT = "ACE OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/wa.htm";
        }

        if (inCard.Contains("/24.jpg"))
        {
            GlobalVars.theCardT = "TWO OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/w2.htm";
        }

        if (inCard.Contains("/25.jpg"))
        {
            GlobalVars.theCardT = "THREE OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/w3.htm";
        }

        if (inCard.Contains("/26.jpg"))
        {
            GlobalVars.theCardT = "FOUR OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/w4.htm";
        }

        if (inCard.Contains("/27.jpg"))
        {
            GlobalVars.theCardT = "FIVE OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/w5.htm";
        }

        if (inCard.Contains("/28.jpg"))
        {
            GlobalVars.theCardT = "SIX OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/w6.htm";
        }

        if (inCard.Contains("/29.jpg"))
        {
            GlobalVars.theCardT = "SEVEN OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/w7.htm";
        }

        if (inCard.Contains("/30.jpg"))
        {
            GlobalVars.theCardT = "EIGHT OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/w8.htm";
        }

        if (inCard.Contains("/31.jpg"))
        {
            GlobalVars.theCardT = "NINE OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/w9.htm";
        }

        if (inCard.Contains("/32.jpg"))
        {
            GlobalVars.theCardT = "TEN OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/w10.htm";
        }

        if (inCard.Contains("/33.jpg"))
        {
            GlobalVars.theCardT = "PAGE OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/wpg.htm";
        }

        if (inCard.Contains("/34.jpg"))
        {
            GlobalVars.theCardT = "KNIGHT OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/wkn.htm";
        }

        if (inCard.Contains("/35.jpg"))
        {
            GlobalVars.theCardT = "QUEEN OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/wqn.htm";
        }

        if (inCard.Contains("/36.jpg"))
        {
            GlobalVars.theCardT = "KING OF WANDS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/wkg.htm";
        }

        // The cups
        if (inCard.Contains("/37.jpg"))
        {
            GlobalVars.theCardT = "ACE OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/ca.htm";
        }

        if (inCard.Contains("/38.jpg"))
        {
            GlobalVars.theCardT = "TWO OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/c2.htm";
        }

        if (inCard.Contains("/39.jpg"))
        {
            GlobalVars.theCardT = "THREE OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/c3.htm";
        }

        if (inCard.Contains("/40.jpg"))
        {
            GlobalVars.theCardT = "FOUR OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/c4.htm";
        }

        if (inCard.Contains("/41.jpg"))
        {
            GlobalVars.theCardT = "FIVE OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/c5.htm";
        }

        if (inCard.Contains("/42.jpg"))
        {
            GlobalVars.theCardT = "SIX OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/c6.htm";
        }

        if (inCard.Contains("/43.jpg"))
        {
            GlobalVars.theCardT = "SEVEN OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/c7.htm";
        }

        if (inCard.Contains("/44.jpg"))
        {
            GlobalVars.theCardT = "EIGHT OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/c8.htm";
        }

        if (inCard.Contains("/45.jpg"))
        {
            GlobalVars.theCardT = "NINE OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/c9.htm";
        }

        if (inCard.Contains("/46.jpg"))
        {
            GlobalVars.theCardT = "TEN OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/c10.htm";
        }

        if (inCard.Contains("/47.jpg"))
        {
            GlobalVars.theCardT = "PAGE OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/cpg.htm";
        }

        if (inCard.Contains("/48.jpg"))
        {
            GlobalVars.theCardT = "KNIGHT OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/ckn.htm";
        }

        if (inCard.Contains("/49.jpg"))
        {
            GlobalVars.theCardT = "QUEEN OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/cqn.htm";
        }

        if (inCard.Contains("/50.jpg"))
        {
            GlobalVars.theCardT = "KING OF CUPS";
            GlobalVars.cardChosen = GlobalVars.theCardT;
        }
        // The swords
        if (inCard.Contains("/51.jpg"))
        {
            GlobalVars.theCardT = "ACE OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/sa.htm";
        }
        if (inCard.Contains("/52.jpg"))
        {
            GlobalVars.theCardT = "TWO OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/s2.htm";
        }
        if (inCard.Contains("/53.jpg"))
        {
            GlobalVars.theCardT = "THREE OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/s3.htm";
        }
        if (inCard.Contains("/54.jpg"))
        {
            GlobalVars.theCardT = "FOUR OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/s4.htm";
        }
        if (inCard.Contains("/55.jpg"))
        {
            GlobalVars.theCardT = "FIVE OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/s5.htm";
        }
        if (inCard.Contains("/56.jpg"))
        {
            GlobalVars.theCardT = "SIX OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/s6.htm";
        }
        if (inCard.Contains("/57.jpg"))
        {
            GlobalVars.theCardT = "SEVEN OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/s7.htm";
        }
        if (inCard.Contains("/58.jpg"))
        {
            GlobalVars.theCardT = "EIGHT OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/s8.htm";
        }
        if (inCard.Contains("/59.jpg"))
        {
            GlobalVars.theCardT = "NINE OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/s9.htm";
        }
        if (inCard.Contains("/60.jpg"))
        {
            GlobalVars.theCardT = "TEN OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/s10.htm";
        }
        if (inCard.Contains("/61.jpg"))
        {
            GlobalVars.theCardT = "PAGE OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/spg.htm";
        }
        if (inCard.Contains("/62.jpg"))
        {
            GlobalVars.theCardT = "KNIGHT OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/skn.htm";
        }
        if (inCard.Contains("/63.jpg"))
        {
            GlobalVars.theCardT = "QUEEN OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/sqn.htm";
        }
        if (inCard.Contains("/64.jpg"))
        {
            GlobalVars.theCardT = "KING OF SWORDS"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/skg.htm";
        }
        // The pentacles
        if (inCard.Contains("/65.jpg"))
        {
            GlobalVars.theCardT = "ACE OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/pa.htm";
        }
        if (inCard.Contains("/66.jpg"))
        {
            GlobalVars.theCardT = "TWO OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/p2.htm";
        }
        if (inCard.Contains("/67.jpg"))
        {
            GlobalVars.theCardT = "THREE OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/p3.htm";
        }
        if (inCard.Contains("/68.jpg"))
        {
            GlobalVars.theCardT = "FOUR OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/p4.htm";
        }
        if (inCard.Contains("/69.jpg"))
        {
            GlobalVars.theCardT = "FIVE OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/p5.htm";
        }
        if (inCard.Contains("/70.jpg"))
        {
            GlobalVars.theCardT = "SIX OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/p6.htm";
        }
        if (inCard.Contains("/71.jpg"))
        {
            GlobalVars.theCardT = "SEVEN OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/p7.htm";
        }
        if (inCard.Contains("/72.jpg"))
        {
            GlobalVars.theCardT = "EIGHT OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/p8.htm";
        }
        if (inCard.Contains("/73.jpg"))
        {
            GlobalVars.theCardT = "NINE OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/p9.htm";
        }
        if (inCard.Contains("/74.jpg"))
        {
            GlobalVars.theCardT = "TEN OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/p10.htm";
        }
        if (inCard.Contains("/75.jpg"))
        {
            GlobalVars.theCardT = "PAGE OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/ppg.htm";
        }
        if (inCard.Contains("/76.jpg"))
        {
            GlobalVars.theCardT = "KNIGHT OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/pkn.htm";
        }
        if (inCard.Contains("/77.jpg"))
        {
            GlobalVars.theCardT = "QUEEN OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/pqn.htm";
        }
        if (inCard.Contains("/78.jpg"))
        {
            GlobalVars.theCardT = "KING OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
            return "http://www.learntarot.com/pkg.htm";
        }
        GlobalVars.theCardT = "ACE OF PENTACLES"; GlobalVars.cardChosen = GlobalVars.theCardT;
        return "http://www.learntarot.com/pa.htm";
    }

    private void ShuffleDeck()
    {
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

    private void Onload()
    {
        card1Image.Source = GlobalVars.TCard1;
        card2Image.Source = GlobalVars.TCard2;
        card3Image.Source = GlobalVars.TCard3;
    }
    private void OnShuffleClicked(object sender, EventArgs e)
    {
        ShuffleDeck();
    }
}