using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mewsix.Helpers
{
    public static class WikiSummaryRetriever
    {
        public static async Task<string> GiveTrackSummary(string artist)
        {
            if (artist == null) { artist = "This"; }
            string summary = $"{artist} is a great artist! That's all we know.";


            using (HttpClient c = new HttpClient())
            {
                try
                {
                    string json = await c.GetStringAsync("https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exlimit=max&explaintext&exintro&titles=" + artist.Replace(" ", "%20") + "&redirects=");
                    try
                    {
                        var responseJson = JsonConvert.DeserializeObject<SummaryRootObject>(json);
                        var firstKey = responseJson.query.pages.First().Key;
                        summary = responseJson.query.pages[firstKey].extract;
                        summary = summary.Substring(0, summary.IndexOf("\n"));
                    }
                    catch (JsonSerializationException e)
                    {
                        Debug.WriteLine(e.Message);
                        return summary;
                    }
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                    return summary;
                }

            }
            return summary;

        }
    }

    public class pageval
    {
        public int pageid { get; set; }
        public int ns { get; set; }
        public string title { get; set; }
        public string extract { get; set; }
    }


    public class Query
    {
        public Dictionary<string, pageval> pages { get; set; }
    }

    public class Limits
    {
        public int extracts { get; set; }
    }

    public class SummaryRootObject
    {
        public string batchcomplete { get; set; }
        public Query query { get; set; }
        public Limits limits { get; set; }
    }
}
