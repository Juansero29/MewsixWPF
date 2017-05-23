using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mewsix.Models
{
    public class TrackPathEventArgs<String> : System.EventArgs
    {

        public String TrackPath { get; private set; }

        public TrackPathEventArgs(String trackPath)
        {
            TrackPath = trackPath;
        }

    }
}
