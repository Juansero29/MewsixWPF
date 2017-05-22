using Mewsix.Models;
using Mewsix.ViewModels;
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
    public partial class MainWindow : Window
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
    }
}
