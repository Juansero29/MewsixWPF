using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Mewsix.Helpers
{
    public class MewsixPlayer : MediaPlayer
    {
        public bool IsOpened { get; private set; }
        public bool IsPaused { get; private set; }

        public string CurrentTrackPath { get; private set; }

        public MewsixPlayer()
        {
            MediaEnded += MewixPlayer_MediaEnded;
        }

        private void MewixPlayer_MediaEnded(object sender, EventArgs e)
        {
            IsOpened = false;
            Stop();
        }

        public void Play(string path)
        {
            Open(new Uri(path));
            Play();
            CurrentTrackPath = path;
            IsOpened = true;
        }

        public void Resume()
        {
            Play();
            IsOpened = true;
            IsPaused = false;
        }

        public new void Pause()
        {
            base.Pause();
            IsPaused = true;
        }
    }
}
