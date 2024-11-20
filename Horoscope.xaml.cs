using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace ZoharBible
{
    public partial class Horoscope : ContentPage
    {
        public Horoscope()
        {
            InitializeComponent();
            this.DayCheckBox.IsChecked = true;
        }
        
        private const string AlertTitle = "Horoscope";

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var zodiacSign = button.Text;
                // await DisplayAlert(AlertTitle, $"{zodiacSign} button clicked", "OK");
                GlobalVars._pPortion = "Horoscope " + zodiacSign;
                GlobalVars.Amida_ = "Horoscope";
                this.MessageLabel.IsVisible = true;
                UpdateLabel("Preparing Horoscope Analysis " + '\n'
                    + GlobalVars._pPortion 
                    + " - " + GlobalVars.HPeriod);
                await Task.Delay(1000);
                await Navigation.PushAsync(new ChatAnalysis());
                UpdateLabel("...");
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
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            string checker = "";
            if (sender is CheckBox selectedCheckBox && e.Value)
            {
                // Disable all other checkboxes
                if (selectedCheckBox != DayCheckBox)
                {
                    DayCheckBox.IsEnabled = false;
                }
                else
                {
                    checker = "Day";
                }

                if (selectedCheckBox != WeekCheckBox)
                {
                    WeekCheckBox.IsEnabled = false;
                }
                else
                {
                    checker = "Week";
                }

                if (selectedCheckBox != MonthCheckBox)
                {
                    MonthCheckBox.IsEnabled = false;
                }
                else
                {
                    checker = "Month";
                }

                if (selectedCheckBox != YearCheckBox)
                {
                    YearCheckBox.IsEnabled = false;
                }
                else
                {
                    checker = "Year";
                }

                GlobalVars.HPeriod = checker;
            }
            else
            {
                // Enable all checkboxes
                DayCheckBox.IsEnabled = true;
                WeekCheckBox.IsEnabled = true;
                MonthCheckBox.IsEnabled = true;
                YearCheckBox.IsEnabled = true;
            }
        }
    }
}