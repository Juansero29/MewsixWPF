using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Mewsix.Helpers
{
    public class MewsixPlayer : MediaPlayer
    {
        public bool IsOpened { get; private set; }
        public bool IsPaused { get; private set; }

        public string CurrentTrackPath { get; private set; }

        public DispatcherTimer Timer { get; private set; } = new DispatcherTimer();
        private TextBlock CurrentTimeTBox { get; set; }
        private TextBlock TotalTimeTBox { get; set; }
        private Slider TimeSlider { get; set; }
        public int Duration { get; private set; }

        public MewsixPlayer(TextBlock currentTimeTBlock, TextBlock totalTimeTBlock, Slider timeSlider)
        {
            MediaEnded += MewixPlayer_MediaEnded;
            MediaOpened += MewsixPlayer_MediaOpened;
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;

            CurrentTimeTBox = currentTimeTBlock;
            TotalTimeTBox = totalTimeTBlock;
            TimeSlider = timeSlider;
            TimeSlider.PreviewMouseUp += TimeSlider_PreviewMouseUp;
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
            TotalTimeTBox.Text = NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            Duration = (int)NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentTimeTBox.Text = String.Format($"{Position.ToString(@"m\:ss")}");
            if (Duration != 0 && !TimeSlider.IsMouseOver)
            {
                TimeSlider.Value = ((float)Position.Seconds / Duration) * 1000;
                Debug.WriteLine(TimeSlider.Value);
            }
        }


        private void TimeSlider_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            double sliderValue = TimeSlider.Value;
            int timeValue = (int)((float)sliderValue / 1000) * Duration;
            Position = new TimeSpan(0, 0, timeValue);
        }

        public void Play(string path)
        {
            Open(new Uri(path));
            CurrentTrackPath = path;
            IsOpened = true;
            base.Play();
            Timer.Start();
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
