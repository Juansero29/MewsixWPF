using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mewsix.Helpers
{
    public class MusicID3Tag
    {
        private byte[] _tagID = new byte[3];      //  3
        private byte[] _title = new byte[30];     //  30
        private byte[] _artist = new byte[30];    //  30 
        private byte[] _album = new byte[30];     //  30 
        private byte[] _year = new byte[4];       //  4 
        private byte[] _comment = new byte[30];   //  30 
        private byte[] _genre = new byte[1];      //  1

        public string Title { get; private set; }
        public string Artist { get; private set; }
        public string Album { get; private set; }
        public string Year { get; private set; }
        public string Comment { get; private set; }
        public string Genre { get; private set; }

        public MusicID3Tag(string trackPath)
        {
            // https://stackoverflow.com/questions/5782642/wpf-c-get-informations-from-audio-file

            using (FileStream fs = File.OpenRead(trackPath))
            {
                if (fs.Length >= 128)
                {
                    fs.Seek(-128, SeekOrigin.End);
                    fs.Read(_tagID, 0, _tagID.Length);
                    fs.Read(_title, 0, _title.Length);
                    fs.Read(_artist, 0, _artist.Length);
                    fs.Read(_album, 0, _album.Length);
                    fs.Read(_year, 0, _year.Length);
                    fs.Read(_comment, 0, _comment.Length);
                    fs.Read(_genre, 0, _genre.Length);
                    string theTAGID = Encoding.Default.GetString(_tagID);

                    if (theTAGID.Equals("TAG"))
                    {
                        Title = Encoding.Default.GetString(_title);
                        Artist = Encoding.Default.GetString(_artist);
                        Album = Encoding.Default.GetString(_album);
                        Year = Encoding.Default.GetString(_year);
                        Comment = Encoding.Default.GetString(_comment);
                        Genre = Encoding.Default.GetString(_genre);

                        Title = Title.Replace("\0", "");
                        Artist = Artist.Replace("\0", "");
                    } else
                    {
                        Uri trackUri = new Uri(trackPath);
                        MessageBox.Show("Incorrect tag for track : "+ trackUri.Segments[trackUri.Segments.Length-1]);
                    }
                }
            }
        }

    }
}
