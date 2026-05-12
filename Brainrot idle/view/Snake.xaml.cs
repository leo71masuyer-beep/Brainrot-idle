using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Brainrot_idle.view
{
    public partial class Snake : Page
    {
        private void Return_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MiniGamesFrame());
        }
        private List<Rectangle> corpsDuSerpent = new List<Rectangle>();
        private const int TailleCase = 20;
        private System.Windows.Threading.DispatcherTimer timerJeu = new System.Windows.Threading.DispatcherTimer();

        public Snake()
        {
            InitializeComponent();

            AjouterMorceau(200, 200);
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