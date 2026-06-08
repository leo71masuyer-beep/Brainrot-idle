using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Brainrot_idle.view.GameCombat;
using Brainrot_idle.Ressources;


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
            if (GameState.IsSnakeDebloque)
            {
                NavigationService.Navigate(new Snake());
            }
            else
            {
                MessageBox.Show("Ce jeu est verrouillé ! Débloque-le dans l'arbre des talents.", "Jeu Verrouillé", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Combat_Button_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.IsCombatDebloque)
            {
                NavigationService.Navigate(new MenuPrincipalCombat());
            }
            else
            {
                MessageBox.Show("Ce jeu est verrouillé ! Débloque-le dans l'arbre des talents.", "Jeu Verrouillé", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Morpion_Button_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.IsMorpionDebloque)
            {
                NavigationService.Navigate(new MorpionFrame());
            }
            else
            {
                MessageBox.Show("Ce jeu est verrouillé ! Débloque-le dans l'arbre des talents.", "Jeu Verrouillé", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
