using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mewsix.Models
{
    public class TracksPathList<String> : List<String>
    {
     
        public event EventHandler<TrackPathEventArgs<String>> OnAdd;

        public new void Add(String item)
        {
            OnAdd?.Invoke(this, new TrackPathEventArgs<String>(item));
            base.Add(item);
        }

    }


   
}
