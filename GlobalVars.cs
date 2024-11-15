using System.Net;
using System.Text;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;

namespace ZoharBible;

public static class GlobalVars
{
    
    public static string ChatAnalysis = "";
    public static string TypeOfProverbAnalysis = "";
    public static string ProverbToAnalyse = "";
    public static string longText = longString();
    public static string gRok = "";
    public static string AiSelected = "";
    public static string Amida_ = "";
    public static string _pPortion = "";
    public static string lLanguage_ = "English";
    public static string SpeechSpeed = "90";
    public static string _ProverbOrPsalm = "Proverbs";
    
    private const string azureApiKey = "f11e7b39c8f043a99760b0a671d87998";
    private const string azureRegion = "westeurope";
    public static TextToSpeechService ttsService = new TextToSpeechService(azureApiKey, azureRegion);

    private static string longString()
    {
        char character = '#'; // The character you want to repeat
        int repeatCount = 3000; // The number of times to repeat the character

        string repeatedString = new string(character, repeatCount);
        return repeatedString;
    }
    public static string GetHttpReturnFromAPIRestLink(string theLinkAPI)
    {
        // This method has some troubles getting the string from the REST API in good format.
        try
        {
            var responseSimple = new WebClient().DownloadString(theLinkAPI);

            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(theLinkAPI);

            WebResponse response = request.GetResponse();
            string responseText = "";
            var encoding = ASCIIEncoding.ASCII;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                responseText = reader.ReadToEnd();
            }

            GlobalVars.ChatAnalysis = "Analysis";
            return responseText;

        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
public static class Translator
{
    private static readonly string subscriptionKey = "61fb96fbc3c64ead90661b6faf2f09d5";
    private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";
    private static readonly string location = "westeurope";

    public static async Task<string> TranslateTextToGiven(string inputText)
    {
        string route = "/translate?api-version=3.0&from=en&to=" + GlobalVars.lLanguage_;

        object[] body = new object[] { new { Text = inputText } };
        var requestBody = JsonConvert.SerializeObject(body);

        using (var client = new HttpClient())
        using (var request = new HttpRequestMessage())
        {
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(endpoint + route);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            request.Headers.Add("Ocp-Apim-Subscription-Region", location);

            HttpResponseMessage response = await client.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();
            var translationResult = JsonConvert.DeserializeObject<TranslationResult[]>(result);
            return translationResult[0].Translations[0].Text;
        }
    }

    private class TranslationResult
    {
        public Translation[] Translations { get; set; }
    }

    private class Translation
    {
        public string Text { get; set; }
    }
}

public class TextToSpeechService
{
    private readonly SpeechSynthesizer _synthesizer;

    public TextToSpeechService(string azureApiKey, string azureRegion)
    {
        var config = SpeechConfig.FromSubscription(azureApiKey, azureRegion);
        config.SetProperty("speech-synthesis-speed", "2.0");
        _synthesizer = new SpeechSynthesizer(config);
    }

    public async Task ConvertTextToSpeechAsyncOld(string text)
    {
        var result = await _synthesizer.SpeakTextAsync(text);
        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
        {
            // Synthesis successful
        }
        else if (result.Reason == ResultReason.Canceled)
        {
            var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
            Console.WriteLine($"Speech synthesis canceled: {cancellation.Reason}");
            Console.WriteLine($"Error details: {cancellation.ErrorDetails}");
        }
    }
    public async Task StopSpeakingAsync()
    {
        await _synthesizer.StopSpeakingAsync();
    }
    public async Task ConvertTextToSpeechAsync(string text)
    {
        int transformedSpeed = 100 - Convert.ToInt32(GlobalVars.SpeechSpeed.Substring(0, 2));
        
        String language = GlobalVars.lLanguage_;
        string voiceName = language switch
        {
            "af" => "af-ZA-AdriNeural",          // Afrikaans
            "ar" => "ar-SA-ZariyahNeural",       // Arabic
            "bn" => "bn-IN-BashkarNeural",       // Bengali
            "bg" => "bg-BG-IvanNeural",          // Bulgarian
            "zh-Hans" => "zh-CN-XiaoxiaoNeural", // Chinese Simplified
            "zh-Hant" => "zh-TW-HsiaoChenNeural",// Chinese Traditional
            "hr" => "hr-HR-SreckoNeural",        // Croatian
            "cs" => "cs-CZ-VlastaNeural",        // Czech
            "da" => "da-DK-ChristelNeural",      // Danish
            "nl" => "nl-NL-ColetteNeural",       // Dutch
            "en" => "en-US-JennyNeural",         // English
            "fi" => "fi-FI-HarriNeural",         // Finnish
            "fr" => "fr-FR-DeniseNeural",        // French
            "de" => "de-DE-KatjaNeural",         // German
            "el" => "el-GR-NestorasNeural",      // Greek
            "he" => "he-IL-AvriNeural",          // Hebrew
            "hi" => "hi-IN-MadhurNeural",        // Hindi
            "hu" => "hu-HU-NoemiNeural",         // Hungarian
            "id" => "id-ID-ArdiNeural",          // Indonesian
            "it" => "it-IT-ElsaNeural",          // Italian
            "ja" => "ja-JP-KeitaNeural",         // Japanese
            "ko" => "ko-KR-SunHiNeural",         // Korean
            "ms" => "ms-MY-OsmanNeural",         // Malay
            "fa" => "fa-IR-DilaraNeural",        // Persian
            "pl" => "pl-PL-ZofiaNeural",         // Polish
            "pt" => "pt-BR-FranciscaNeural",     // Portuguese
            "pa" => "pa-IN-GurdeepNeural",       // Punjabi
            "ro" => "ro-RO-EmilNeural",          // Romanian
            "ru" => "ru-RU-DmitryNeural",        // Russian
            "sr" => "sr-RS-NicholasNeural",      // Serbian
            "sk" => "sk-SK-LukasNeural",         // Slovak
            "es" => "es-ES-ElviraNeural",        // Spanish
            "sv" => "sv-SE-MattiasNeural",       // Swedish
            "ta" => "ta-IN-PallaviNeural",       // Tamil
            "th" => "th-TH-AcharaNeural",        // Thai
            "tr" => "tr-TR-SedaNeural",          // Turkish
            "uk" => "uk-UA-PolinaNeural",        // Ukrainian
            "ur" => "ur-PK-AsadNeural",          // Urdu
            "vi" => "vi-VN-LienNeural",          // Vietnamese
            _ => "en-US-JennyNeural"             // Standaard stem
        };
        string ssml = $@"
            <speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>
                <voice name='{voiceName}'>
                    <prosody rate='-{transformedSpeed}%'>{text}</prosody>
                </voice>
            </speak>";
        var result = await _synthesizer.SpeakSsmlAsync(ssml);

        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
        {
            // Synthesis successful
        }
        else if (result.Reason == ResultReason.Canceled)
        {
            var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
            Console.WriteLine($"Speech synthesis canceled: {cancellation.Reason}");
            Console.WriteLine($"Error details: {cancellation.ErrorDetails}");
        }
    }
}

