using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace Mewsix.Helpers
{
    public class TrackLyricsRetrieverService
    {
        private static TrackLyricsRetrieverService _instance;
        /// <summary>
        /// Gets singleton instance of the <see cref="TrackLyricsRetrieverService"/>
        /// </summary>
        public static TrackLyricsRetrieverService Instance
        {
            get
            {
                if (_instance == null) _instance = new TrackLyricsRetrieverService();

                return _instance;
            }
        }

        /// <summary>
        /// Initialize a default <see cref="TrackLyricsRetrieverService"/> who manage players
        /// </summary>
        private TrackLyricsRetrieverService() { }

        private static readonly string MUSIXMATCH_AUTH_KEY = "f2a6f30c5bbd5d93e582d93b03f32d1e";
        public async Task<string> GiveTrackLyricsAsync(string trackTitle, string artist)
        {
            string lyrics = "No lyrics have been found!";
            if (artist == null) { artist = ""; }
            if (trackTitle == null) { trackTitle = ""; }

            using (HttpClient c = new HttpClient())
            {
                try
                {
                    string json = await c.GetStringAsync("http://api.musixmatch.com/ws/1.1/matcher.lyrics.get?apikey=" + MUSIXMATCH_AUTH_KEY + "&q_track=" + trackTitle.ToLower().Replace(" ", "%20") + "&q_artist=" + artist.ToLower().Replace(" ", "%20"));
                    try
                    {
                        RootObject parsedObject = JsonConvert.DeserializeObject<RootObject>(json);
                        if (parsedObject.message.body == null) return lyrics;
                        if (!String.IsNullOrWhiteSpace(parsedObject.message.body.lyrics.lyrics_body))
                        {
                            lyrics = parsedObject.message.body.lyrics.lyrics_body;

                            lyrics = lyrics.Substring(0, lyrics.IndexOf("*******"));
                        }

                    }
                    catch (JsonSerializationException e)
                    {
                        Debug.WriteLine(e.Message);
                        return lyrics;
                    }
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                    return lyrics;
                }
                
            }
            return lyrics;

        }

        private class Header
        {
            public int status_code { get; set; }
            public double execute_time { get; set; }
        }

        private class Lyrics
        {
            public int lyrics_id { get; set; }
            public int can_edit { get; set; }
            public int locked { get; set; }
            public string action_requested { get; set; }
            public int verified { get; set; }
            public int restricted { get; set; }
            public int instrumental { get; set; }
            public int @explicit { get; set; }
            public string lyrics_body { get; set; }
            public string lyrics_language { get; set; }
            public string lyrics_language_description { get; set; }
            public string script_tracking_url { get; set; }
            public string pixel_tracking_url { get; set; }
            public string html_tracking_url { get; set; }
            public string lyrics_copyright { get; set; }
            public List<object> writer_list { get; set; }
            public List<object> publisher_list { get; set; }
            public string updated_time { get; set; }
        }

        private class Body
        {
            public Lyrics lyrics { get; set; }
        }

        private class Message
        {
            public Header header { get; set; }
            public Body body { get; set; }
        }

        private class RootObject
        {
            public Message message { get; set; }
        }
    }

}
