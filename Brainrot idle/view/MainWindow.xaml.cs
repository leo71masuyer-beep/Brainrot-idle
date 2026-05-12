using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Brainrot_idle.view
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MediaPlayer player = new MediaPlayer();

        private void PlayMusic()
        {
            try
            {
                string audioPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ressources/music.mp3");

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
            PlayMusic();
            MainFrame.Navigate(new HomePage());
        }
    }
}