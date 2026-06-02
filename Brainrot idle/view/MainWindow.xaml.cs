using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Brainrot_idle.view
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MediaPlayer player = new MediaPlayer();
        private readonly Random random = new Random();
        private List<string> musiques = new();
        private int indexActuel = 0;

        private void PlayMusic()
        {
            try
            {
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ressources/music.mp3");

                if (System.IO.File.Exists(audioPath))
                {
                    player.Stop();
                    player.Open(new Uri(audioPath, UriKind.Absolute));
                    player.Volume = 1.0;

                    player.MediaEnded += (s, e) => { player.Position = TimeSpan.Zero; player.Play(); };
                    player.Play();
                }
                else
                {
                    MessageBox.Show("Le fichier n'est toujours pas vu à cet endroit : " + audioPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur critique : " + ex.Message);
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            ChargerMusiques();
            player.MediaEnded += Player_MediaEnded;

            JouerMusiqueCourante();

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

            musiques = Directory
                .GetFiles(dossierMusiques, "*.mp3")
                .OrderBy(x => random.Next())
                .ToList();
        }
        private void JouerMusiqueCourante()
        {
            if (musiques.Count == 0)
                return;

            player.Stop();
            player.Open(new Uri(musiques[indexActuel], UriKind.Absolute));
            player.Volume = 1.0;
            player.Play();
        }
        private void Player_MediaEnded(object? sender, EventArgs e)
        {
            indexActuel++;

            // Toutes les musiques ont été jouées
            if (indexActuel >= musiques.Count)
            {
                // Nouveau mélange
                musiques = musiques
                    .OrderBy(x => random.Next())
                    .ToList();

                indexActuel = 0;
            }

            JouerMusiqueCourante();
        }
    }
}