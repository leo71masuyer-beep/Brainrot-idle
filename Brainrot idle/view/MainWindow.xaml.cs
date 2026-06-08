using Brainrot_idle.Ressources;
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
        public MusicViewModel GlobalMusicViewModel { get; set; }
        public static MediaPlayer player = new MediaPlayer();
        public static double CurrentVolume = 1.0; 

        public MainWindow()
        {
            InitializeComponent();
            GlobalMusicViewModel = new MusicViewModel();

            MainFrame.Navigate(new HomePage());

            // Si le tutoriel est activé, on déclenche la première étape au chargement
            if (IsTutoActive)
            {
                DeclencherEtapeTuto(1);
            }
        }

        // méthode du tuto

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
                    TutoContentText.Text = "Clique sur le bouton 'Mini jeux' en bas pour jouer à des mini-jeux et augmente tes scores pour gagner des multiplicateur et augmenter tes points par secondes !";
                    TutoButton.Content = "Fin du tuto (Let's go)";
                    TutoPopup.Visibility = Visibility.Visible;
                    break;

                default:
                    // Si on dépasse les étapes prévues, on ferme l'affichage
                    TutoPopup.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}