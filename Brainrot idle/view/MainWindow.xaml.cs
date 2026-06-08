using Brainrot_idle.Ressources;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace Brainrot_idle.view
{
    public partial class MainWindow : Window
    {
        public static MediaPlayer player = new MediaPlayer();
        public static double CurrentVolume = 1.0;
        private static MainWindow? instance;
        private readonly Random random = new();
        private List<string> toutesLesMusiques = new();
        private string? musiqueActuelle;
        private readonly Stack<string> historiqueMusiques = new();
        public MainWindow()
        {
            InitializeComponent();

            instance = this;

            ChargerMusiques();

            player.MediaEnded += Player_MediaEnded;

            PasserMusiqueSuivante();

            MainFrame.Navigate(new HomePage());
        }

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