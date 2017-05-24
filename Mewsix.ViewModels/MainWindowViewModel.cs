using Mewsix.Helpers;
using Mewsix.Models;
using Mewsix.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Mewsix.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IDataManager DataManager { get; set; }

        public ObservableCollection<Track> Tracks { get { return new ObservableCollection<Track>(DataManager.Tracks); } set { } }

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
            DataManager = new StubData();
            SelectedTrack = Tracks[1];
        }

        public void AddTrack(string trackPath)
        {
            MusicID3Tag tag = new MusicID3Tag(trackPath);
            Track newTrack = new Track(trackPath, tag, AlbumImageLinkRetriever.GiveAlbumImageLink(tag.Title, tag.Artists[0]));
            if (!Tracks.Contains(newTrack))
            {
                DataManager.Add(newTrack);
                SelectedTrack = Tracks[Tracks.Count - 1];
            } else
            {
                Debug.Print("This track has already been added! ");
            }
            OnPropertyChanged(nameof(Tracks));
        }

        public void Remove(Track t)
        {
            DataManager.Remove(t);
            OnPropertyChanged(nameof(Tracks));
            SelectedTrack = Tracks[0];
        }

        public void Update(Track t)
        {
            DataManager.Update(t);
            OnPropertyChanged(nameof(Tracks));
        }

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
