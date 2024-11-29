using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using ZoharBible;

namespace ZoharBible
{
    public class AzureSpeechToText
    {
        private static string subscriptionKey = Secrets.subscriptionKey;
        private static string serviceRegion = Secrets.azureRegion;

        public static async Task<string> TranscribeAudioAsync(string filepath, List<string> keyword)
        {
            string dSentiment = await GlobalVars.GetHttpReturnFromAPIRestLink(
                Secrets.RESTAPI + @"ChatGPT/"
                                + "Thank the user for the command and tell the user you are busy interpreting the commend given" +
                                " and you ask from some patience..");
            await GlobalVars.ttsService.ConvertTextToSpeechAsync(dSentiment);
            var config = SpeechConfig.FromSubscription(Secrets.wToTSubscription, Secrets.llocation);

            using var audioInput = AudioConfig.FromWavFileInput(filepath);
            using var recognizer = new SpeechRecognizer(config, audioInput);

            var result = await recognizer.RecognizeOnceAsync();

            bool isInKeywords = false;
            foreach (var item in keyword)
            {
                if (result.Text.Contains(item))
                    isInKeywords = true;
            }

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                if (isInKeywords) // Recording found in the keywords?
                {
                    
                    if (result.Text.Contains("Dialogue", StringComparison.OrdinalIgnoreCase) &&
                        (result.Text.Contains("Activate", StringComparison.OrdinalIgnoreCase)
                        || result.Text.Contains("Start", StringComparison.OrdinalIgnoreCase)))
                    {
                        GlobalVars._Dialogue = true;
                        return "AI Dialogue checkbox will be started.";
                    }
                    if (result.Text.Contains("Dialogue", StringComparison.OrdinalIgnoreCase) &&
                             (result.Text.Contains("Deactivate", StringComparison.OrdinalIgnoreCase)
                              || result.Text.Contains("Stop", StringComparison.OrdinalIgnoreCase)))
                    {
                        GlobalVars._Dialogue = false;
                        return "AI Dialogue checkbox will be stopped.";
                    }
                    if (result.Text.Contains("Who", StringComparison.OrdinalIgnoreCase) &&
                        (result.Text.Contains("are", StringComparison.OrdinalIgnoreCase)
                         || result.Text.Contains("you", StringComparison.OrdinalIgnoreCase)))
                    {
                        return "This is the new version of 'Hello World'.";
                    }
                    return "Command not found.";
                }
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                return "Geen spraak herkend.";
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);

                if (cancellation.Reason == CancellationReason.Error)
                {
                    return "Foutdetails:" + cancellation.ErrorDetails;

                }

                return "Geannuleerd:" + cancellation.Reason;
            }

            return "None audio analysis happened";
        }
    }
}