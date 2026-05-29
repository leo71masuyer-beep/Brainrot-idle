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
        private double points = 0;
        public static double auraPoints = 0;
        public static double auraParSeconde = 0;
        private double clicsCetteSeconde = 0;

        public static int nbAmelioration1 = 0;
        public static int nbAmelioration2 = 0;
        public static int nbAmelioration3 = 0;
        public static int nbAmelioration4 = 0;
        public static int nbAmelioration5 = 0;
        public static int nbAmelioration6 = 0;
        public static int nbAmelioration7 = 0;
        public static int nbAmelioration8 = 0;
        public static int nbAmelioration9 = 0;
        public static int nbAmelioration10 = 0;

        private double prixAmelioration1 = 10;
        private double prixAmelioration2 = 100;
        private double prixAmelioration3 = 1_000;
        private double prixAmelioration4 = 10_000;
        private double prixAmelioration5 = 100_000;
        private double prixAmelioration6 = 1_000_000;
        private double prixAmelioration7 = 10_000_000;
        private double prixAmelioration8 = 100_000_000;
        private double prixAmelioration9 = 1_000_000_000;
        private double prixAmelioration10 = 10_000_000_000;

        private DispatcherTimer timer;
        public HomePage()
        {
            InitializeComponent();

            AuraPoints.Content = "AuraPoints : " + FormatterNombre(points);
            AuraPointsParSec.Content = "Points/seconde : " + FormatterNombre(auraParSeconde);

            NbAmelioration1.Content = "Possédé : " + nbAmelioration1;
            NbAmelioration2.Content = "Possédé : " + nbAmelioration2;
            NbAmelioration3.Content = "Possédé : " + nbAmelioration3;
            NbAmelioration4.Content = "Possédé : " + nbAmelioration4;
            NbAmelioration5.Content = "Possédé : " + nbAmelioration5;
            NbAmelioration6.Content = "Possédé : " + nbAmelioration6;
            NbAmelioration7.Content = "Possédé : " + nbAmelioration7;
            NbAmelioration8.Content = "Possédé : " + nbAmelioration8;
            NbAmelioration9.Content = "Possédé : " + nbAmelioration9;
            NbAmelioration10.Content = "Possédé : " + nbAmelioration10;

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
            if (points >= prixAmelioration1)
            {
                points -= (long)prixAmelioration1;
                auraParSeconde += 1;
                nbAmelioration1++;

                prixAmelioration1 *= 1.1;
               PrixAmelioration1.Content = "Prix : " + FormatterNombre(prixAmelioration1);

                NbAmelioration1.Content = "Possédé : " + nbAmelioration1;
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
            if (points >= prixAmelioration2)
            {
                points -= (long)prixAmelioration2;
                auraParSeconde += 10;
                nbAmelioration2++;

                prixAmelioration2 *= 1.1;
                PrixAmelioration2.Content = "Prix : " + FormatterNombre(prixAmelioration2);

                NbAmelioration2.Content = "Possédé : " + nbAmelioration2;
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
            if (points >= prixAmelioration3)
            {
                points -= (long)prixAmelioration3;
                auraParSeconde += 100;
                nbAmelioration3++;

                prixAmelioration3 *= 1.1;
                PrixAmelioration3.Content = "Prix : " + FormatterNombre(prixAmelioration3);

                NbAmelioration3.Content = "Possédé : " + nbAmelioration3;

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
            if (points >= prixAmelioration4)
            {
                points -= (long)prixAmelioration4;
                auraParSeconde += 1_000;
                nbAmelioration4++;
                prixAmelioration4 *= 1.1;
                PrixAmelioration4.Content = "Prix : " + FormatterNombre(prixAmelioration4);
                NbAmelioration4.Content = "Possédé : " + nbAmelioration4;

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
            if (points >= prixAmelioration5)
            {
                points -= (long)prixAmelioration5;
                auraParSeconde += 10_000;
                nbAmelioration5++;
                prixAmelioration5 *= 1.1;
                PrixAmelioration5.Content = "Prix : " + FormatterNombre(prixAmelioration5);
                NbAmelioration5.Content = "Possédé : " + nbAmelioration5;

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
            if (points >= prixAmelioration6)
            {
                points -= (long)prixAmelioration6;
                auraParSeconde += 100_000;
                nbAmelioration6++;
                prixAmelioration6 *= 1.1;
                PrixAmelioration6.Content = "Prix : " + FormatterNombre(prixAmelioration6);
                NbAmelioration6.Content = "Possédé : " + nbAmelioration6;

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
            if (points >= prixAmelioration7)
            {
                points -= (long)prixAmelioration7;
                auraParSeconde += 1_000_000;
                nbAmelioration7++;
                prixAmelioration7 *= 1.1;
                PrixAmelioration7.Content = "Prix : " + FormatterNombre(prixAmelioration7);
                NbAmelioration7.Content = "Possédé : " + nbAmelioration7;

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
            if (points >= prixAmelioration8)
            {
                points -= (long)prixAmelioration8;
                auraParSeconde += 10_000_000;
                nbAmelioration8++;
                prixAmelioration8 *= 1.1;
                PrixAmelioration8.Content = "Prix : " + FormatterNombre(prixAmelioration8);
                NbAmelioration8.Content = "Possédé : " + nbAmelioration8;

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
            if (points >= prixAmelioration9)
            {
                points -= (long)prixAmelioration9;
                auraParSeconde += 100_000_000;
                nbAmelioration9++;
                prixAmelioration9 *= 1.1;
                PrixAmelioration9.Content = "Prix : " + FormatterNombre(prixAmelioration9);
                NbAmelioration9.Content = "Possédé : " + nbAmelioration9;

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
            if (points >= prixAmelioration10)
            {
                points -= (long)prixAmelioration10;
                auraParSeconde += 1_000_000_000;
                nbAmelioration10++;
                prixAmelioration10 *= 1.1;
                PrixAmelioration10.Content = "Prix : " + FormatterNombre(prixAmelioration10);
                NbAmelioration10.Content = "Possédé : " + nbAmelioration10;

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
            points += auraParSeconde;
            AuraPointsParSec.Content = "Points/seconde : " + FormatterNombre((auraParSeconde + clicsCetteSeconde));
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
