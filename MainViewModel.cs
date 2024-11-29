using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZoharBible;

public class MainViewModel : INotifyPropertyChanged
{
    private string _result;

    public string Result
    {
        get { return _result; }
        set
        {
            _result = value;
            OnPropertyChanged();
        }
    }

    public ICommand TranscribeCommand { get; }

    public MainViewModel()
    {
        TranscribeCommand = new Command(async () => await TranscribeAudio());
    }

    private async Task TranscribeAudio()
    {
        Result = await AzureSpeechToText.TranscribeAudioAsync("audiofile.wav", "zoekwoord");
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}