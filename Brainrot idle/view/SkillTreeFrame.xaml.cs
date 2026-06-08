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

        // ---------------- SYSTÈME D'ACHAT GÉNÉRIQUE ----------------
        /// <summary>
        /// Tente d'acheter une compétence en consommant 1 point de compétence.
        /// </summary>
        /// <param name="nomCompetence">Nom de la compétence affiché dans les messages.</param>
        /// <param name="dejaAchete">Booléen du GameState indiquant si l'amélioration est possédée.</param>
        /// <returns>True si l'achat a réussi, sinon False.</returns>
        private bool TenterAchatCompetence(string nomCompetence, bool dejaAchete)
        {
            if (dejaAchete)
            {
                MessageBox.Show($"{nomCompetence} est déjà débloqué !");
                return false;
            }

            if (GameState.PointsDeCompetence > 0)
            {
                GameState.PointsDeCompetence -= 1;
                MettreAjourInterfaceXP();
                MessageBox.Show($"{nomCompetence} débloqué avec succès !");
                return true;
            }
            else
            {
                MessageBox.Show("Points de compétence insuffisants.");
                return false;
            }
        }

        // ---------------- COMPÉTENCES LIÉES AUX MINI-JEUX ----------------
        private void JeuDeCombat_Click(object sender, RoutedEventArgs e)
        {
            if (TenterAchatCompetence("Jeu de combat", GameState.IsCombatDebloque))
            {
                GameState.IsCombatDebloque = true;
            }
        }

        private void Jeu3_Click(object sender, RoutedEventArgs e)
        {
            if (TenterAchatCompetence("Jeu Snake", GameState.IsSnakeDebloque))
            {
                GameState.IsSnakeDebloque = true;
            }
        }

        private void Jeu4_Click(object sender, RoutedEventArgs e)
        {
            if (TenterAchatCompetence("Jeu Morpion", GameState.IsMorpionDebloque))
            {
                GameState.IsMorpionDebloque = true;
            }
        }

        // ---------------- AUTRES COMPÉTENCES (PROVISOIRES) ----------------
        // Ces boutons renvoient 'false' par défaut tant qu'ils n'ont pas de variables dédiées dans le GameState.
        private void Jeu5_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Jeu 5", false);
        private void Jeu6_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Jeu 6", false);
        private void Multiplicateur_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Multiplicateur", false);
        private void Boost1_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Boost 1", false);
        private void Boost2_Click(object sender, RoutedEventArgs e) => TenterAchatCompetence("Boost 2", false);
    }
}