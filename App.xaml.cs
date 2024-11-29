namespace ZoharBible;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Maak een instantie van je audio service class.
        IAudioService audioService = new AudioService();
        
        // Roep de Intro constructor aan.
        var introPage = new Intro(audioService);
        
        // Stel de MainPage in bijvoorbeeld op de introPage als dat de bedoeling is.
        MainPage = introPage;
    }
}