using Mewsix.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mewsix.Models
{
    public class Track
    {

        public string Title { get; set; }
        public string Artist { get; private set; }
        public Uri AlbumUri { get; private set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public string Lyrics { get; private set; }

        public string TrackPath { get; private set; }


        public Track(string title, string artist, string uri)
        {
            Title = title;
            Artist = artist;
            AlbumUri = new Uri(uri);
        }

        public Track(string title, string artist, string uri, string album, string year, string lyrics) : this(title, artist, uri)
        {
            Album = album;
            Year = year;
            Lyrics = lyrics;
        }

        public Track(string trackPath, MusicID3Tag t, string albumUri) : this(t.Title, t.Artist, albumUri, t.Album, t.Year, t.Lyrics)
        {
            TrackPath = trackPath;
        }

        public override string ToString()
        {
            return Title + " by " + Artist;
        }
    }
}
