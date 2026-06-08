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

        /// <summary>
        /// S'exécute automatiquement à chaque fois que la page s'affiche.
        /// Gère l'affichage des chaînes et l'effet de transparence (grisé).
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // 1. Visuel pour le Snake
            if (GameState.IsSnakeDebloque)
            {
                SnakeButton.Opacity = 1.0; // Totalement opaque (normal)
                SnakeLockImage.Visibility = Visibility.Collapsed; // Masque la chaîne
            }
            else
            {
                SnakeButton.Opacity = 0.5; // Effet bouton grisé
                SnakeLockImage.Visibility = Visibility.Visible; // Affiche la chaîne
            }

            // 2. Visuel pour le Combat
            if (GameState.IsCombatDebloque)
            {
                CombatButton.Opacity = 1.0;
                CombatLockImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                CombatButton.Opacity = 0.5;
                CombatLockImage.Visibility = Visibility.Visible;
            }

            // 3. Visuel pour le Morpion
            if (GameState.IsMorpionDebloque)
            {
                MorpionButton.Opacity = 1.0;
                MorpionLockImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                MorpionButton.Opacity = 0.5;
                MorpionLockImage.Visibility = Visibility.Visible;
            }
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