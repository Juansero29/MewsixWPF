using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Mewsix.Models
{
    public sealed class MewsixPlayer : MediaPlayer, INotifyPropertyChanged
    {
        public bool IsOpened { get; private set; }
        public bool IsPaused { get; private set; }

        public string CurrentTrackPath { get; private set; }

        public DispatcherTimer Timer { get; private set; } = new DispatcherTimer();
        public int Duration { get; private set; }
        private bool MouseDown { get; set; }

        private string _CurrentTime;
        public string CurrentTime { get { return _CurrentTime; } private set { _CurrentTime = value; OnPropertyChanged(nameof(CurrentTime)); } }

        private string _TotalTime;
        public string TotalTime
        {
            get
            {
                return _TotalTime;
            }
            private set
            {
                _TotalTime = value;
                OnPropertyChanged(nameof(TotalTime));
            }
        }

        private double _SliderValue;
        public double SliderValue
        {
            get { return _SliderValue; }
            set
            {
                _SliderValue = value;
                OnPropertyChanged(nameof(SliderValue));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private static MewsixPlayer _Instance = null;
        private static readonly object Padlock = new object();


        MewsixPlayer()
        {
            TotalTime = "0:00";
            CurrentTime = "0:00";
            MediaEnded += MewixPlayer_MediaEnded;
            MediaOpened += MewsixPlayer_MediaOpened;
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
        }

        public static MewsixPlayer Instance
        {
            get
            {
                /* The lock keyword ensures that one thread does not enter a critical
                 * section of code while another thread is in the critical section. 
                 * If another thread tries to enter a locked code, it will wait, 
                 * block, until the object is released. Unfortunately, performance
                 * suffers as a lock is acquired every time the instance is requested.*/
                lock (Padlock)
                {
                    if(_Instance == null)
                    {
                        _Instance = new MewsixPlayer();
                    }
                    return _Instance;
                }
            }
        }


        private void MewixPlayer_MediaEnded(object sender, EventArgs e)
        {
            IsOpened = false;
            Stop();
            Timer.Stop();
            Duration = 0;
        }

        // NaturalDuration.TimeSpan can only be called after the Media has been opened.
        private void MewsixPlayer_MediaOpened(object sender, EventArgs e)
        {
            TotalTime = NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            Duration = (int)NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentTime = String.Format($"{Position.ToString(@"m\:ss")}");
            if (Duration != 0 && !MouseDown)
            {
                SliderValue = (double)((float)Position.TotalSeconds / Duration) * 1000;
                //Debug.WriteLine(TimeSlider.Value);
            }
        }

        // MouseDown allows us to let the slider progress only if the user hasn't clicked on it
        public void OnPreviewMouseDown()
        {
            MouseDown = true;
        }

        public void OnPreviewMouseUp()
        {
            double sliderValue = SliderValue;
            int timeValue = (int)(((float)sliderValue / 1000) * Duration);
            Position = new TimeSpan(0, 0, timeValue);
            MouseDown = false;
        }

        public void Play(string path)
        {
            if (path != null)
            {
                Open(new Uri(path));
                CurrentTrackPath = path;
                IsOpened = true;
                base.Play();
                Timer.Start();
            }
        }

        public void Resume()
        {
            base.Play();
            IsOpened = true;
            IsPaused = false;
            Timer.IsEnabled = true;
        }

        public new void Pause()
        {
            base.Pause();
            IsPaused = true;
            Timer.IsEnabled = false;
        }

        public void NewTrack(string path)
        {
            Open(new Uri(path));
            CurrentTrackPath = path;
            IsOpened = true;
            Timer.Start();

            if (!IsPaused)
            {
                base.Play();
            }

        }

    }
}
