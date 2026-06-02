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

            ChargerListeMusiques();
        }

        private void ChargerListeMusiques()
        {
            MusicListPanel.Children.Clear();

            foreach (string musique in GameState.MusiquesDisponibles)
            {
                CheckBox cb = new CheckBox();

                cb.Content = musique;
                cb.Margin = new Thickness(5);
                cb.IsChecked = GameState.MusiquesActives.Contains(musique);

                cb.Checked += Music_Checked;
                cb.Unchecked += Music_Unchecked;

                MusicListPanel.Children.Add(cb);
            }
        }

        private void Music_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            GameState.MusiquesActives.Add(cb.Content.ToString());
        }

        private void Music_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            GameState.MusiquesActives.Remove(cb.Content.ToString());
        }

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.SkipCurrentMusic();
        }
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.PreviousMusic();
        }
        private void Return_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}