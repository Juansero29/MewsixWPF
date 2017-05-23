using Mewsix.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mewsix.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Track> Tracks { get; set; }
        public TracksPathList<String> TrackUris { get; set; }

        Track _selectedTrack;
        public Track SelectedTrack
        {
            get
            {
                return _selectedTrack;
            }

            set
            {
                _selectedTrack = value;
                OnPropertyChanged(nameof(SelectedTrack));
            }
        }


        public MainWindowViewModel()
        {
            Tracks = new ObservableCollection<Track> {
                new Track("Creep", "Radiohead", "https://images.genius.com/dc6a81658957cf95dc7a5834b6321b7a.300x300x1.jpg"),
                new Track("Young Stuff", "Snarky Puppy", "https://s3.amazonaws.com/bit-photos/large/6303622.jpeg"),
                new Track("Like A Stone", "Audioslave", "https://images-na.ssl-images-amazon.com/images/I/81SPG6dHDXL._SL1500_.jpg"),
                new Track("Like A Stone", "Audioslave", "https://images-na.ssl-images-amazon.com/images/I/81SPG6dHDXL._SL1500_.jpg"),
                new Track("Like A Stone", "Audioslave", "https://images-na.ssl-images-amazon.com/images/I/81SPG6dHDXL._SL1500_.jpg")};

            TrackUris = new TracksPathList<String>();
            TrackUris.OnAdd += TrackUris_OnAdd;
        }

        private void TrackUris_OnAdd(object sender, TrackPathEventArgs<String> e)
        {
            // https://stackoverflow.com/questions/5782642/wpf-c-get-informations-from-audio-file

            using (FileStream fs = File.OpenRead(e.TrackPath))
            {
                if (fs.Length >= 128)
                {
                    MusicID3Tag tag = new MusicID3Tag();
                    fs.Seek(-128, SeekOrigin.End);
                    fs.Read(tag.TAGID, 0, tag.TAGID.Length);
                    fs.Read(tag.Title, 0, tag.Title.Length);
                    fs.Read(tag.Artist, 0, tag.Artist.Length);
                    fs.Read(tag.Album, 0, tag.Album.Length);
                    fs.Read(tag.Year, 0, tag.Year.Length);
                    fs.Read(tag.Comment, 0, tag.Comment.Length);
                    fs.Read(tag.Genre, 0, tag.Genre.Length);
                    string theTAGID = Encoding.Default.GetString(tag.TAGID);

                    if (theTAGID.Equals("TAG"))
                    {
                        string Title = Encoding.Default.GetString(tag.Title);
                        string Artist = Encoding.Default.GetString(tag.Artist);
                        string Album = Encoding.Default.GetString(tag.Album);
                        string Year = Encoding.Default.GetString(tag.Year);
                        string Comment = Encoding.Default.GetString(tag.Comment);
                        string Genre = Encoding.Default.GetString(tag.Genre);

                        Console.WriteLine(Title);
                        Console.WriteLine(Artist);
                        Console.WriteLine(Album);
                        Console.WriteLine(Year);
                        Console.WriteLine(Comment);
                        Console.WriteLine(Genre);
                        Console.WriteLine();

                        Title = Title.Replace("\0", "");
                        Artist = Artist.Replace("\0", "");

                        Tracks.Add(new Track(Title, Artist, "https://images-na.ssl-images-amazon.com/images/I/81SPG6dHDXL._SL1500_.jpg"));

                    }
                }
            }

        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }




    }


    class MusicID3Tag

    {

        public byte[] TAGID = new byte[3];      //  3
        public byte[] Title = new byte[30];     //  30
        public byte[] Artist = new byte[30];    //  30 
        public byte[] Album = new byte[30];     //  30 
        public byte[] Year = new byte[4];       //  4 
        public byte[] Comment = new byte[30];   //  30 
        public byte[] Genre = new byte[1];      //  1

    }
}
