using Mewsix.Helpers;
using Mewsix.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using System.IO;
using System.Windows.Input;
using Mewsix.ViewModels.Commands;
using System.Windows;

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
                if (!(String.IsNullOrEmpty(SearchCriteria) || String.IsNullOrWhiteSpace(SearchCriteria)))
                {
                    return new ObservableCollection<Track>(_Tracks.Where(t => t.Title.ToLower().Contains(SearchCriteria.ToLower()) || t.Artists[0].ToLower().Contains(SearchCriteria.ToLower())));
                }
                else
                {
                    return _Tracks;
                }

            }
            set
            {
                _Tracks = value;
            }
        }

        private string _SearchCriteria;

        public string SearchCriteria
        {
            get { return _SearchCriteria; }
            set
            {
                _SearchCriteria = value;
                OnPropertyChanged(nameof(SearchCriteria));
                OnPropertyChanged(nameof(Tracks));
            }
        }


        public MewsixPlayer MPlayer { get; set; }

        Track _selectedTrack;
        public Track SelectedTrack
        {
            get
            {
                if (Tracks == null || Tracks.Count == 0 || SelectedIndex < 0 || SelectedIndex >= Tracks.Count) return null;
                return Tracks[SelectedIndex];
                
 //               return _selectedTrack;
            }

            //set
            //{
            //    _selectedTrack = value;
            //    OnPropertyChanged(nameof(SelectedTrack));
            //}
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
                if (value == _selectedIndex) return;


                if (value < 0)
                {
                    _selectedIndex = Tracks.Count - 1;
                    //if (!Tracks.ToList().Any())
                    //{
                    //    SelectedTrack = null;
                    //}
                    //else
                    //{
                    //    SelectedTrack = Tracks[SelectedIndex];
                    //}
                    Debug.Print("selectedIndex : " + value);
                    OnPropertyChanged(nameof(SelectedIndex));
                    OnPropertyChanged(nameof(SelectedTrack));
                    return;
                }
                if (value > Tracks.Count - 1)
                {
                    _selectedIndex = 0;
                    //SelectedTrack = Tracks[SelectedIndex];
                    Debug.Print("selectedIndex : " + value);
                    return;
                }



                _selectedIndex = value;
                //SelectedTrack = Tracks[SelectedIndex];
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(SelectedTrack));
                Debug.Print("selectedIndex : " + value);

                _playPauseTrackButtonClickCommand.RaiseCanExecute();
                _nextTrackButtonClickCommand.RaiseCanExecute();
                _previousTrackButtonClickCommand.RaiseCanExecute();


            }
        }

        public MainWindowViewModel()
        {
            SearchCriteria = "   ";
            DataManager = new Data.Data();
            if (DataManager.Tracks != null) _Tracks = new ObservableCollection<Track>(DataManager.Tracks);

<<<<<<< HEAD
=======

>>>>>>> 1fbdaf38e18e5fd1bb1a50525873f02af6b1358c
            if (Tracks != null && Tracks.ToList().Count > 0)
            {
                SelectedIndex = 0;
            }
            else
            {
                SelectedIndex = -1;
            }//{ SelectedTrack = Tracks[0]; } else { SelectedTrack = null; }
<<<<<<< HEAD
            MPlayer = new MewsixPlayer();
=======
            MPlayer = MewsixPlayer.Instance;

>>>>>>> 1fbdaf38e18e5fd1bb1a50525873f02af6b1358c


            /* COMMAND CREATION AND DEFINITION OF THE FUNCTION WE WANT TO CALL WHEN ACTION IS INVOKED */
            _previewMouseDownCommand = new MewsixCommand(MPlayer.OnPreviewMouseDown, t => true);
            _previewMouseUpCommand = new MewsixCommand(MPlayer.OnPreviewMouseUp, t => true);
            _singleTrackAddCommand = new MewsixCommand(OnAddButtonClicked, t => true);
            _singleFolderAddCommand = new MewsixCommand(AddFolder, t => true);
<<<<<<< HEAD
=======

>>>>>>> 1fbdaf38e18e5fd1bb1a50525873f02af6b1358c
            _removeItemClickCommand = new MewsixCommand(() => Remove(SelectedTrack), t => true);
            _windowClosingCommand = new MewsixCommand(OnWindowClosing, t => true);

            _playPauseTrackButtonClickCommand = new MewsixCommand(PlaySelectedTrack, t => Tracks.Any());
            _nextTrackButtonClickCommand = new MewsixCommand(PlayNext, t => Tracks.Any());
            _previousTrackButtonClickCommand = new MewsixCommand(PlayPrevious, t => Tracks.Any());


        }


        /* DEFINITION OF COMMAND REFERENCES THAT WILL BE USED WHEN EVENT IS TRIGGERED */

        /// <summary>
        /// Command invoked when the slider PreviewMouseUp event is launched.
        /// </summary>
        private readonly MewsixCommand _previewMouseDownCommand;
        public ICommand PreviewMouseDownCommand => _previewMouseDownCommand;

        /// <summary>
        /// Command invoked when the slider PreviewMouseUp event is launched.
        /// </summary>
        private readonly MewsixCommand _previewMouseUpCommand;
        public ICommand PreviewMouseUpCommand => _previewMouseUpCommand;

        /// <summary>
        /// Command invoked when the button to add a single track is clicked.
        /// </summary>
        private readonly MewsixCommand _singleTrackAddCommand;
        public ICommand SingleTrackAddCommand => _singleTrackAddCommand;

        /// <summary>
        /// Command invoked when the button to add a single folder is clicked.
        /// </summary>
        private readonly MewsixCommand _singleFolderAddCommand;
        public ICommand SingleFolderAddCommand => _singleFolderAddCommand;

        /// <summary>
        /// Command invoked when the button to add a single folder is clicked.
        /// </summary>
        private readonly MewsixCommand _playPauseTrackButtonClickCommand;
        public ICommand PlayPauseTrackButtonClickCommand => _playPauseTrackButtonClickCommand;

        /// <summary>
        /// Command invoked when the button to add a single folder is clicked.
        /// </summary>
        private readonly MewsixCommand _previousTrackButtonClickCommand;
        public ICommand PreviousTrackButtonClickCommand => _previousTrackButtonClickCommand;

        /// <summary>
        /// Command invoked when the button to add a single folder is clicked.
        /// </summary>
        private readonly MewsixCommand _nextTrackButtonClickCommand;
        public ICommand NextTrackButtonClickCommand => _nextTrackButtonClickCommand;

        /// <summary>
        /// Command invoked when the button to add a single folder is clicked.
        /// </summary>
        private readonly MewsixCommand _removeItemClickCommand;
        public ICommand RemoveItemClickCommand => _removeItemClickCommand;


        /// <summary>
        /// Command invoked when the button to add a single folder is clicked.
        /// </summary>
        private readonly MewsixCommand _windowClosingCommand;
        public ICommand WindowClosingCommand => _windowClosingCommand;


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
                currentTrack.UpdateContents();
                TagLib.File file = TagLib.File.Create(currentTrack.Path);
                TagLib.Tag tag = file.Tag;

                tag.Title = currentTrack.Title;
                tag.Album = currentTrack.Album;
                tag.AlbumArtists = currentTrack.Artists;
                tag.Year = Convert.ToUInt32(currentTrack.Year);
                tag.Comment = currentTrack.Comment;
                // TODO tag.Genre = currentTrack.Genre;

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

        private void OnTrackPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Track t = sender as Track;
            if (t == null) return;
            Update(t);
        }

        public void OnWindowClosing()
        {
            SearchCriteria = null;
            DataManager.Save(Tracks);
        }

        public void OnAddButtonClicked()
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

        public async void AddTrack(string trackPath)
        {
            SearchCriteria = null;
            MusicID3Tag tag = new MusicID3Tag(trackPath);
            string albumUri = await AlbumImageLinkRetriever.GiveAlbumImageLink(tag.Title, tag.Artists);
            string lyrics = await TrackLyricsRetriever.GiveTrackLyrics(tag.Title, tag.Artists[0]);
            string summary = await WikiSummaryRetriever.GiveTrackSummary(tag.Artists[0]);
            Track newTrack = new Track(trackPath, tag, albumUri, lyrics, summary);
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
            SearchCriteria = null;
            /* REMOVE FROM VIEW MODEL'S DATA */
            int oldIndex = SelectedIndex;
            string oldTrackPath = t.Path;
            Tracks.Remove(t);

            /* REMOVE FROM DATA MANAGER'S DATA*/
            DataManager.Remove(t);

            if (Tracks.Count <= 0)
            {
                //SelectedTrack = null;
                SelectedIndex = -1;
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
            SearchCriteria = null;
            /* UPDATE FOR THE VIEW MODEL'S DATA */
            for (int i = Tracks.Count - 1; i >= 0; i--)
            {
                if (Tracks[i].Equals(t))
                {
                    Tracks.RemoveAt(i);
                    Tracks.Insert(i, t);
                }
            }

            //SelectedTrack = Tracks[Tracks.ToList().FindIndex(track => track.ID == t.ID)];
            SelectedIndex = Tracks.ToList().FindIndex(track => track.ID == t.ID);
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

        public void PlayNext()
        {
            if (!MPlayer.IsOpened) { SelectedIndex++; }
            else
            {
                SelectedIndex = GetCurrentlyPlayingIndex() + 1;
                MPlayer.NewTrack(SelectedTrack.Path);
            }
        }

        public void PlayPrevious()
        {
            if (!MPlayer.IsOpened) { SelectedIndex--; }
            else
            {
                SelectedIndex = GetCurrentlyPlayingIndex() - 1;
                MPlayer.NewTrack(SelectedTrack.Path);
            }
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
            SearchCriteria = null;
            // The index of the currently playing song which may be different from the selected song
            Track currentlyPlaying = Tracks.Where(t => t.Path.Equals(MPlayer.CurrentTrackPath)).First();
            return Tracks.IndexOf(currentlyPlaying);
        }


    }
}
