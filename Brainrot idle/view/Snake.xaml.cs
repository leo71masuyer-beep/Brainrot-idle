using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Brainrot_idle.view
{
    public partial class Snake : Window
    {
        private List<Rectangle> corpsDuSerpent = new List<Rectangle>();
        private const int TailleCase = 20;
        private System.Windows.Threading.DispatcherTimer timerJeu = new System.Windows.Threading.DispatcherTimer();
        private int directionX = 1;
        private int directionY = 0;

        public Snake()
        {
            InitializeComponent();
            AjouterMorceau(200, 200);

            timerJeu.Interval = TimeSpan.FromMilliseconds(150);
            timerJeu.Tick += TimerJeu_Tick;
            timerJeu.Start();
        }

        private void TimerJeu_Tick(object sender, EventArgs e)
        {
            BougerSerpent();
        }

        private void BougerSerpent()
        {
            Rectangle tete = corpsDuSerpent[0];

            double positionXActuelle = Canvas.GetLeft(tete);
            double positionYActuelle = Canvas.GetTop(tete);

            Canvas.SetLeft(tete, positionXActuelle + (directionX * TailleCase));
            Canvas.SetTop(tete, positionYActuelle + (directionY * TailleCase));
        }

        private void AjouterMorceau(double x, double y)
        {
            Rectangle morceau = new Rectangle
            {
                Width = TailleCase,
                Height = TailleCase,
                Fill = Brushes.Green,
                Margin = new Thickness(1)
            };

            Canvas.SetLeft(morceau, x);
            Canvas.SetTop(morceau, y);

            ZoneDeJeu.Children.Add(morceau);
            corpsDuSerpent.Add(morceau);
        }
    }
}