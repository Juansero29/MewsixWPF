using Mewsix.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace Mewsix.Data
{
    //This class is in charge of giving Tracks for the ViewModel from the PocoTracks.
    //This class contains the raw data and it's the one who is in charge of adding, updating and deleting information.
    public class Data : IDataManager
    {
        private IEnumerable<PocoTrack> _Tracks;
        public IEnumerable<Track> Tracks
        {
            get
            {
                return PocoTrack2Tracks(_Tracks);
            }
            private set
            {
                _Tracks = Track2PocoTracks(value);
            }
        }

        public Data()
        {
            Read();
        }

        public void Add(Track track)
        {
            if(Tracks == null)
            {
                Tracks = new List<Track>();
            }
            List<Track> list = Tracks as List<Track>;
            list.Add(track);
            Save(list);
        }

        public void Remove(Track track)
        {
            List<Track> list = Tracks as List<Track>;
            list.Remove(track);
            Save(list);
        }

        public void Update(Track t)
        {
            List<Track> list = Tracks as List<Track>;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Equals(t))
                {
                    list.RemoveAt(i);
                    list.Insert(i, t);
                }
            }
            Save(list);
        }

        public void Save(IEnumerable<Track> toSaveTracks)
        {
            if (toSaveTracks != null)
            {
                IEnumerable<PocoTrack> pocoTracksToSave = Track2PocoTracks(toSaveTracks);
                //Convert this variable to JSON and save it into a JSON file.
                string json = JsonConvert.SerializeObject(pocoTracksToSave.ToArray(), Formatting.Indented);
                //Make persistant in a file with poco data
                File.WriteAllText(@"c:\Mewsix\Data.json", json);
                //Update for the current session
                Tracks = toSaveTracks;
            }
        }

        public void Read()
        {
            if (File.Exists(@"c:\Mewsix\Data.json"))
            {

                //Garbage collector: He doesn't know how to clean up some of the classes because he doesn't know when we're going to stop using it, 
                // This kind of classes implement the interface IDisposable to that we can clean it up when we find is rights. 
                //The block using calls Dispose() when the final bracket is finished.


                //All that we put inside the parentheses on using must implement using and then will be disposed when the final bracket is reached.


                //There's always a case in which a resource is used by multiple objets, in that case using doesn't work, we must come back to Dispose();

                using (StreamReader r = new StreamReader(@"c:\Mewsix\Data.json"))
                {
                    string json = r.ReadToEnd();
                    List<PocoTrack> readPocoTracks = JsonConvert.DeserializeObject<List<PocoTrack>>(json);
                    _Tracks = readPocoTracks;
                }
            }
            else
            {
                Directory.CreateDirectory(@"c:\Mewsix\");
                File.Create(@"c:\Mewsix\Data.json");
            }
        }

        private IEnumerable<Track> PocoTrack2Tracks(IEnumerable<PocoTrack> pocotracksToConvert)
        {
            if (pocotracksToConvert != null)
            {
                List<Track> tracksToReturn = new List<Track>();
                foreach (PocoTrack pt in pocotracksToConvert)
                {
                    tracksToReturn.Add(new Track(pt));
                }
                return tracksToReturn;
            }

            return null;

        }

        private IEnumerable<PocoTrack> Track2PocoTracks(IEnumerable<Track> tracksToConvert)
        {
            List<PocoTrack> pocoTracksToReturn = new List<PocoTrack>();
            foreach (Track t in tracksToConvert)
            {
                pocoTracksToReturn.Add(new PocoTrack { Album = t.Album, Artists = t.Artists, Comment = t.Comment, Genre=t.Genre, Lyrics = t.Lyrics, AlbumUri = t.AlbumUri.ToString(), Summary = t.Summary, Title = t.Title, TrackPath = t.Path, Year = t.Year, ID = t.ID });
            }

            return pocoTracksToReturn;
        }
    }
}
