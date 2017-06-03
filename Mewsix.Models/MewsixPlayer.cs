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
    public class MewsixPlayer : MediaPlayer, INotifyPropertyChanged
    {
        public bool IsOpened { get; private set; }
        public bool IsPaused { get; private set; }

        public string CurrentTrackPath { get; private set; }

        public DispatcherTimer Timer { get; private set; } = new DispatcherTimer();
        private Slider TimeSlider { get; set; }
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


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



        public MewsixPlayer(Slider timeSlider)
        {
            TotalTime = "0:00";
            CurrentTime = "0:00";
            MediaEnded += MewixPlayer_MediaEnded;
            MediaOpened += MewsixPlayer_MediaOpened;
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
            TimeSlider = timeSlider;
            TimeSlider.PreviewMouseUp += TimeSlider_PreviewMouseUp;
            TimeSlider.PreviewMouseDown += TimeSlider_PreviewMouseDown;
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
                TimeSlider.Value = ((float)Position.TotalSeconds / Duration) * 1000;
                //Debug.WriteLine(TimeSlider.Value);
            }
        }

        // MouseDown Allows us to let the slider progress only if the user hasn't clicked on it
        private void TimeSlider_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MouseDown = true;
        }

        private void TimeSlider_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            double sliderValue = TimeSlider.Value;
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
