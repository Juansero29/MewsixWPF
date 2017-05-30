using MahApps.Metro.Controls;
using Mewsix.Helpers;
using Mewsix.Models;
using Mewsix.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace Mewsix
{
    public partial class MainWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(TextBlock_Current_Time, TextBlock_Total_Time, Slider_Time);
            Closing += (DataContext as MainWindowViewModel).OnWindowClosing;
        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO Put the tab's names into a constant file? 
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
                Filter = "Music files|*.mp3;*.flac",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string trackPath in openFileDialog.FileNames)
                {
                    (DataContext as MainWindowViewModel).AddTrack(trackPath);
                }
            }
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var trackPath in files)
                {
                    if (Path.GetExtension(trackPath) == ".mp3")
                        (DataContext as MainWindowViewModel).AddTrack(trackPath);
                }
            }
        }

        private void Button_Play_Pause_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel).PlaySelectedTrack();
        }


        private void Button_Previous_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel).PlayPrevious();
        }


        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel).PlayNext();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //TODO This work shouldn't be done by the view.
                Track currentTrack = (DataContext as MainWindowViewModel).SelectedTrack;
                currentTrack.UpdateImage();
                TagLib.File file = TagLib.File.Create(currentTrack.Path);
                TagLib.Tag tag = file.Tag;

                tag.Title = currentTrack.Title;
                tag.Album = currentTrack.Album;
                tag.Year = Convert.ToUInt32(currentTrack.Year);

                try
                {
                    file.Save();
                    (DataContext as MainWindowViewModel).Update(currentTrack);
                    MessageBox.Show("Track information updated !");

                }
                catch (UnauthorizedAccessException exception)
                {
                    MessageBox.Show("Couldn't update track info. Access denied.");
                    Debug.WriteLine(exception.ToString());
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Couldn't update track info. An unknown exception occured");
                    Debug.WriteLine(exception.ToString());
                }

            }

        }

        
        private void PopupBox_OnOpened(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Just making sure the popup has opened.");
        }

        private void PopupBox_OnClosed(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Just making sure the popup has closed.");
        }

        private void Button_Add_Folder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This feature will be added soon!");
            Debug.Print("A dialog should open!");
        }

        private void ListView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            Mouse.SetCursor(Cursors.Hand);
            e.Handled = true;
        }

        private void MenuItemRemove_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel).Remove((DataContext as MainWindowViewModel).SelectedTrack);
        }
    }
}
