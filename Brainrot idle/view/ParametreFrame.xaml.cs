using System.Windows;
using System.Windows.Controls;

namespace Brainrot_idle.view
{
    public partial class ParametreFrame : Page
    {
        public ParametreFrame()
        {
            InitializeComponent();
            // On s'assure que le slider affiche le volume actuel
            VolumeSlider.Value = MainWindow.CurrentVolume * 100;
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage());
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Mise à jour de la variable globale
            MainWindow.CurrentVolume = e.NewValue / 100.0;

            // Application immédiate au lecteur
            if (MainWindow.player != null)
            {
                MainWindow.player.Volume = MainWindow.CurrentVolume;
            }
        }
    }
}