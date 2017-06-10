using System.Collections.Generic;

namespace Mewsix.Models
{
    public interface IDataManager
    {
        IEnumerable<Track> Tracks { get; }

        void Add(Track t);

        void Remove(Track t);

        void Update(Track t);

        void Save(IEnumerable<Track> toSaveTracks);

        void Read();


    }
}
