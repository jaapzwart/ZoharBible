using System.Net;
using System.Text;

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

