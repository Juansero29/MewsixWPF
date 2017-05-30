using Mewsix.Helpers;
using Mewsix.Models;
using Mewsix.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace Mewsix.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IDataManager DataManager { get; set; }

        private ObservableCollection<Track> _Tracks;
        public ObservableCollection<Track> Tracks
        {
            get
            {
                return _Tracks;
            }
            set
            {
                _Tracks = value;
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
                if (value < 0)
                {
                    _selectedIndex = Tracks.Count - 1;
                    if (!Tracks.ToList().Any())
                    {
                        SelectedTrack = null;
                    }
                    else
                    {
                        SelectedTrack = Tracks[SelectedIndex];
                    }
                    Debug.Print("selectedIndex : " + value);
                    return;
                }
                if (value > Tracks.Count - 1)
                {
                    _selectedIndex = 0;
                    SelectedTrack = Tracks[SelectedIndex];
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
            if (DataManager.Tracks != null) _Tracks = new ObservableCollection<Track>(DataManager.Tracks);

            if (Tracks != null && Tracks.ToList().Count > 0) { SelectedTrack = Tracks[0]; } else { SelectedTrack = null; }
            MPlayer = new MewsixPlayer(TextBlock_Current_Time, TextBlock_Total_Time, Slider_Time);
        }

        private void OnTrackPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Track t = sender as Track;
            if (t == null) return;
            Update(t);
        }

        public void AddTrack(string trackPath)
        {
            MusicID3Tag tag = new MusicID3Tag(trackPath);
            Track newTrack = new Track(trackPath, tag, AlbumImageLinkRetriever.GiveAlbumImageLink(tag.Title, tag.Artists[0]));
            newTrack.PropertyChanged += OnTrackPropertyChanged;
            if (Tracks == null) Tracks = new ObservableCollection<Track>();
            if (Tracks == null || !Tracks.Contains(newTrack))
            {
                Tracks.Add(newTrack);
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
            /* REMOVE FROM VIEW MODEL'S DATA */
            Tracks.Remove(t);

            /* REMOVE FROM DATA MANAGER'S DATA*/
            DataManager.Remove(t);

            OnPropertyChanged(nameof(Tracks));
            if (Tracks.Count == 0) SelectedTrack = null;
            SelectedTrack = Tracks[0];
        }

        public void Update(Track t)
        {
            /* UPDATE FOR THE VIEW MODEL'S DATA */
            for (int i = Tracks.Count - 1; i >= 0; i--)
            {
                if (Tracks[i].Equals(t))
                {
                    Tracks.RemoveAt(i);
                    Tracks.Add(t);
                }
            }

            /* UPDATE FOR THE DATA MANAGER'S DATA */
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
