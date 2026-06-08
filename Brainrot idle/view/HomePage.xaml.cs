using Brainrot_idle.Ressources;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Brainrot_idle.view
{
    public partial class HomePage : Page
    {
        // Ce timer sert uniquement à rafraîchir l'interface utilisateur (UI)
        private DispatcherTimer uiTimer;

        public HomePage()
        {
            InitializeComponent();

            // S'exécute à chaque fois que la page s'affiche à l'écran
            this.Loaded += HomePage_Loaded;

            // Configuration du rafraîchissement automatique de l'interface
            uiTimer = new DispatcherTimer();
            uiTimer.Interval = TimeSpan.FromMilliseconds(200); // 5 fois par seconde pour une fluidité maximale
            uiTimer.Tick += UiTimer_Tick;
            uiTimer.Start(); // Lance le rafraîchissement

            // NOTE : L'événement 'this.Unloaded += HomePage_Unloaded;' a été supprimé 
            // pour empêcher le timer de s'éteindre lors de la navigation vers le Skill Tree.
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            // Force un rafraîchissement visuel immédiat dès qu'on revient sur la page
            UpdateUI();
        }

        // ---------------- REFRESH DE L'UI (BOUCLE) ----------------
        private void UiTimer_Tick(object sender, EventArgs e)
        {
            // Le timer se contente de demander la mise à jour des éléments visuels
            UpdateUI();
        }

        // ---------------- CLICK PRINCIPAL ----------------
        private void AuraButton_Click(object sender, RoutedEventArgs e)
        {
            // Sécurité pour éviter un multiplicateur à 0 ou négatif
            if (GameState.MultiplicateurAuraParClic <= 0) GameState.MultiplicateurAuraParClic = 1.0;

            // Calcul du gain réel basé sur l'arbre de compétences
            double pointsGagnes = 1.0 * GameState.MultiplicateurAuraParClic;

            GameState.points += pointsGagnes;
            GameState.clicsCetteSeconde += 1;

            UpdateUI();
        }

        // ---------------- LOGIQUE DES AMÉLIORATIONS ----------------
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
                // Message discret sans bloquer le thread principal du jeu
                UpdateUI();
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

        // ---------------- MISE À JOUR DE L'INTERFACE ----------------
        private void UpdateUI()
        {
            // Calcul de la production passive totale intégrant le bonus du mini-jeu Snake
            double multiplicateurSnake = 1.0 + (GameState.MeilleurScoreSnake * 0.15);
            double auraBoosteParSec = GameState.auraParSeconde * multiplicateurSnake;

            double multiplicateurClic = GameState.MultiplicateurAuraParClic <= 0 ? 1.0 : GameState.MultiplicateurAuraParClic;
            double pointsClicsCeSeconde = GameState.ClicsParSeconde * multiplicateurClic;
            double productionTotaleParSec = auraBoosteParSec + pointsClicsCeSeconde;

            // Affichage des textes d'Aura principaux
            AuraPoints.Content = "AuraPoints : " + FormatterNombre(GameState.points);
            AuraPointsParSec.Content = "Points/seconde : " + FormatterNombre(productionTotaleParSec);
            // Quantités possédées
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

            // Prix actuels
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

        // ---------------- FORMATTAGE DES NOMBRES ----------------
        private string FormatterNombre(double nombre)
        {
            string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };
            int index = 0;

            while (nombre >= 1000 && index < suffixes.Length - 1)
            {
                nombre /= 1000;
                index++;
            }

            return nombre.ToString("0.00") + suffixes[index];
        }

        // ---------------- NAVIGATION BETWEEN FRAMES ----------------
        private void MiniGames_Button_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new MiniGamesFrame());
        private void SkillTree_Button_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new SkillTreeFrame());
        private void Statistiques_Button_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new StatistiquesFrames());
        //private void button4_Button_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ParametreFrame());
        private void Music_Button_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new MusicFrame());
        private void Parametre_Button_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ParametreFrame());
    }
}