using Mewsix.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mewsix.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Song> Songs { get; set; }

        public MainWindowViewModel()
        {
            Songs = new ObservableCollection<Song> {
                new Song("Creep", "Radiohead", "https://images.genius.com/dc6a81658957cf95dc7a5834b6321b7a.300x300x1.jpg"),
                new Song("Young Stuff", "Snarky Puppy", "https://s3.amazonaws.com/bit-photos/large/6303622.jpeg"),
                new Song("Like A Stone", "Audioslave", "https://images-na.ssl-images-amazon.com/images/I/81SPG6dHDXL._SL1500_.jpg"),
                new Song("Like A Stone", "Audioslave", "https://images-na.ssl-images-amazon.com/images/I/81SPG6dHDXL._SL1500_.jpg"),
                new Song("Like A Stone", "Audioslave", "https://images-na.ssl-images-amazon.com/images/I/81SPG6dHDXL._SL1500_.jpg")};
        }

        public void OnRaisePropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
