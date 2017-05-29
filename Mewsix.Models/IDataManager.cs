using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mewsix.Models
{
    public interface IDataManager
    {
        IEnumerable<Track> Tracks { get; }

        void Add(Track t);

        void Remove(Track t);

        void Update(Track updatedTrack, Track currentTrack);

        void Save(IEnumerable<Track> toSaveTracks);

        void Read();


    }
}
