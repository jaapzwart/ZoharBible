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
        _synthesizer = new SpeechSynthesizer(config);
    }

    public async Task ConvertTextToSpeechAsync(string text)
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
}

