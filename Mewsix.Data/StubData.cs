using Mewsix.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Mewsix.Data
{
    //This class is in charge of giving Tracks for the ViewModel from the PocoTracks.
    //This class contains the raw data and it's the one who is in charge of adding, uptading and deleting information.
    public class StubData : IDataManager
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

        public StubData()
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

        public void Update(Track track)
        {
            List<Track> list = Tracks as List<Track>;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(track))
                {
                    list[i] = track;
                }
            }
            Save(list);
        }

        public void Save(IEnumerable<Track> toSaveTracks)
        {
            //TODO Save the list of tracks in a file, JSON, DB....
            IEnumerable<PocoTrack> pocoTracksToSave = Track2PocoTracks(toSaveTracks);
            //Convert this variable to JSON and save it into a JSON file.
            string json = JsonConvert.SerializeObject(pocoTracksToSave.ToArray(), Formatting.Indented);
            //Make persistant in a file with poco data
            File.WriteAllText(@"c:\Mewsix\Data.json", json);
            //Update for the current session
            Tracks = toSaveTracks;
        }

        public void Read()
        {
            ////TODO Read the list of tracks from a file, JSON, DB....
            //_Tracks = new List<PocoTrack> {
            //        new PocoTrack{Title = "Creep", Artists = "Radiohead", AlbumUri = "https://images.genius.com/dc6a81658957cf95dc7a5834b6321b7a.300x300x1.jpg" },
            //        new PocoTrack{Title = "Young Stuff", Artists = "Snarky Puppy", AlbumUri = "https://s3.amazonaws.com/bit-photos/large/6303622.jpeg" },
            //        new PocoTrack{Title = "Like A Stone", Artists = "Audioslave", AlbumUri = "https://images-na.ssl-images-amazon.com/images/I/81SPG6dHDXL._SL1500_.jpg" }
            //};

            if (File.Exists(@"c:\Mewsix\Data.json"))
            {
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


            //Open the JSON file, parse the poco tracks and assign them to _Tracks;
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
                pocoTracksToReturn.Add(new PocoTrack { Album = t.Album, Artists = t.Artists, Lyrics = t.Lyrics, AlbumUri = t.AlbumUri.ToString(), Title = t.Title, TrackPath = t.Path, Year = t.Year });
            }

            return pocoTracksToReturn;
        }
    }
}
