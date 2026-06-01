using Brainrot_idle.Ressources;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Brainrot_idle.view
{
    public partial class SkillTreeFrame : Page
    {
        public SkillTreeFrame()
        {
            InitializeComponent();
            MettreAjourInterfaceXP();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        // Action lors du clic sur le bouton "+1 XP"
        private void XpButton_Click(object sender, RoutedEventArgs e)
        {
            GameState.XpActuelle += 1 * GameState.MultiplicateurXp;

            // Vérification du passage de niveau
            while (GameState.XpActuelle >= GameState.XpRequise)
            {
                GameState.XpActuelle -= GameState.XpRequise;
                GameState.Niveau += 1;
                GameState.PointsDeCompetence += 1;

                GameState.XpRequise = Math.Ceiling(GameState.XpRequise * 1.25);

            }

            MettreAjourInterfaceXP();
        }

        private void MettreAjourInterfaceXP()
        {
            TexteNiveau.Text = "Niveau : " + GameState.Niveau;
            TextePointsDispo.Text = "Points disponibles : " + GameState.PointsDeCompetence;
            BarreXp.Maximum = GameState.XpRequise;
            BarreXp.Value = GameState.XpActuelle;
            TexteXp.Text = $"{GameState.XpActuelle:0.0} / {GameState.XpRequise} XP";
        }

        // ---------------- CLIC DE LA COMPÉTENCE APPRENTI ----------------
        private void Apprenti_Click(object sender, RoutedEventArgs e)
        {
            int coutActuel = GameState.NiveauApprenti + 1;

            if (GameState.PointsDeCompetence >= coutActuel)
            {
                GameState.PointsDeCompetence -= coutActuel;
                GameState.NiveauApprenti += 1;
                GameState.MultiplicateurXp *= 2.5;

                MettreAjourInterfaceXP();

            }
            else
            {
                // Optionnel : Tu peux laisser un avertissement si le joueur clique sans points, 
                // mais l'idéal est de ne rien faire ou de désactiver le bouton visuellement.
            }
        }

        // ---------------- CLIC DE LA COMPÉTENCE CLIQUEUR ----------------
        private void Cliqueur_Click(object sender, RoutedEventArgs e)
        {
            int coutActuel = GameState.NiveauCliqueur + 1;

            if (GameState.PointsDeCompetence >= coutActuel)
            {
                GameState.PointsDeCompetence -= coutActuel;
                GameState.NiveauCliqueur += 1;
                GameState.MultiplicateurAuraParClic *= 2.5;

                MettreAjourInterfaceXP();

            }
        }

        // ---------------- AUTRES COMPÉTENCES ----------------
        private void TenterAchatCompetence(string nomCompetence, Action actionEffet)
        {
            if (GameState.PointsDeCompetence > 0)
            {
                GameState.PointsDeCompetence -= 1;
                actionEffet.Invoke();
                MettreAjourInterfaceXP();
            }
        }

        private void JeuDeCombat_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Jeu de combat", () => { });
        private void Jeu3_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Jeu 3", () => { });
        private void Jeu4_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Jeu 4", () => { });
        private void Jeu5_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Jeu 5", () => { });
        private void Jeu6_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Jeu 6", () => { });
        private void Multiplicateur_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Multiplicateur", () => { });
        private void Boost1_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Boost 1", () => { });
        private void Boost2_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Boost 2", () => { });
    }
}