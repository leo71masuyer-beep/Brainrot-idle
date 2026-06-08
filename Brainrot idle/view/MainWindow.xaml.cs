using Brainrot_idle.view;
using System.Windows;
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Brainrot_idle.Ressources;
using Brainrot_idle.view;

namespace Brainrot_idle
{
    public partial class MainWindow : Window
    {
        // ==========================================
        // PROPRIÉTÉS ET GESTION DE LA MUSIQUE
        // ==========================================
        public MusicViewModel GlobalMusicViewModel { get; set; }
        public static MediaPlayer player = new MediaPlayer();
        public static double CurrentVolume = 1.0;
        private static MainWindow? instance;
        private readonly Random random = new();
        private List<string> toutesLesMusiques = new();
        private string? musiqueActuelle;
        private readonly Stack<string> historiqueMusiques = new();
        public static event Action? MusicChanged;

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

            // Initialisation du système de musique
            GlobalMusicViewModel = new MusicViewModel();
            instance = this;

            // Navigation initiale vers l'écran d'accueil
            MainFrame.Navigate(new HomePage());

            // Si le tutoriel est activé, on déclenche la première étape au chargement
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
                    TutoTitleText.Text = "BIENVENUE (1/3)";
                    TutoContentText.Text = "Voici ton espace de farm ! Clique sur le gros bouton central pour générer tes premiers points d'Aura 😈.";
                    TutoButton.Content = "C'est parti !";
                    TutoPopup.Visibility = Visibility.Visible;
                    break;

                case 2:
                    TutoTitleText.Text = "AMÉLIORATIONS (2/3)";
                    TutoContentText.Text = "Bien joué ! Utilise ton Aura dans le panneau de droite pour acheter des améliorations (comme le Bananini 🍌) et générer de l'Aura automatiquement par seconde !";
                    TutoButton.Content = "Suivant";
                    TutoPopup.Visibility = Visibility.Visible;
                    break;

                case 3:
                    TutoTitleText.Text = "MINI-JEUX (3/3)";
                    TutoContentText.Text = "Clique sur le bouton 'Mini jeux' en bas pour jouer à des mini-jeux et augmente tes scores pour gagner des multiplicateurs et augmenter tes points par seconde !";
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

            if (currentTutoStep <= 3)
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