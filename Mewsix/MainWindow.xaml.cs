using Mewsix.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Mewsix
{
    public partial class MainWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            //TODO Make this a command.
            (DataContext as MainWindowViewModel).OnObjectDroppedOnView(sender, e);          
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //TODO Make this a command.
            (DataContext as MainWindowViewModel).OnTextBoxKeyDown(sender, e);
        }

        private void ListView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            Mouse.SetCursor(Cursors.Wait);
        }
    }
}
