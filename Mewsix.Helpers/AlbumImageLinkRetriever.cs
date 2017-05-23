﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Mewsix.Helpers
{
    public static class AlbumImageLinkRetriever
    {
        public static string GiveAlbumImageLink(string trackTitle, string artist)
        {
            string link;
            if (artist == null) { artist = ""; }
            if (trackTitle == null) { trackTitle = ""; }
            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString("https://api.deezer.com/search?q=" + artist.ToLower().Replace(" ", "_") + "_" + trackTitle.ToLower().Replace(" ", "_") + "&index=0&limit=2");
                RootObject parsedObject = JsonConvert.DeserializeObject<RootObject>(json);

                if (parsedObject.data.Count > 0)
                {
                    link = parsedObject.data[0].album.cover_big;
                }
                else
                {
                    link = "https://seositecheckup.com/articlephoto/404_error.png";
                }
            }
            return link;
        }

        public class Artist
        {
            public int id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
            public string picture { get; set; }
            public string picture_small { get; set; }
            public string picture_medium { get; set; }
            public string picture_big { get; set; }
            public string picture_xl { get; set; }
            public string tracklist { get; set; }
            public string type { get; set; }
        }

        public class Album
        {
            public int id { get; set; }
            public string title { get; set; }
            public string cover { get; set; }
            public string cover_small { get; set; }
            public string cover_medium { get; set; }
            public string cover_big { get; set; }
            public string cover_xl { get; set; }
            public string tracklist { get; set; }
            public string type { get; set; }
        }

        public class Datum
        {
            public int id { get; set; }
            public bool readable { get; set; }
            public string title { get; set; }
            public string title_short { get; set; }
            public string title_version { get; set; }
            public string link { get; set; }
            public int duration { get; set; }
            public int rank { get; set; }
            public bool explicit_lyrics { get; set; }
            public string preview { get; set; }
            public Artist artist { get; set; }
            public Album album { get; set; }
            public string type { get; set; }
        }

        public class RootObject
        {
            public List<Datum> data { get; set; }
            public int total { get; set; }
            public string next { get; set; }
        }
    }


}