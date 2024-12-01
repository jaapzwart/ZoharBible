using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Text.Json;
using OpenAI.API.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ZoharBible
{
    public class MainViewModelChatGPT : INotifyPropertyChanged
    {
        private string _generatedImageUrl;
        private readonly HttpClient _httpClient;

        public MainViewModelChatGPT()
        {
            _httpClient = new HttpClient();
            GenerateImageCommand = new Command(async () => await GenerateImageAsync());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // URL of the generated image
        public string GeneratedImageUrl
        {
            get => _generatedImageUrl;
            set
            {
                _generatedImageUrl = value;
                OnPropertyChanged(nameof(GeneratedImageUrl));
            }
        }

        // Command to trigger image generation
        public ICommand GenerateImageCommand { get; }

        private async Task GenerateImageAsync()
        {
            string apiKey = Secrets.DallE_key;  // Replace with your actual OpenAI API key
            string endpoint = Secrets.urlDalle;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                model = "dall-e-3",  // Model name
                prompt = GlobalVars.DallE_Image_string,  // Your prompt
                size = "1024x1024",  // Image size
                quality = "standard",  // Image quality
                n = 1  // Number of images to generate
            };

            string jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(endpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string imageUrl = result.data[0].url;
                    GeneratedImageUrl = imageUrl;
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                
            }
            
        }
        

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ImageResponse
    {
        public List<ImageData> Data { get; set; }
    }

    public class ImageData
    {
        public string Url { get; set; }
    }
}
