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

        public string Title { get; private set; }
        public string[] Artists { get; private set; }
        public string Album { get; private set; }
        public string Year { get; private set; }
        public string Comment { get; private set; }
        public string Genre { get; private set; }
        public string Lyrics { get; private set; }

        public MusicID3Tag(string trackPath)
        {
            // https://stackoverflow.com/questions/5782642/wpf-c-get-informations-from-audio-file (characters display problem)
            // https://stackoverflow.com/questions/68283/view-edit-id3-data-for-mp3-files --> use TagLib


            TagLib.Tag tags = TagLib.File.Create(trackPath).Tag;

            Title = tags.Title;
            Artists = tags.Performers;
            Album = tags.Album;
            Year = tags.Year.ToString();
            Comment = tags.Comment;
            Genre = tags.JoinedGenres;
            Lyrics = tags.Lyrics;
            if (!Artists.ToList().Any())
            {
                Artists = new string[] { "Unknown" };
            }
            if (Title == null) Title = "None";
            if (Year == null) Year = "1984";
            if (Album == null) Album = "Unknown";
        }

    }
}
