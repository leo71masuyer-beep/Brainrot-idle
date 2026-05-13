using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Brainrot_idle.view.GameCombat;


namespace Brainrot_idle.view
{
    /// <summary>
    /// Logique d'interaction pour MiniGamesFrame.xaml
    /// </summary>
    public partial class MiniGamesFrame : Page
    {
        public MiniGamesFrame()
        {
            InitializeComponent();
        }
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
        private void Snake_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Snake());
        }

        private void Combat_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuPrincipalCombat());
        }
    }
}
