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
using System;
using System.Windows.Forms;
using System.IO;
using System.Windows;
using System.Windows.Input;

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
                if (_Tracks == null) { return new ObservableCollection<Track>(); }
                return _Tracks;
            }
            set
            {
                _Tracks = value;
            }
        }

        private MewsixPlayer MPlayer { get; set; }

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

        public void AddButton()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Music files|*.mp3;*.flac",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string trackPath in openFileDialog.FileNames)
                {
                    AddTrack(trackPath);
                }
            }
        }

        public void OnObjectDroppedOnView(object sender, System.Windows.DragEventArgs e)
        {
            List<string> filepaths = new List<string>();
            string[] pathData = e.Data.GetData(System.Windows.DataFormats.FileDrop, false) as string[];
            if (pathData == null) return;
            foreach (var path in pathData)
            {
                //If there's a directory in the current path
                if (Directory.Exists(path))
                {
                    //Add files from folder
                    filepaths.AddRange(Directory.GetFiles(path));
                }
                else //If there isn't a directory, then there is a file.
                {
                    //Add filepath
                    filepaths.Add(path);
                }
            }

            //For each file path, if it's a music file, we add it to the tracks.
            foreach (var trackPath in filepaths)
            {
                if (Path.GetExtension(trackPath) == ".mp3" || Path.GetExtension(trackPath) == ".flac")
                {
                    AddTrack(trackPath);
                }
            }
        }

        public void OnTextBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Track currentTrack = SelectedTrack;
                currentTrack.UpdateImage();
                TagLib.File file = TagLib.File.Create(currentTrack.Path);
                TagLib.Tag tag = file.Tag;

                tag.Title = currentTrack.Title;
                tag.Album = currentTrack.Album;
                tag.Year = Convert.ToUInt32(currentTrack.Year);

                try
                {
                    file.Save();
                    Update(currentTrack);
                    System.Windows.MessageBox.Show("Track information updated !");

                }
                catch (UnauthorizedAccessException exception)
                {
                    System.Windows.MessageBox.Show("Couldn't update track info. Access denied.");
                    Debug.WriteLine(exception.ToString());
                }
                catch (Exception exception)
                {
                    System.Windows.MessageBox.Show("Couldn't update track info. An unknown exception occured");
                    Debug.WriteLine(exception.ToString());
                }

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
            string albumUri = AlbumImageLinkRetriever.GiveAlbumImageLink(tag.Title, tag.Artists?[0]);
            string lyrics = TrackLyricsRetriever.GiveTrackLyrics(tag.Title, tag.Artists[0]);
            Track newTrack = new Track(trackPath, tag, albumUri, lyrics);
            newTrack.PropertyChanged += OnTrackPropertyChanged;
            if (_Tracks == null) _Tracks = new ObservableCollection<Track>();

            if (!Tracks.Contains(newTrack))
            {
                Tracks.Add(newTrack);
                DataManager.Add(newTrack);
                SelectedIndex = Tracks.Count - 1;
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
            int oldIndex = SelectedIndex;
            string oldTrackPath = t.Path;
            Tracks.Remove(t);

            /* REMOVE FROM DATA MANAGER'S DATA*/
            DataManager.Remove(t);

            if (Tracks.Count <= 0)
            {
                SelectedTrack = null;
                return;
            }

            SelectedIndex = oldIndex;  // As if the next song in the list was selected

            if (MPlayer.IsOpened && oldTrackPath == MPlayer.CurrentTrackPath)
            {
                MPlayer.NewTrack(SelectedTrack.Path);
            }

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
            if (!Tracks.Any()) return;

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
            if (!Tracks.Any()) return;

            SelectedIndex = GetCurrentlyPlayingIndex() - 1;
            MPlayer.NewTrack(SelectedTrack.Path);
        }

        public void PlayNext()
        {
            if (!Tracks.Any()) return;
                       
            SelectedIndex = GetCurrentlyPlayingIndex() +1;
            MPlayer.NewTrack(SelectedTrack.Path);
        }

        public void AddFolder()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                     string[] files = Directory.GetFiles(fbd.SelectedPath);
                    foreach (string trackPath in files)
                    {
                        if (Path.GetExtension(trackPath) == ".mp3" || Path.GetExtension(trackPath) == ".flac")
                        {
                            AddTrack(trackPath);
                        }
                    }
                }


            }

        }

        private int GetCurrentlyPlayingIndex()
        {
            // The index of the currently playing song which may be different from the selected song
            Track currentlyPlaying = Tracks.Where(t => t.Path.Equals(MPlayer.CurrentTrackPath)).First();
            return Tracks.IndexOf(currentlyPlaying);
        }


        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            DataManager.Save(Tracks);
            e.Cancel = false;
        }

    }
}
