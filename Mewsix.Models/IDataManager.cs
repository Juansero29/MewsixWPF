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

        void Add(Track nounours);

        void Remove(Track nounours);

        void Update(Track nounours);

        void Save(IEnumerable<Track> toSaveTracks);

        void Read();


    }
}
