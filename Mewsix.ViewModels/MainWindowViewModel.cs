using Mewsix.Helpers;
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
                new Track("Like A Stone", "Audioslave", "https://images-na.ssl-images-amazon.com/images/I/81SPG6dHDXL._SL1500_.jpg")};

            TrackUris = new TracksPathList<String>();
            TrackUris.OnAdd += TrackUris_OnAdd;
        }

        private void TrackUris_OnAdd(object sender, TrackPathEventArgs<String> e)
        {
            MusicID3Tag tag = new MusicID3Tag(e.TrackPath);     
            Tracks.Add(new Track(e.TrackPath, tag, "https://images-na.ssl-images-amazon.com/images/I/81SPG6dHDXL._SL1500_.jpg"));
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }




    }


}
