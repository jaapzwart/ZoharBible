using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using HealthKit;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ZoharBible;

public static class DoVideoClipLipSync
{
    public static async Task<string> CreateClipAvatar(string whatToSay)
    {
        whatToSay = DialogueCleaner(whatToSay);
        string fileName = "";
        if(GlobalVars.videoTalked.Contains("Bill"))
            fileName = "TalkedBill.mp4";
        if(GlobalVars.videoTalked.Contains("Elon"))
            fileName = "TalkedElon.mp4";
        if(GlobalVars.videoTalked.Contains("Google"))
            fileName = "TalkedGoogle.mp4";
        if(GlobalVars.videoTalked.Contains("Interactive"))
            fileName = "TalkedInteractive.mp4";
        
        string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

        if (!fileName.Contains("TalkedInteractive"))
        {
            if (File.Exists(filePath))
            {
                GlobalVars.videoFileExists = true;
                GlobalVars.videoFilePath = filePath;
                return "Found";
            }
        }

        var options = new RestClientOptions("https://api.d-id.com/talks");
        var client = new RestClient(options);
        var request = new RestRequest("");
        request.AddHeader("accept", "application/json");
        request.AddHeader("authorization", Secrets.ID_Key);
        string jsonBody = $"{{\"source_url\":\"https://d-id-public-bucket.s3.us-west-2.amazonaws.com/alice.jpg\",\"script\":{{\"type\":\"text\",\"subtitles\":\"false\",\"provider\":{{\"type\":\"microsoft\",\"voice_id\":\"Sara\"}},\"input\":\"{whatToSay}\"}},\"config\":{{\"fluent\":\"false\",\"pad_audio\":\"0.0\"}},\"user_data\":\"What to talk\"}}";
        request.AddJsonBody(jsonBody, false);
        var response = await client.PostAsync(request);
        JObject json = JObject.Parse(response.Content);
        string idValue = json["id"]?.ToString();
        
        return await Task.FromResult(idValue);

    }
    private static string DialogueCleaner(string _dialogue)
    {
        _dialogue = _dialogue.Replace(":", " ").Replace("(", " ").Replace(")", " ")
            .Replace("-", " ").Replace("*", "").Replace(" -", " ").Replace(" - ", " ")
            .Replace("***", "")
            .Replace("###", "")
            .Replace("**", "").Replace("*", "");

        _dialogue = Regex.Replace(_dialogue, @"[^\S\r\n,.]+", " ").Trim();
        
        return _dialogue;
    }
    public static async Task<string> CreateAnimationAvatar(string whatToSay)
    {
        string fileName = "";
        if (GlobalVars.videoTalked.Contains("Bill")
            || GlobalVars.videoTalked.Contains("Elon")
            || GlobalVars.videoTalked.Contains("Google"))
            fileName = "TalkedAnimation.mp4";
        
        string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

        if (File.Exists(filePath))
        {
            GlobalVars.videoFileExists = true;
            GlobalVars.videoFilePath = filePath;
            return "Found";
        }
        var options = new RestClientOptions("https://api.d-id.com/animations");
        var client = new RestClient(options);
        var request = new RestRequest("");
        request.AddHeader("accept", "application/json");
        request.AddHeader("authorization", Secrets.ID_Key);
        string jsonBody = "{\"source_url\":\"https://d-id-public-bucket.s3.us-west-2.amazonaws.com/alice.jpg\"}";
        request.AddJsonBody(jsonBody, false);
        var response = await client.PostAsync(request);
        JObject json = JObject.Parse(response.Content);
        string idValue = json["id"]?.ToString();
        
        return await Task.FromResult(idValue);

    }
    

    public static async Task<string> GetVideoClip(string theClip)
    {
        try
        {
            RestClientOptions options;
            if(GlobalVars.anim)
                options = new RestClientOptions("https://api.d-id.com/animations/" + theClip);
            else
                options = new RestClientOptions("https://api.d-id.com/talks/" + theClip);
            var client = new RestClient(options);
            var request = new RestRequest();
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization",
                Secrets.ID_Key);

            string resultUrl = "Not found";

            // Retries to check for 200 OK and existing result_url
            for (int retry = 0; retry < 10; retry++)
            {
                var response = await client.GetAsync(request);

                if (response.StatusCode == HttpStatusCode.OK && response.Content != null)
                {
                    var jsonDocument = JsonDocument.Parse(response.Content);

                    if (jsonDocument.RootElement.TryGetProperty("result_url", out JsonElement resultUrlElement))
                    {
                        resultUrl = resultUrlElement.GetString() ?? "Not found";

                        if (resultUrl != "Not found")
                        {
                            break; // Exit loop if URL is found
                        }
                    }
                }

                await Task.Delay(1000); // Delay before retrying
            }
            return resultUrl;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + ":" + e.StackTrace);
            throw;
        }
    }
    public static async Task<string> DownloadVideoAsync(string url)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new Exception("Failed to download video.");

        string fileName = "";
        if (GlobalVars.videoTalked.Contains("Interactive"))
        {
            fileName = "TalkedInteractive.mp4";
            
        }
        if (GlobalVars.videoTalked.Contains("Bill"))
        {
            if(GlobalVars.anim)
                fileName = "TalkedAnimation.mp4";
            else
                fileName = "TalkedBill.mp4";
            
        }

        if(GlobalVars.videoTalked.Contains("Elon"))
        {
            if(GlobalVars.anim)
                fileName = "TalkedAnimation.mp4";
            else
                fileName = "TalkedElon.mp4";
            
        }
            
        if(GlobalVars.videoTalked.Contains("Google"))
        {
            if(GlobalVars.anim)
                fileName = "TalkedAnimation.mp4";
            else
                fileName = "TalkedGoogle.mp4";
            
        }
        
        string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

        using (var stream = await response.Content.ReadAsStreamAsync())
        using (var fileStream = File.Create(filePath))
        {
            await stream.CopyToAsync(fileStream);
        }

        return filePath;
    }
    public static async Task<string> PlayExistingVideo(string _Video)
    {
        string fileName = _Video;
        string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

        return filePath;
    }
}