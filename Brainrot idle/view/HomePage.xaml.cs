using Brainrot_idle.Ressources;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Brainrot_idle.view
{
    public partial class HomePage : Page
    {
        private DispatcherTimer timer;

        public HomePage()
        {
            InitializeComponent();

            UpdateUI();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        // ---------------- CLICK PRINCIPAL ----------------
        private void AuraButton_Click(object sender, RoutedEventArgs e)
        {
            GameState.points += 1;
            GameState.clicsCetteSeconde += 1;

            UpdateUI();
        }

        // ---------------- AMÉLIORATIONS ----------------
        private void BuyUpgrade(int index, double gain)
        {
            if (GameState.points >= GameState.prixAmeliorations[index])
            {
                GameState.points -= GameState.prixAmeliorations[index];
                GameState.auraParSeconde += gain;
                GameState.nbAmeliorations[index]++;

                GameState.prixAmeliorations[index] *= 1.1;

                UpdateUI();
            }
            else
            {
                MessageBox.Show("Pas assez de points");
            }
        }

        private void Amelioration1_Click(object sender, RoutedEventArgs e) => BuyUpgrade(0, 1);
        private void Amelioration2_Click(object sender, RoutedEventArgs e) => BuyUpgrade(1, 10);
        private void Amelioration3_Click(object sender, RoutedEventArgs e) => BuyUpgrade(2, 100);
        private void Amelioration4_Click(object sender, RoutedEventArgs e) => BuyUpgrade(3, 1_000);
        private void Amelioration5_Click(object sender, RoutedEventArgs e) => BuyUpgrade(4, 10_000);
        private void Amelioration6_Click(object sender, RoutedEventArgs e) => BuyUpgrade(5, 100_000);
        private void Amelioration7_Click(object sender, RoutedEventArgs e) => BuyUpgrade(6, 1_000_000);
        private void Amelioration8_Click(object sender, RoutedEventArgs e) => BuyUpgrade(7, 10_000_000);
        private void Amelioration9_Click(object sender, RoutedEventArgs e) => BuyUpgrade(8, 100_000_000);
        private void Amelioration10_Click(object sender, RoutedEventArgs e) => BuyUpgrade(9, 1_000_000_000);

        // ---------------- TIMER ----------------
        private void Timer_Tick(object sender, EventArgs e)
        {
            GameState.points += GameState.auraParSeconde;

            AuraPointsParSec.Content =
                "Points/seconde : " + FormatterNombre(GameState.auraParSeconde + GameState.clicsCetteSeconde);

            GameState.clicsCetteSeconde = 0;

            UpdateUI();
        }

        // ---------------- UI ----------------
        private void UpdateUI()
        {
            AuraPoints.Content = "AuraPoints : " + FormatterNombre(GameState.points);

            NbAmelioration1.Content = "Possédé : " + GameState.nbAmeliorations[0];
            NbAmelioration2.Content = "Possédé : " + GameState.nbAmeliorations[1];
            NbAmelioration3.Content = "Possédé : " + GameState.nbAmeliorations[2];
            NbAmelioration4.Content = "Possédé : " + GameState.nbAmeliorations[3];
            NbAmelioration5.Content = "Possédé : " + GameState.nbAmeliorations[4];
            NbAmelioration6.Content = "Possédé : " + GameState.nbAmeliorations[5];
            NbAmelioration7.Content = "Possédé : " + GameState.nbAmeliorations[6];
            NbAmelioration8.Content = "Possédé : " + GameState.nbAmeliorations[7];
            NbAmelioration9.Content = "Possédé : " + GameState.nbAmeliorations[8];
            NbAmelioration10.Content = "Possédé : " + GameState.nbAmeliorations[9];

            PrixAmelioration1.Content = "Prix : " + FormatterNombre(GameState.prixAmeliorations[0]);
            PrixAmelioration2.Content = "Prix : " + FormatterNombre(GameState.prixAmeliorations[1]);
            PrixAmelioration3.Content = "Prix : " + FormatterNombre(GameState.prixAmeliorations[2]);
            PrixAmelioration4.Content = "Prix : " + FormatterNombre(GameState.prixAmeliorations[3]);
            PrixAmelioration5.Content = "Prix : " + FormatterNombre(GameState.prixAmeliorations[4]);
            PrixAmelioration6.Content = "Prix : " + FormatterNombre(GameState.prixAmeliorations[5]);
            PrixAmelioration7.Content = "Prix : " + FormatterNombre(GameState.prixAmeliorations[6]);
            PrixAmelioration8.Content = "Prix : " + FormatterNombre(GameState.prixAmeliorations[7]);
            PrixAmelioration9.Content = "Prix : " + FormatterNombre(GameState.prixAmeliorations[8]);
            PrixAmelioration10.Content = "Prix : " + FormatterNombre(GameState.prixAmeliorations[9]);
        }

        // ---------------- FORMAT ----------------
        private string FormatterNombre(double nombre)
        {
            string[] suffixes =
            {
                "", "K", "M", "B", "T",
                "Qa", "Qi", "Sx", "Sp",
                "Oc", "No", "Dc"
            };

            int index = 0;

            while (nombre >= 1000 && index < suffixes.Length - 1)
            {
                nombre /= 1000;
                index++;
            }

            return nombre.ToString("0.00") + suffixes[index];
        }

        // ---------------- NAVIGATION ----------------
        private void MiniGames_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MiniGamesFrame());
        }

        private void Parametre_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ParametreFrame());
        }
        private void SkillTree_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SkillTreeFrame());
        }
    }
}