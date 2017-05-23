using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mewsix.Models
{
    public class Track
    {

        public string Title { get; private set; }
        public string Artist { get; private set; }
        public Uri AlbumUri { get; private set; }


        public string Lyrics { get; private set; }

        public Track(string title, string artist, string uri)
        {
            Title = title;
            Artist = artist;
            AlbumUri = new Uri(uri);
        }

        public override string ToString()
        {
            return Title + " by " + Artist;
        }
    }
}
