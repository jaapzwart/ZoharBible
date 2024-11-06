using System.Text;
using Newtonsoft.Json;

namespace ZoharBible;

/// <summary>
/// The Proverbs class represents a content page within the Zohar Bible application.
/// It provides functionality to retrieve and display a random proverb from the Bible.
/// </summary>
public partial class ChatAnalysis : ContentPage
{
    /// <summary>
    /// The ChatAnalysis class is a content page within the Zohar Bible application, designed
    /// to retrieve and display a text-based analysis of a specific proverb.
    /// </summary>
    public ChatAnalysis()
    {
        InitializeComponent();
        string responseText = "";
        if (GlobalVars.AiSelected.Contains("ChatGPT"))
        {
            responseText = GlobalVars.GetHttpReturnFromAPIRestLink("https://bibleapje.azurewebsites.net/api/ChatGPT/"
                                                                   + "Give an analysis on " +
                                                                   GlobalVars.ProverbToAnalyse +
                                                                   " from out the perspective of the " +
                                                                   GlobalVars.TypeOfProverbAnalysis + ".");
            this.ChatAnalysisText.Text = responseText.TrimStart();
        }
        else
        {
            CallGetGrok();
        }
    }
    private async void CallGetGrok()
    {
        string question = "Give an analysis on " + GlobalVars.ProverbToAnalyse +
                          " from out the perspective of the " + GlobalVars.TypeOfProverbAnalysis + ".";
        string resultGrok = await GetGrok(question);
        this.ChatAnalysisText.Text = GlobalVars.gRok;
    }
    static async Task<string> GetGrok(string question)
    {
        string resultGrok = "";
        // Your API key and base URL
        string apiKey = "xai-UHkCfAnGrY6wz9IRv5TAZD0olYZhuBdFY2aNimqBR6otc3oVkFgMzXJBjftAuIaLOjt0CD7Kgf4olmXV";
        string baseURL = "https://api.x.ai/v1/chat/completions";

        // Create the HTTP client
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            
            // Define the request body
            var requestBody = new
            {
                model = "grok-beta",
                messages = new[]
                {
                    new { role = "system", content = "You are Grok, a chatbot inspired by the Holy Spirit." },
                    new { role = "user", content = question }
                }
            };

            // Serialize the request body to JSON
            string json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make the POST request
            HttpResponseMessage response = await client.PostAsync(baseURL, content);

            // Read and output the response
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseBody);
                resultGrok = result.choices[0].message.content;
                GlobalVars.gRok = resultGrok;
                //Console.WriteLine(result.choices[0].message.content);
            }
            else
            {
                resultGrok = response.StatusCode.ToString();
                GlobalVars.gRok = resultGrok;
                //Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        return resultGrok;
    }
}