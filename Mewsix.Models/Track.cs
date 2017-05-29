using Mewsix.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mewsix.Models
{
    public class Track : IEquatable<Track>, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private string _ID;
        public string ID
        {
            get
            {
                return _ID;
            }
            private set
            {
                _ID = value;
            }
        }


        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _Artists;
        public string Artists
        {
            get { return _Artists; }
            set
            {
                _Artists = value;
                OnPropertyChanged(nameof(Artists));
            }
        }

        private Uri _AlbumUri;
        public Uri AlbumUri
        {
            get { return _AlbumUri; }
            set
            {
                _AlbumUri = value;
                OnPropertyChanged(nameof(AlbumUri));
            }
        }

        public void UpdateImage()
        {
            AlbumUri = new Uri(AlbumImageLinkRetriever.GiveAlbumImageLink(Title, Artists));
        }

        private string _Album;
        public string Album
        {
            get { return _Album; }
            set
            {
                _Album = value;
                OnPropertyChanged(nameof(Album));
            }
        }

        private string _Year;

        public string Year
        {
            get { return _Year; }
            set
            {
                _Year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        private string _Lyrics;
        public string Lyrics
        {
            get { return _Lyrics; }
            set
            {
                _Lyrics = value;
                OnPropertyChanged(nameof(Year));
            }
        }
        private string _TrackPath;

        public string Path
        {
            get { return _TrackPath; }
            private set
            {
                _TrackPath = value;
                OnPropertyChanged(nameof(Path));
            }
        }

        public Track(PocoTrack pocotrack)
        {
            Title = pocotrack.Title;
            Artists = pocotrack.Artists;
            AlbumUri = new Uri(pocotrack.AlbumUri);
            Album = pocotrack.Album;
            Year = pocotrack.Year;
            Lyrics = pocotrack.Lyrics;
            Path = pocotrack.TrackPath;
            ID = pocotrack.ID;
        }

        public Track(string title, string artists, string uri)
        {
            Title = title;
            Artists = artists;
            AlbumUri = new Uri(uri);
            ID = IdGenerator.GetID();
        }

        public Track(string title, string artists, string uri, string album, string year, string lyrics) : this(title, artists, uri)
        {
            Album = album;
            Year = year;
            Lyrics = lyrics;
            ID = IdGenerator.GetID();
        }

        public Track(string trackPath, MusicID3Tag t, string albumUri) : this(t.Title, t.Artists.ToList().Aggregate((i, j) => i + ", " + j), albumUri, t.Album, t.Year, t.Lyrics)
        {
            Path = trackPath;
            ID = IdGenerator.GetID();
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
            return ID == other.ID;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode() % 31;
        }
    }
}
