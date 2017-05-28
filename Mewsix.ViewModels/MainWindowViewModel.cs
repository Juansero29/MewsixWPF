using Mewsix.Helpers;
using Mewsix.Models;
using Mewsix.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Controls;

namespace Mewsix.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IDataManager DataManager { get; set; }

        public ObservableCollection<Track> Tracks
        {
            get
            {
                if (DataManager.Tracks != null)
                {
                    return new ObservableCollection<Track>(DataManager.Tracks);
                }
                return null;

            }
            set
            {

            }
        }

        private MewsixPlayer MPlayer { get; set; }
        private bool IsPlaying { get; set; }
        private bool IsPaused { get; set; }

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

        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (value <= 0)
                {
                    _selectedIndex = Tracks.Count - 1;
                    Debug.Print("selectedIndex : " + value);
                    return;
                }
                if (value >= Tracks.Count - 1)
                {
                    _selectedIndex = 0;
                    Debug.Print("selectedIndex : " + value);
                    return;
                }

                _selectedIndex = value;
                SelectedTrack = Tracks[SelectedIndex];
                Debug.Print("selectedIndex : " + value);
            }
        }

        public MainWindowViewModel(TextBlock TextBlock_Current_Time, TextBlock TextBlock_Total_Time, Slider Slider_Time)
        {
            DataManager = new StubData();
            if (DataManager.Tracks != null) SelectedTrack = Tracks[0];
            MPlayer = new MewsixPlayer(TextBlock_Current_Time, TextBlock_Total_Time, Slider_Time);
        }

        public void AddTrack(string trackPath)
        {
            MusicID3Tag tag = new MusicID3Tag(trackPath);
            Track newTrack = new Track(trackPath, tag, AlbumImageLinkRetriever.GiveAlbumImageLink(tag.Title, tag.Artists[0]));
            if (Tracks == null)
            {
                DataManager.Add(newTrack);
                SelectedTrack = Tracks[Tracks.Count - 1];
            }
            else if (!Tracks.Contains(newTrack))
            {
                DataManager.Add(newTrack);
                SelectedTrack = Tracks[Tracks.Count - 1];
            }
            else
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

        public void PlaySelectedTrack()
        {
            if (MPlayer.CurrentTrackPath != SelectedTrack.Path || !MPlayer.IsOpened)
            {
                MPlayer.Play(SelectedTrack.Path);
            }
            else
            {
                if (!MPlayer.IsPaused)
                {
                    MPlayer.Pause();
                }
                else
                {
                    MPlayer.Resume();
                }
            }

        }

        public void PlayPrevious()
        {
            SelectedIndex--;
            MPlayer.NewTrack(SelectedTrack.Path);
        }

        public void PlayNext()
        {
            SelectedIndex++;
            MPlayer.NewTrack(SelectedTrack.Path);
        }


        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            DataManager.Save(Tracks);
            e.Cancel = false;
        }

    }
}
