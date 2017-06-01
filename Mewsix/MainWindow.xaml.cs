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
            //TODO Make this a command.
            (DataContext as MainWindowViewModel).AddButton();
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            //TODO Make this a command.
            (DataContext as MainWindowViewModel).OnObjectDroppedOnView(sender, e);
          
        }

        private void Button_Play_Pause_Click(object sender, RoutedEventArgs e)
        {
            //TODO Make this a command.
            (DataContext as MainWindowViewModel).PlaySelectedTrack();
        }


        private void Button_Previous_Click(object sender, RoutedEventArgs e)
        {
            //TODO Make this a command.
            (DataContext as MainWindowViewModel).PlayPrevious();
        }


        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            //TODO Make this a command.
            (DataContext as MainWindowViewModel).PlayNext();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //TODO Make this a command.
            (DataContext as MainWindowViewModel).OnTextBoxKeyDown(sender, e);
        }

        private void Button_Add_Folder_Click(object sender, RoutedEventArgs e)
        {
            //TODO Make this a command.
            (DataContext as MainWindowViewModel).AddFolder();
        }

        private void ListView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            Mouse.SetCursor(Cursors.Wait);
        }

        private void MenuItemRemove_Click(object sender, RoutedEventArgs e)
        {
            //TODO Make this a command.
            (DataContext as MainWindowViewModel).Remove((DataContext as MainWindowViewModel).SelectedTrack);
        }
    }
}
