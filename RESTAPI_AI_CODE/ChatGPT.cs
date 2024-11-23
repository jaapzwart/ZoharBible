//-------------------------------------------------------
// Down code sits behind a REST API GET call in Azure.
// It calls for ChatGPT giving it a string to work on.
// This is called in ZoharBible when ChatGPT is chosen.
// You have to make it work for your own environment.
// The code is example code.
//-------------------------------------------------------

/*
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.OpenApi;
using Nest;
using OpenAI_API.Completions;
using OpenAI.API.Completions;

namespace ZoharBible.RESTAPI_AI_CODE;

public class ChatGPT
{
    [HttpGet("{textToTranslate}")]
        public async Task<string> Get(string textToTranslate)
        {
            try
            {
                string getDate = DateTime.Now.Hour.ToString("d") + DateTime.Now.Minute.ToString("d") +
                    DateTime.Now.Second.ToString("d") + DateTime.Now.Millisecond.ToString("d");
                string cleanedString = Regex.Replace(textToTranslate, @"[^a-zA-Z\s]+", "");
                cleanedString = cleanedString.Replace(" ", "_");
                await writeFileToBlob(textToTranslate, getDate + " " + cleanedString + ".txt");
                OpenAI_API openAI = new OpenAI_API.OpenAIAPI("KEY");
                string temperature = readFileFromBlob("temperatureChatGPT.txt", "BLOBNAME");
                double _temperature = Convert.ToDouble(temperature);
                string maxtokensChatGPT = readFileFromBlob("maxTokensChatGPT.txt", "BLOBNAME");
                int _maxtokensChatGPT = Convert.ToInt32(maxtokensChatGPT);

                CompletionRequest completion = new CompletionRequest();
                completion.Prompt = textToTranslate;
                completion.MaxTokens = _maxtokensChatGPT;
                completion.Temperature = _temperature;
                completion.Model = "gpt-3.5-turbo-instruct"; // Set the model ID for GPT-3.5-turbo
                //completion.Model = "text-davinci-003";
                var result = await openAI.Completions.CreateCompletionAsync(completion);
                string answer = "";
                if (result != null)
                {
                    foreach (var item in result.Completions)
                    {
                        answer += "" + item.Text;
                    }
                    return answer;
                }
                else
                    return "No results from BlackBeltBible AI.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    private async Task<string> writeFileToBlob(string writeToBlobKentekenControle, string fileName)
    {
        try
        {
            // Initialise client in a different place if you like
            string connS = "CONNECTION STRING";
            CloudStorageAccount account = CloudStorageAccount.Parse(connS);
            var blobClient = account.CreateCloudBlobClient();

            // Make sure container is there
            var blobContainer = blobClient.GetContainerReference("chatgpt");
            await blobContainer.CreateIfNotExistsAsync();

            WebClient wc = new WebClient();
            using (Stream fs = wc.OpenWrite("URL" + fileName))
            {
                TextWriter tw = new StreamWriter(fs);
                CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(
                    fileName);
                await blockBlob.UploadTextAsync(writeToBlobKentekenControle);
                //tw.Flush();
            }
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public static string readFileFromBlob(string fileName, string blobber)
    {
        try
        {
            // Initialise client in a different place if you like
            string connS = "CONNECTION STRING";
            CloudStorageAccount account = CloudStorageAccount.Parse(connS);
            var blobClient = account.CreateCloudBlobClient();

            var blobContainer = blobClient.GetContainerReference(blobber);
            blobContainer.CreateIfNotExistsAsync();

            CloudBlockBlob blob = blobContainer.GetBlockBlobReference($"{fileName}");
            string contents = blob.DownloadTextAsync().Result;

            return contents;
        }
        catch (Exception ex)
        {
            return "0";
        }
    }
}
*/