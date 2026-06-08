using System;
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
            // 1. Visuel pour le Snake (Jeu 1)
            if (GameState.IsSnakeDebloque)
            {
                SnakeButton.Opacity = 1.0;
                SnakeLockImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                SnakeButton.Opacity = 0.5;
                SnakeLockImage.Visibility = Visibility.Visible;
            }

            // 2. Visuel pour le Combat (Jeu 2)
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

            // 3. Visuel pour le Morpion (Jeu 3)
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

            // 4. Visuel pour le Jeu 4
            if (GameState.IsJeu4Debloque)
            {
                Jeu4Button.Opacity = 1.0;
                Jeu4LockImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                Jeu4Button.Opacity = 0.5;
                Jeu4LockImage.Visibility = Visibility.Visible;
            }

            // 5. Visuel pour le Jeu 5
            if (GameState.IsJeu5Debloque)
            {
                Jeu5Button.Opacity = 1.0;
                Jeu5LockImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                Jeu5Button.Opacity = 0.5;
                Jeu5LockImage.Visibility = Visibility.Visible;
            }

            // 6. Visuel pour le Jeu 6
            if (GameState.IsJeu6Debloque)
            {
                Jeu6Button.Opacity = 1.0;
                Jeu6LockImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                Jeu6Button.Opacity = 0.5;
                Jeu6LockImage.Visibility = Visibility.Visible;
            }

            // 7. Visuel pour le Jeu 7
            if (GameState.IsJeu7Debloque)
            {
                Jeu7Button.Opacity = 1.0;
                Jeu7LockImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                Jeu7Button.Opacity = 0.5;
                Jeu7LockImage.Visibility = Visibility.Visible;
            }

            // 8. Visuel pour le Jeu 8
            if (GameState.IsJeu8Debloque)
            {
                Jeu8Button.Opacity = 1.0;
                Jeu8LockImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                Jeu8Button.Opacity = 0.5;
                Jeu8LockImage.Visibility = Visibility.Visible;
            }
        }

        // ==========================================
        // ÉVÉNEMENTS DE CLIC DES MINI-JEUX
        // ==========================================

        private void Snake_Button_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.IsSnakeDebloque)
            {
                NavigationService.Navigate(new Snake());
            }
            else
            {
                AfficherMessageVerrouille();
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
                AfficherMessageVerrouille();
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
                AfficherMessageVerrouille();
            }
        }

        private void Jeu4_Button_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.IsJeu4Debloque)
            {
                // Remplacer 'PageJeu4' par le nom de ta classe de jeu réelle quand elle sera créée
                // NavigationService.Navigate(new PageJeu4()); 
            }
            else
            {
                AfficherMessageVerrouille();
            }
        }

        private void Jeu5_Button_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.IsJeu5Debloque)
            {
                // NavigationService.Navigate(new PageJeu5());
            }
            else
            {
                AfficherMessageVerrouille();
            }
        }

        private void Jeu6_Button_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.IsJeu6Debloque)
            {
                // NavigationService.Navigate(new PageJeu6());
            }
            else
            {
                AfficherMessageVerrouille();
            }
        }

        private void Jeu7_Button_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.IsJeu7Debloque)
            {
                // NavigationService.Navigate(new PageJeu7());
            }
            else
            {
                AfficherMessageVerrouille();
            }
        }

        private void Jeu8_Button_Click(object sender, RoutedEventArgs e)
        {
            if (GameState.IsJeu8Debloque)
            {
                // NavigationService.Navigate(new PageJeu8());
            }
            else
            {
                AfficherMessageVerrouille();
            }
        }

        /// <summary>
        /// Alerte commune pour éviter la duplication de code.
        /// </summary>
        private void AfficherMessageVerrouille()
        {
            MessageBox.Show("Ce jeu est verrouillé ! Débloque-le dans l'arbre des talents.", "Jeu Verrouillé", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}