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
        }

        private void OnButtonClicked_Aries(object sender, EventArgs e)
        {
            // Voeg hier jouw logica toe voor Aries
            DisplayAlert("Horoscope", "Aries button clicked", "OK");
        }

        private void OnButtonClicked_Taurus(object sender, EventArgs e)
        {
            // Voeg hier jouw logica toe voor Taurus
            DisplayAlert("Horoscope", "Taurus button clicked", "OK");
        }

        private void OnButtonClicked_Gemini(object sender, EventArgs e)
        {
            // Voeg hier jouw logica toe voor Gemini
            DisplayAlert("Horoscope", "Gemini button clicked", "OK");
        }

        private void OnButtonClicked_Cancer(object sender, EventArgs e)
        {
            // Voeg hier jouw logica toe voor Cancer
            DisplayAlert("Horoscope", "Cancer button clicked", "OK");
        }

        private void OnButtonClicked_Leo(object sender, EventArgs e)
        {
            // Voeg hier jouw logica toe voor Leo
            DisplayAlert("Horoscope", "Leo button clicked", "OK");
        }

        private void OnButtonClicked_Virgo(object sender, EventArgs e)
        {
            // Voeg hier jouw logica toe voor Virgo
            DisplayAlert("Horoscope", "Virgo button clicked", "OK");
        }

        private void OnButtonClicked_Libra(object sender, EventArgs e)
        {
            // Voeg hier jouw logica toe voor Libra
            DisplayAlert("Horoscope", "Libra button clicked", "OK");
        }

        private void OnButtonClicked_Scorpio(object sender, EventArgs e)
        {
            // Voeg hier jouw logica toe voor Scorpio
            DisplayAlert("Horoscope", "Scorpio button clicked", "OK");
        }

        private void OnButtonClicked_Sagittarius(object sender, EventArgs e)
        {
            // Voeg hier jouw logica toe voor Sagittarius
            DisplayAlert("Horoscope", "Sagittarius button clicked", "OK");
        }

        private void OnButtonClicked_Capricorn(object sender, EventArgs e)
        {
            // Voeg hier jouw logica toe voor Capricorn
            DisplayAlert("Horoscope", "Capricorn button clicked", "OK");
        }

        private void OnButtonClicked_Aquarius(object sender, EventArgs e)
        {
            // Voeg hier jouw logica toe voor Aquarius
            DisplayAlert("Horoscope", "Aquarius button clicked", "OK");
        }

        private void OnButtonClicked_Pisces(object sender, EventArgs e)
        {
            // Voeg hier jouw logica toe voor Pisces
            DisplayAlert("Horoscope", "Pisces button clicked", "OK");
        }
        
        private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox selectedCheckBox && e.Value)
            {
                // Disable all other checkboxes
                if (selectedCheckBox != DayCheckBox)
                    DayCheckBox.IsEnabled = false;
                if (selectedCheckBox != WeekCheckBox)
                    WeekCheckBox.IsEnabled = false;
                if (selectedCheckBox != MonthCheckBox)
                    MonthCheckBox.IsEnabled = false;
                if (selectedCheckBox != YearCheckBox)
                    YearCheckBox.IsEnabled = false;
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