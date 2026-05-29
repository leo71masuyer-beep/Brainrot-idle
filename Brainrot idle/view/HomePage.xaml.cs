using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Brainrot_idle.view
{
    public partial class HomePage : Page
    {
        private long points = 0;
        private long gainParSeconde = 0;
        private long clicsCetteSeconde = 0;

        private int amelioration1Possede = 0;
        private int amelioration2Possede = 0;
        private int amelioration3Possede = 0;
        private int amelioration4Possede = 0;
        private int amelioration5Possede = 0;
        private int amelioration6Possede = 0;
        private int amelioration7Possede = 0;
        private int amelioration8Possede = 0;
        private int amelioration9Possede = 0;
        private int amelioration10Possede = 0;

        private DispatcherTimer timer;
        public HomePage()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        //ensemble des differents onglet
        private void MiniGames_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MiniGamesFrame());
        }
        private void Parametre_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ParametreFrame());
        }
        //fonction principal du jeu
        private void AuraButton_Click(object sender, RoutedEventArgs e)
        {
            points += 1;
            clicsCetteSeconde += 1;
            MettreAJourLabel();
        }
        //---------------------------------------------------------Amélioration---------------------------------------------------------------------------------------
        private void Amelioration1_Click(object sender, RoutedEventArgs e)
        {
            // Vérifie si le joueur a assez de points
            if (points >= 10)
            {
                points -= 10;
                gainParSeconde += 1;
                amelioration1Possede++;

                NbAmelioration1.Content ="Possédé : " + amelioration1Possede;

                MettreAJourLabel();
            }
            else
            {
                MessageBox.Show("Pas assez de point");
            }
        }
        private void Amelioration2_Click(object sender, RoutedEventArgs e)
        {
            // Vérifie si le joueur a assez de points
            if (points >= 100)
            {
                points -= 100;
                gainParSeconde += 10;

                NbAmelioration2.Content = "Possédé : " + amelioration2Possede;

                MettreAJourLabel();
            }
            else
            {
                MessageBox.Show("Pas assez de point");
            }
        }
        private void Amelioration3_Click(object sender, RoutedEventArgs e)
        {
            // Vérifie si le joueur a assez de points
            if (points >= 1_000)
            {
                points -= 1_000;
                gainParSeconde += 100;

                NbAmelioration3.Content = "Possédé : " + amelioration3Possede;

                MettreAJourLabel();
            }
            else
            {
                MessageBox.Show("Pas assez de point");
            }
        }
        private void Amelioration4_Click(object sender, RoutedEventArgs e)
        {
            // Vérifie si le joueur a assez de points
            if (points >= 10_000)
            {
                points -= 10_000;
                gainParSeconde += 1_000;

                NbAmelioration4.Content = "Possédé : " + amelioration4Possede;

                MettreAJourLabel();
            }
            else
            {
                MessageBox.Show("Pas assez de point");
            }
        }
        private void Amelioration5_Click(object sender, RoutedEventArgs e)
        {
            // Vérifie si le joueur a assez de points
            if (points >= 100_000)
            {
                points -= 100_000;
                gainParSeconde += 10_000;

                NbAmelioration5.Content = "Possédé : " + amelioration5Possede;

                MettreAJourLabel();
            }
            else
            {
                MessageBox.Show("Pas assez de point");
            }
        }
        private void Amelioration6_Click(object sender, RoutedEventArgs e)
        {
            // Vérifie si le joueur a assez de points
            if (points >= 1_000_000)
            {
                points -= 1_000_000;
                gainParSeconde += 100_000;

                NbAmelioration6.Content = "Possédé : " + amelioration6Possede;

                MettreAJourLabel();
            }
            else
            {
                MessageBox.Show("Pas assez de point");
            }
        }
        private void Amelioration7_Click(object sender, RoutedEventArgs e)
        {
            // Vérifie si le joueur a assez de points
            if (points >= 10_000_000)
            {
                points -= 10_000_000;
                gainParSeconde += 1_000_000;

                NbAmelioration7.Content = "Possédé : " + amelioration7Possede;

                MettreAJourLabel();
            }
            else
            {
                MessageBox.Show("Pas assez de point");
            }
        }
        private void Amelioration8_Click(object sender, RoutedEventArgs e)
        {
            // Vérifie si le joueur a assez de points
            if (points >= 100_000_000)
            {
                points -= 100_000_000;
                gainParSeconde += 10_000_000;

                NbAmelioration8.Content = "Possédé : " + amelioration8Possede;

                MettreAJourLabel();
            }
            else
            {
                MessageBox.Show("Pas assez de point");
            }
        }
        private void Amelioration9_Click(object sender, RoutedEventArgs e)
        {
            // Vérifie si le joueur a assez de points
            if (points >= 1_000_000_000)
            {
                points -= 1_000_000_000;
                gainParSeconde += 100_000_000;

                NbAmelioration9.Content = "Possédé : " + amelioration9Possede;

                MettreAJourLabel();
            }
            else
            {
                MessageBox.Show("Pas assez de point");
            }
        }
        private void Amelioration10_Click(object sender, RoutedEventArgs e)
        {
            // Vérifie si le joueur a assez de points
            if (points >= 10_000_000_000)
            {
                points -= 10_000_000_000;
                gainParSeconde += 1_000_000_000;

                NbAmelioration10.Content = "Possédé : " + amelioration10Possede;

                MettreAJourLabel();
            }
            else
            {
                MessageBox.Show("Pas assez de point");
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Timer_Tick(object sender, EventArgs e)
        {
            points += gainParSeconde;
            AuraPointsParSec.Content = "Points/seconde : " + FormatterNombre((gainParSeconde + clicsCetteSeconde));
            clicsCetteSeconde = 0;
            MettreAJourLabel();
        }
        private void MettreAJourLabel()
        {
            AuraPoints.Content = "AuraPoints : " + FormatterNombre(points);
        }
        private string FormatterNombre(double nombre)
        {
            string[] suffixes =
            {
        "",
        "K",
        "M",
        "B",
        "T",
        "Qa",
        "Qi",
        "Sx",
        "Sp",
        "Oc",
        "No",
        "Dc"
    };

            int index = 0;

            while (nombre >= 1000 && index < suffixes.Length - 1)
            {
                nombre /= 1000;
                index++;
            }

            return nombre.ToString("0.00") + suffixes[index];
        }

    }
}
