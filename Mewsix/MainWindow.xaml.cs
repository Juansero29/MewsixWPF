using MahApps.Metro.Controls;
using Mewsix.Helpers;
using Mewsix.Models;
using Mewsix.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mewsix
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO mettre les noms des tabs dans un fichier de constantes? 
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Header as string;

            switch (tabItem)
            {
                case "Lyrics":
                    break;

                case "Info":
                    break;

                default:
                    return;
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ListView lv = e.OriginalSource as ListView;
            // ListViewItem lvi = lv.SelectedItem as ListViewItem;

            // if ((DataContext as MainWindowViewModel).SelectedTrack != null)
            //     MessageBox.Show((DataContext as MainWindowViewModel).SelectedTrack.ToString());
        }


        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    (DataContext as MainWindowViewModel).AddTrack(filename);
                }
            }
 
        }

        private void Button_Play_Click(object sender, RoutedEventArgs e)
        {
            //TODO Play the selected track on the ViewModel
        }

        private void Button_Previous_Click(object sender, RoutedEventArgs e)
        {
            //TODO Go to the previous track on the ViewModel
        }


        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Track currentTrack = (DataContext as MainWindowViewModel).SelectedTrack;
                TagLib.File file = TagLib.File.Create(currentTrack.TrackPath);
                TagLib.Tag tag = file.Tag;

                tag.Title = currentTrack.Title;
                tag.Album = currentTrack.Album;
                tag.Year = Convert.ToUInt32(currentTrack.Year);

                try
                {
                    file.Save();
                }
                catch (UnauthorizedAccessException exception)
                {
                    MessageBox.Show("Couldn't update track info. Access denied.");
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Couldn't update track info. An unknown exception occured");
                }
                MessageBox.Show("Track information updated !");

            }

        }

    }
}
