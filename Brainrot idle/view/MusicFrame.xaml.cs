using Brainrot_idle.Ressources;
using System.Windows;
using System.Windows.Controls;

namespace Brainrot_idle.view
{
    public partial class MusicFrame : Page
    {
        public MusicFrame()
        {
            InitializeComponent();

            // On se branche au système musical global qui tourne déjà en fond
            DataContext = MainWindow.GlobalMusicSystem;
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlobalMusicSystem.SkipPrevious();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlobalMusicSystem.SkipNext();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage());
        }
    }
}