using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Brainrot_idle.view
{
    public partial class Snake : Page
    {
        private List<Rectangle> corpsDuSerpent = new List<Rectangle>();
        private const int TailleCase = 20;
        private DispatcherTimer timerJeu;
        private int directionX = 1;
        private int directionY = 0;
        private Ellipse pomme;
        private Random random = new Random();
        private bool directionChangeeCeTour = false;

        // Nouvelle variable pour suivre le score
        private int score = 0;

        public Snake()
        {
            InitializeComponent();

            // NOUVEAU : On force le timer à s'exécuter avec la priorité du rendu visuel
            timerJeu = new DispatcherTimer(DispatcherPriority.Render, Dispatcher);

            // 80ms est un excellent compromis pour la fluidité en WPF
            timerJeu.Interval = TimeSpan.FromMilliseconds(80);
            timerJeu.Tick += TimerJeu_Tick;

            Loaded += Snake_Loaded;
            Unloaded += Snake_Unloaded;
        }

        private void Snake_Loaded(object sender, RoutedEventArgs e)
        {
            DemarrerNouvellePartie();
        }
        private void Snake_Unloaded(object sender, RoutedEventArgs e)
        {
            timerJeu.Stop();
            timerJeu.Tick -= TimerJeu_Tick;
        }

        private void DemarrerNouvellePartie()
        {
            // Nettoyage de la zone de jeu pour une nouvelle partie
            timerJeu.Stop();
            ZoneDeJeu.Children.Clear();
            corpsDuSerpent.Clear();

            // Réinitialisation des variables de jeu
            score = 0;
            TexteScore.Text = "Score : " + score;
            directionX = 1;
            directionY = 0;

            // Spawn du serpent (tête + 2 morceaux de corps pour commencer proprement)
            AjouterMorceauSnake(200, 200);

            AjouterPomme();

            // Forcer le focus sur la page pour capturer les touches du clavier
            Focusable = true;
            Focus();
            Keyboard.Focus(this);

            timerJeu.Start();
        }

        private void Return_Button_Click(object sender, RoutedEventArgs e)
        {
            timerJeu.Stop();
            if (score > Brainrot_idle.Ressources.GameState.MeilleurScoreSnake)
            {
                Brainrot_idle.Ressources.GameState.MeilleurScoreSnake = score;
            }
            NavigationService.Navigate(new MiniGamesFrame());
        }

        private void TimerJeu_Tick(object sender, EventArgs e)
        {
            directionChangeeCeTour = false;
            BougerSerpent();
            VerifierCollisionMurs();
            VerifierCollisionPomme();
        }

        private void BougerSerpent()
        {
            // Déplacement du corps (de la queue vers la tête)
            for (int i = corpsDuSerpent.Count - 1; i > 0; i--)
            {
                double xPrecedent = Canvas.GetLeft(corpsDuSerpent[i - 1]);
                double yPrecedent = Canvas.GetTop(corpsDuSerpent[i - 1]);

                Canvas.SetLeft(corpsDuSerpent[i], xPrecedent);
                Canvas.SetTop(corpsDuSerpent[i], yPrecedent);
            }

            // Déplacement de la tête
            Rectangle tete = corpsDuSerpent[0];
            double positionXActuelle = Canvas.GetLeft(tete);
            double positionYActuelle = Canvas.GetTop(tete);

            Canvas.SetLeft(tete, positionXActuelle + (directionX * TailleCase));
            Canvas.SetTop(tete, positionYActuelle + (directionY * TailleCase));
        }

        private void VerifierCollisionMurs()
        {
            Rectangle tete = corpsDuSerpent[0];
            double teteX = Canvas.GetLeft(tete);
            double teteY = Canvas.GetTop(tete);

            // Vérification des limites du Canvas (800 x 600)
            if (teteX < 0 || teteX >= ZoneDeJeu.Width || teteY < 0 || teteY >= ZoneDeJeu.Height)
            {
                GameOver();
            }
        }

        private void VerifierCollisionPomme()
        {
            if (corpsDuSerpent.Count == 0 || pomme == null) return;

            Rectangle tete = corpsDuSerpent[0];
            double teteX = Canvas.GetLeft(tete);
            double teteY = Canvas.GetTop(tete);

            double pommeX = Canvas.GetLeft(pomme);
            double pommeY = Canvas.GetTop(pomme);

            if (teteX == pommeX && teteY == pommeY)
            {
                // Augmentation et mise à jour du score
                score++;
                TexteScore.Text = "Score : " + score;

                // Grandir le serpent
                Rectangle dernierMorceau = corpsDuSerpent[corpsDuSerpent.Count - 1];
                double nouveauX = Canvas.GetLeft(dernierMorceau);
                double nouveauY = Canvas.GetTop(dernierMorceau);

                AjouterMorceauSnake(nouveauX, nouveauY);

                // Générer une nouvelle pomme
                AjouterPomme();
            }
        }

        private void GameOver()
        {
            timerJeu.Stop();
            if (score > Brainrot_idle.Ressources.GameState.MeilleurScoreSnake)
            {
                Brainrot_idle.Ressources.GameState.MeilleurScoreSnake = score;
                MessageBox.Show($"Nouveau Record ! Score final : {score}", "Perdu !", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Game Over ! Votre score final est de : {score}", "Perdu !", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            DemarrerNouvellePartie();
        }

        private void AjouterMorceauSnake(double x, double y)
        {

            Rectangle morceau = new Rectangle
            {
                Width = TailleCase,
                Height = TailleCase,
                Fill = Brushes.Green,
                Stroke = Brushes.Green,
                StrokeThickness = 1
            };

            Canvas.SetLeft(morceau, x);
            Canvas.SetTop(morceau, y);

            ZoneDeJeu.Children.Add(morceau);
            corpsDuSerpent.Add(morceau);
        }

        private void AjouterPomme()
        {
            if (pomme != null)
            {
                ZoneDeJeu.Children.Remove(pomme);
            }

            pomme = new Ellipse
            {
                Width = TailleCase,
                Height = TailleCase,
                Fill = Brushes.Red
            };

            // 800px / 20 = 40 colonnes (0 à 39)
            // 600px / 20 = 30 lignes (0 à 29)
            int x = random.Next(0, 40) * TailleCase;
            int y = random.Next(0, 30) * TailleCase;

            Canvas.SetLeft(pomme, x);
            Canvas.SetTop(pomme, y);

            ZoneDeJeu.Children.Add(pomme);
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (directionChangeeCeTour) return;

            if (e.Key == Key.Up && directionY != 1)
            {
                directionX = 0;
                directionY = -1;
                directionChangeeCeTour = true;
                e.Handled = true;
            }
            else if (e.Key == Key.Down && directionY != -1)
            {
                directionX = 0;
                directionY = 1;
                directionChangeeCeTour = true;
                e.Handled = true;
            }
            else if (e.Key == Key.Left && directionX != 1)
            {
                directionX = -1;
                directionY = 0;
                directionChangeeCeTour = true;
                e.Handled = true;
            }
            else if (e.Key == Key.Right && directionX != -1)
            {
                directionX = 1;
                directionY = 0;
                directionChangeeCeTour = true;
                e.Handled = true;
            }
        }
    }
}