using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Brainrot_idle.view
{
    public partial class Snake : Page
    {
        private List<Rectangle> corpsDuSerpent = new List<Rectangle>();
        private const int TailleCase = 20;
        private System.Windows.Threading.DispatcherTimer timerJeu = new System.Windows.Threading.DispatcherTimer();
        private int directionX = 1;
        private int directionY = 0;
        private Rectangle nourriture;
        private Random rnd = new Random();

        public Snake()
        {
            InitializeComponent();

            // On crée la tête
            AjouterMorceau(200, 200);
            //On fait apparaître la première pomme
            PlacerNourriture();

            timerJeu.Interval = TimeSpan.FromMilliseconds(50);
            timerJeu.Tick += TimerJeu_Tick;
            timerJeu.Start();

            this.Focusable = true;
            this.Loaded += (s, e) => this.Focus();
        }

        private void Return_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MiniGamesFrame());
        }

        private void TimerJeu_Tick(object sender, EventArgs e)
        {
            BougerSerpent();
            VerifierCollisionNourriture();
        }

        private void BougerSerpent()
        {
            // Chaque morceau prend la place de celui qui le précède
            for (int i = corpsDuSerpent.Count - 1; i > 0; i--)
            {
                Canvas.SetLeft(corpsDuSerpent[i], Canvas.GetLeft(corpsDuSerpent[i - 1]));
                Canvas.SetTop(corpsDuSerpent[i], Canvas.GetTop(corpsDuSerpent[i - 1]));
            }

            Rectangle tete = corpsDuSerpent[0];
            double positionXActuelle = Canvas.GetLeft(tete);
            double positionYActuelle = Canvas.GetTop(tete);

            Canvas.SetLeft(tete, positionXActuelle + (directionX * TailleCase));
            Canvas.SetTop(tete, positionYActuelle + (directionY * TailleCase));
        }

        // Placer la nourriture aléatoirement
        private void PlacerNourriture()
        {
            if (nourriture != null) ZoneDeJeu.Children.Remove(nourriture);

            nourriture = new Rectangle
            {
                Width = TailleCase,
                Height = TailleCase,
                Fill = Brushes.Red
            };

            int x = rnd.Next(0, 20) * TailleCase;
            int y = rnd.Next(0, 15) * TailleCase;

            Canvas.SetLeft(nourriture, x);
            Canvas.SetTop(nourriture, y);
            ZoneDeJeu.Children.Add(nourriture);
        }

        // Vérifier si on mange la nourriture 
        private void VerifierCollisionNourriture()
        {
            Rectangle tete = corpsDuSerpent[0];

            double teteX = Canvas.GetLeft(tete);
            double teteY = Canvas.GetTop(tete);
            double nourritureX = Canvas.GetLeft(nourriture);
            double nourritureY = Canvas.GetTop(nourriture);

            // Si la tête est exactement sur la nourriture
            if (teteX == nourritureX && teteY == nourritureY)
            {
                // On ajoute un nouveau morceau. 
                // car la boucle dans BougerSerpent le replacera correctement au prochain mouvement.
                AjouterMorceau(-50, -50);

                // On déplace la nourriture ailleurs
                PlacerNourriture();
            }
        }

        private void AjouterMorceau(double x, double y)
        {
            // On peut optionnellement changer la couleur du corps pour le différencier de la tête
            Brush couleur = corpsDuSerpent.Count == 0 ? Brushes.Green : Brushes.LightGreen;

            Rectangle morceau = new Rectangle
            {
                Width = TailleCase,
                Height = TailleCase,
                Fill = couleur,
                Margin = new Thickness(1)
            };

            Canvas.SetLeft(morceau, x);
            Canvas.SetTop(morceau, y);

            ZoneDeJeu.Children.Add(morceau);
            corpsDuSerpent.Add(morceau);
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && directionY != 1) { directionX = 0; directionY = -1; }
            else if (e.Key == Key.Down && directionY != -1) { directionX = 0; directionY = 1; }
            else if (e.Key == Key.Left && directionX != 1) { directionX = -1; directionY = 0; }
            else if (e.Key == Key.Right && directionX != -1) { directionX = 1; directionY = 0; }
        }
    }
}