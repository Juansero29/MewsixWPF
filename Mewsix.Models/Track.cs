using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mewsix.Models
{
    public class Track
    {
        public string Name { get; private set; }
        public string Artist { get; private set; }
        public Uri AlbumUri { get; private set; }

        public string Lyrics { get; private set; }

        public Track(string name, string artist, string uri)
        {
            Name = name;
            Artist = artist;
            AlbumUri = new Uri(uri);
        }
    }
}
