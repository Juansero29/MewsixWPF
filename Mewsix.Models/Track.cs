using Mewsix.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mewsix.Models
{
    public class Track : IEquatable<Track>
    {
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _Artists;
        public string Artists
        {
            get { return _Artists; }
            set { _Artists = value; }
        }

        private Uri _AlbumUri;
        public Uri AlbumUri
        {
            get { return _AlbumUri; }
            set { _AlbumUri = value; }
        }

        private string _Album;
        public string Album
        {
            get { return _Album; }
            set { _Album = value; }
        }

        private string _Year;

        public string Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        private string _Lyrics;
        public string Lyrics
        {
            get { return _Lyrics; }
            set { _Lyrics = value; }
        }
        private string _TrackPath;

        public string TrackPath
        {
            get { return _TrackPath; }
            private set { _TrackPath = value; }
        }

        public Track(PocoTrack pocotrack)
        {
            Title = pocotrack.Title;
            Artists = pocotrack.Artists;
            AlbumUri = new Uri(pocotrack.AlbumUri);
            Album = pocotrack.Album;
            Year = pocotrack.Year;
            Lyrics = pocotrack.Lyrics;
            TrackPath = pocotrack.TrackPath;
        }

        public Track(string title, string artists, string uri)
        {
            Title = title;
            Artists = artists;
            AlbumUri = new Uri(uri);
        }

        public Track(string title, string artists, string uri, string album, string year, string lyrics) : this(title, artists, uri)
        {
            Album = album;
            Year = year;
            Lyrics = lyrics;
        }

        public Track(string trackPath, MusicID3Tag t, string albumUri) : this(t.Title, t.Artists.ToList().Aggregate((i, j) => i + ", " + j), albumUri, t.Album, t.Year, t.Lyrics)
        {
            TrackPath = trackPath;
        }

        public override string ToString()
        {
            return Title + " by " + Artists;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (!obj.GetType().Equals(GetType())) return false;
            return Equals(obj as Track);
        }

        public bool Equals(Track other)
        {
            if (other == null) return false;
            return Title == other.Title && Artists == other.Artists && Year == other.Year;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode() % 31;
        }
    }
}
