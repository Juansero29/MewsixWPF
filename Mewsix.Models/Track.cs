using Mewsix.Helpers;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

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
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }


        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_ID != value)
                {
                    _Title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private string[] _Artists;
        public string[] Artists
        {
            get { return _Artists; }
            set
            {
                if (_Artists != value)
                {
                    _Artists = value;
                    OnPropertyChanged(nameof(Artists));
                }
            }
        }

        private Uri _AlbumUri;
        public Uri AlbumUri
        {
            get { return _AlbumUri; }
            set
            {
                if (_AlbumUri != value)
                {
                    _AlbumUri = value;
                    OnPropertyChanged(nameof(AlbumUri));
                }
            }
        }

        private string _Album;
        public string Album
        {
            get { return _Album; }
            set
            {
                if (_Album != value)
                {
                    _Album = value;
                    OnPropertyChanged(nameof(Album));
                }
            }
        }

        private string _Year;
        public string Year
        {
            get { return _Year; }
            set
            {
                if (_Year != value)
                {
                    _Year = value;
                    OnPropertyChanged(nameof(Year));
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }

        private string _genre;
        public string Genre
        {
            get { return _genre; }
            set
            {
                if (_genre != value)
                {
                    _genre = value;
                    OnPropertyChanged(nameof(Genre));
                }
            }
        }

        private string _Lyrics;
        public string Lyrics
        {
            get { return _Lyrics; }
            set
            {
                if (_Lyrics != value)
                {
                    _Lyrics = value;
                    OnPropertyChanged(nameof(Year));
                }
            }
        }

        private string _TrackPath;
        public string Path
        {
            get { return _TrackPath; }
            private set
            {
                if (_TrackPath != value)
                {
                    _TrackPath = value;
                    OnPropertyChanged(nameof(Path));
                }
            }
        }

        private string _Summary;
        public string Summary
        {
            get { return _Summary; }
            set
            {
                _Summary = value;
                OnPropertyChanged(nameof(Summary));
            }
        }

        public Track(PocoTrack pocotrack)
        {
            Title = pocotrack.Title;
            Artists = pocotrack.Artists;
            AlbumUri = new Uri(pocotrack.AlbumUri);
            Album = pocotrack.Album;
            Year = pocotrack.Year;
            Comment = pocotrack.Comment;
            Genre = pocotrack.Genre;
            Lyrics = pocotrack.Lyrics;
            Summary = pocotrack.Summary;
            Path = pocotrack.TrackPath;
            ID = pocotrack.ID;
        }

        public Track(string title, string[] artists, string uri)
        {
            Title = title;
            Artists = artists;
            AlbumUri = new Uri(uri);
            ID = IdGenerator.GetID();
        }


        public Track(string title, string[] artists, string uri, string album, string year, string comment, string genre, string lyrics) : this(title, artists, uri)
        {
            Album = album;
            Year = year;
            Comment = comment;
            Genre = genre;
            Lyrics = lyrics;
            ID = IdGenerator.GetID();
        }

        /*public Track(string trackPath, MusicID3Tag t, string albumUri) : this(t.Title, t.Artists, albumUri, t.Album, t.Year, t.Lyrics)
        {
            Path = trackPath;
            ID = IdGenerator.GetID();
        }*/

        public Track(string trackPath, MusicID3Tag t, string albumUri, string lyricsString) : this(t.Title, t.Artists, albumUri, t.Album, t.Year, t.Comment, t.Genre, t.Lyrics)
        {
            Lyrics = lyricsString;
            Path = trackPath;
            ID = IdGenerator.GetID();
        }

        public Track(string trackPath, MusicID3Tag t, string albumUri, string lyricsString, string summary) : this(t.Title, t.Artists, albumUri, t.Album, t.Year, t.Comment, t.Genre, t.Lyrics)
        {
            Lyrics = lyricsString;
            Summary = summary;
            Path = trackPath;
            ID = IdGenerator.GetID();
        }


        public async void UpdateContents()
        {
            AlbumUri = new Uri(await AlbumImageLinkRetriever.GiveAlbumImageLink(Title, Artists) as string);
            Summary = await WikiSummaryRetriever.GiveTrackSummary(Artists[0]);
            Lyrics = await TrackLyricsRetriever.GiveTrackLyrics(Title, Artists[0]);
        }

        public override string ToString()
        {
            return Title + " by " + Artists.Aggregate((i, j) => i + ", " + j);
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
            if (ID == other.ID) return true;
            return Title == other.Title && Artists[0] == other.Artists[0];
        }

        public override int GetHashCode()
        {
            //DON'T USE A PROPERTY THAT CHANGES FOR RETURNING THE HASHCODE!!!!11!!!!!!!
            //if (Title != null) return Math.Abs(Title.GetHashCode()) % 31;
            //else return ID.GetHashCode() % 31; 
            return ID.GetHashCode() % 31;
        }
    }
}
