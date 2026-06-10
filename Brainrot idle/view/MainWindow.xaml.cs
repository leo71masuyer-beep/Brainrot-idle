using System;
using System.Windows;
using System.Windows.Media;
using Brainrot_idle.view;
using Brainrot_idle.Ressources.systememusic;

namespace Brainrot_idle
{
    public partial class MainWindow : Window
    {
        // ==========================================
        // PROPRIÉTÉS ET GESTION DE LA MUSIQUE
        // ==========================================

        // Rendue statique pour que MusicFrame puisse y accéder directement
        public static MusicViewModel GlobalMusicSystem { get; set; }

        public static MediaPlayer player = new MediaPlayer();
        public static double CurrentVolume = 1.0;

        // ==========================================
        // PROPRIÉTÉS ET GESTION DU TUTORIEL
        // ==========================================
        public bool IsTutoActive { get; set; } = true; // Définir sur false pour désactiver complètement le tuto
        private int currentTutoStep = 1;

        // ==========================================
        // CONSTRUCTEUR PRINCIPAL
        // ==========================================
        public MainWindow()
        {
            InitializeComponent();

            // ==========================================
            // CONFIGURATION PLEIN ÉCRAN
            // ==========================================
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;

            // 1. Initialisation du volume global
            player.Volume = CurrentVolume;

            // 2. Initialisation du système de musique (la musique se lance ici)
            GlobalMusicSystem = new MusicViewModel();

            // 3. Navigation initiale vers l'écran d'accueil
            MainFrame.Navigate(new HomePage());

            // 4. Si le tutoriel est activé, on déclenche la première étape au chargement
            if (IsTutoActive)
            {
                DeclencherEtapeTuto(1);
            }
        }

        // ==========================================
        // MÉTHODES DU TUTORIEL
        // ==========================================

        /// <summary>
        /// Configure et affiche la fenêtre de tutoriel selon l'étape demandée.
        /// </summary>
        public void DeclencherEtapeTuto(int etape)
        {
            if (!IsTutoActive) return;

            currentTutoStep = etape;

            switch (etape)
            {
                case 1:
                    TutoTitleText.Text = "BIENVENUE (1/4)";
                    TutoContentText.Text = "Voici ton espace de farm ! Clique sur le gros bouton central pour générer tes premiers points d'Aura 😈.";
                    TutoButton.Content = "C'est parti !";
                    TutoPopup.Visibility = Visibility.Visible;
                    break;

                case 2:
                    TutoTitleText.Text = "AMÉLIORATIONS (2/4)";
                    TutoContentText.Text = "Bien joué ! Utilise ton Aura dans le panneau de droite pour acheter des améliorations (comme le Bananini 🍌) et générer de l'Aura automatiquement par seconde !";
                    TutoButton.Content = "Suivant";
                    TutoPopup.Visibility = Visibility.Visible;
                    break;

                case 3:
                    TutoTitleText.Text = "MINI-JEUX (3/4)";
                    TutoContentText.Text = "Clique sur le bouton 'Mini jeux' en bas pour jouer à des mini-jeux et augmente tes scores pour gagner des multiplicateurs et augmenter tes points par seconde !";
                    TutoButton.Content = "Suivant";
                    TutoPopup.Visibility = Visibility.Visible;
                    break;

                case 4:
                    TutoTitleText.Text = "BUT (4/4)";
                    TutoContentText.Text = "Essaie d'obtenir le plus de point possible pour aider Tung Tung Tung Sahur et ses amis";
                    TutoButton.Content = "Fin du tuto (Let's go)";
                    TutoPopup.Visibility = Visibility.Visible;
                    break;

                default:
                    // Si on dépasse les étapes prévues, on ferme l'affichage
                    TutoPopup.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        /// <summary>
        /// Événement de clic sur le bouton de la pop-up de tutoriel pour passer à la suite.
        /// </summary>
        private void TutoButton_Click(object sender, RoutedEventArgs e)
        {
            currentTutoStep++;

            // CORRECTION : Changé 3 par 4 pour autoriser l'affichage de l'étape 4
            if (currentTutoStep <= 4)
            {
                DeclencherEtapeTuto(currentTutoStep);
            }
            else
            {
                // Fin du tutoriel, on cache la fenêtre
                TutoPopup.Visibility = Visibility.Collapsed;
            }
        }
    }
}