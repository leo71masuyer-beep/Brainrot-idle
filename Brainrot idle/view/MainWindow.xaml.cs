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
        // gestion musique
        public static MediaPlayer player = new MediaPlayer();
        public static double CurrentVolume = 1.0;
        private static MainWindow? instance;
        private readonly Random random = new();
        private List<string> toutesLesMusiques = new();
        private string? musiqueActuelle;
        private readonly Stack<string> historiqueMusiques = new();

        // gestion tuto

        public bool IsTutoActive { get; set; } = true; // Définir sur false pour désactiver complètement le tuto
        private int currentTutoStep = 1;

        public MainWindow()
        {
            InitializeComponent();

            instance = this;

            // Initialisation de la musique
            ChargerMusiques();
            player.MediaEnded += Player_MediaEnded;
            PasserMusiqueSuivante();

            // Navigation vers la page d'accueil
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

        /// <summary>
        /// Événement de clic sur le bouton de la pop-up de tutoriel.
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

        // méthode musique

        private void ChargerMusiques()
        {
            string dossierMusiques = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Ressources",
                "music");

            if (!Directory.Exists(dossierMusiques))
            {
                MessageBox.Show($"Dossier introuvable : {dossierMusiques}");
                return;
            }

            toutesLesMusiques = Directory
                .GetFiles(dossierMusiques, "*.mp3")
                .ToList();

            // Initialisation des listes du GameState
            GameState.MusiquesDisponibles.Clear();
            GameState.MusiquesActives.Clear();

            foreach (string musique in toutesLesMusiques)
            {
                string nomFichier = Path.GetFileName(musique);

                GameState.MusiquesDisponibles.Add(nomFichier);
                GameState.MusiquesActives.Add(nomFichier);
            }
        }

        private void Player_MediaEnded(object? sender, EventArgs e)
        {
            PasserMusiqueSuivante();
        }

        public static void SkipCurrentMusic()
        {
            instance?.PasserMusiqueSuivante();
        }

        private void PasserMusiqueSuivante()
        {
            if (toutesLesMusiques.Count == 0)
                return;

            var musiquesDisponibles = toutesLesMusiques
                .Where(f =>
                    GameState.MusiquesActives.Contains(
                        Path.GetFileName(f)))
                .ToList();

            if (musiquesDisponibles.Count == 0)
            {
                MessageBox.Show("Aucune musique sélectionnée.");
                return;
            }

            string prochaineMusique;

            do
            {
                prochaineMusique =
                    musiquesDisponibles[random.Next(musiquesDisponibles.Count)];
            }
            while (
                musiquesDisponibles.Count > 1 &&
                prochaineMusique == musiqueActuelle
            );

            if (musiqueActuelle != null)
            {
                historiqueMusiques.Push(musiqueActuelle);
            }

            musiqueActuelle = prochaineMusique;

            player.Stop();
            player.Open(new Uri(prochaineMusique, UriKind.Absolute));
            player.Volume = CurrentVolume;
            player.Play();
        }

        public static void PreviousMusic()
        {
            instance?.RevenirMusiquePrecedente();
        }

        private void RevenirMusiquePrecedente()
        {
            if (historiqueMusiques.Count == 0)
                return;

            string precedente = historiqueMusiques.Pop();

            musiqueActuelle = precedente;

            player.Stop();
            player.Open(new Uri(precedente, UriKind.Absolute));
            player.Volume = CurrentVolume;
            player.Play();
        }
    }
}