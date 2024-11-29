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

        public static async Task<string> TranscribeAudioAsync(string filepath, string keyword)
        {
            var config = SpeechConfig.FromSubscription(Secrets.wToTSubscription, Secrets.llocation);

            using var audioInput = AudioConfig.FromWavFileInput(filepath);
            using var recognizer = new SpeechRecognizer(config, audioInput);

            var result = await recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                if (result.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    return "Gevonden:" + result.Text;
                }
                else
                {
                    return "Het zoekwoord is niet gevonden.";
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