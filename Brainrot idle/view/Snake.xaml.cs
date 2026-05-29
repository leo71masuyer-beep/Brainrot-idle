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
        private DispatcherTimer timerJeu = new DispatcherTimer();
        private int directionX = 1;
        private int directionY = 0;
        private Ellipse pomme;
        private Random random = new Random();

        public Snake()
        {
            InitializeComponent();

            timerJeu.Interval = TimeSpan.FromMilliseconds(50);
            timerJeu.Tick += TimerJeu_Tick;

            Loaded += Snake_Loaded;
        }

        private void Snake_Loaded(object sender, RoutedEventArgs e)
        {
            AjouterMorceauSnake(200, 200);

            AjouterPomme();

            Focusable = true;
            Focus();
            Keyboard.Focus(this);

            timerJeu.Start();
        }

        private void Return_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MiniGamesFrame());
        }

        private void TimerJeu_Tick(object sender, EventArgs e)
        {
            BougerSerpent();
            VerifierCollisionPomme();
        }

        private void BougerSerpent()
        {
            for (int i = corpsDuSerpent.Count - 1; i > 0; i--)
            {
                double xPrecedent = Canvas.GetLeft(corpsDuSerpent[i - 1]);
                double yPrecedent = Canvas.GetTop(corpsDuSerpent[i - 1]);

                Canvas.SetLeft(corpsDuSerpent[i], xPrecedent);
                Canvas.SetTop(corpsDuSerpent[i], yPrecedent);
            }

            Rectangle tete = corpsDuSerpent[0];
            double positionXActuelle = Canvas.GetLeft(tete);
            double positionYActuelle = Canvas.GetTop(tete);

            Canvas.SetLeft(tete, positionXActuelle + (directionX * TailleCase));
            Canvas.SetTop(tete, positionYActuelle + (directionY * TailleCase));
        }

        private void AjouterMorceauSnake(double x, double y)
        {
            // On peut optionnellement changer la couleur du corps pour le différencier de la tête
            Brush couleur = corpsDuSerpent.Count == 0 ? Brushes.Green : Brushes.LightGreen;

            Rectangle morceau = new Rectangle
            {
                Width = TailleCase,
                Height = TailleCase,
                Fill = Brushes.Green
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

            int x = random.Next(0, 39) * TailleCase;
            int y = random.Next(0, 29) * TailleCase;

            Canvas.SetLeft(pomme, x);
            Canvas.SetTop(pomme, y);

            ZoneDeJeu.Children.Add(pomme);
        }

        private void VerifierCollisionPomme()
        {
            Rectangle tete = corpsDuSerpent[0];

            double teteX = Canvas.GetLeft(tete);
            double teteY = Canvas.GetTop(tete);

            double pommeX = Canvas.GetLeft(pomme);
            double pommeY = Canvas.GetTop(pomme);

            if (teteX == pommeX && teteY == pommeY)
            {
                Rectangle dernierMorceau = corpsDuSerpent[corpsDuSerpent.Count - 1];

                double nouveauX = Canvas.GetLeft(dernierMorceau);
                double nouveauY = Canvas.GetTop(dernierMorceau);

                AjouterMorceauSnake(nouveauX, nouveauY);

                AjouterPomme();
            }
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && directionY != 1)
            {
                directionX = 0;
                directionY = -1;
            }
            else if (e.Key == Key.Down && directionY != -1)
            {
                directionX = 0;
                directionY = 1;
            }
            else if (e.Key == Key.Left && directionX != 1)
            {
                directionX = -1;
                directionY = 0;
            }
            else if (e.Key == Key.Right && directionX != -1)
            {
                directionX = 1;
                directionY = 0;
            }
        }
    }
}