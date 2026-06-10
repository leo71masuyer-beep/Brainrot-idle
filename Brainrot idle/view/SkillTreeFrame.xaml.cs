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
            // Mise à jour des textes principaux de l'en-tête
            TexteNiveau.Text = "Niveau : " + GameState.Niveau;
            TextePointsDispo.Text = "Points disponibles : " + GameState.PointsDeCompetence;
            BarreXp.Maximum = GameState.XpRequise;
            BarreXp.Value = GameState.XpActuelle;
            TexteXp.Text = $"{GameState.XpActuelle:0.0} / {GameState.XpRequise} XP";

            // Mise à jour dynamique du coût textuel affiché dans l'arbre pour chaque bouton
            // Rangée 1
            BtnApprenti.Content = $"Apprenti\n(Niveau {GameState.NiveauApprenti})";
            BtnCliqueur.Content = $"Cliqueur\n(Niveau {GameState.NiveauCliqueur})";

            // Rangée 2 (Si débloqué, on affiche 'Acheté', sinon le prix)
            LblPrixApprenti.Text = $"Prix : {GameState.NiveauApprenti + 1} Pt";
            LblPrixCliqueur.Text = $"Prix : {GameState.NiveauCliqueur + 1} Pt";

            LblPrixCombat.Text = GameState.IsCombatDebloque ? "Débloqué" : "Prix : 2 Pts";
            LblPrixMorpion.Text = GameState.IsMorpionDebloque ? "Débloqué" : "Prix : 2 Pts";
            LblPrixJeu4.Text = GameState.IsJeu4Debloque ? "Débloqué" : "Prix : 2 Pts";
            LblPrixJeu5.Text = GameState.IsJeu5Debloque ? "Débloqué" : "Prix : 2 Pts";

            // Rangée 3
            LblPrixJeu6.Text = GameState.IsJeu6Debloque ? "Débloqué" : "Prix : 3 Pts";
            LblPrixJeu7.Text = GameState.IsJeu7Debloque ? "Débloqué" : "Prix : 3 Pts";
            LblPrixSnake.Text = GameState.IsSnakeDebloque ? "Débloqué" : "Prix : 3 Pts";

            // Rangée 4
            LblPrixJeu8.Text = GameState.IsJeu8Debloque ? "Débloqué" : "Prix : 5 Pts";
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
                MessageBox.Show("Points de compétence insuffisants.");
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
            else
            {
                MessageBox.Show("Points de compétence insuffisants.");
            }
        }

        // ---------------- SYSTÈME D'ACHAT GÉNÉRIQUE CONFIGURABLE ----------------
        /// <summary>
        /// Tente d'acheter une compétence en consommant le nombre exact de points requis.
        /// </summary>
        private bool TenterAchatCompetence(string nomCompetence, bool dejaAchete, int cout)
        {
            if (dejaAchete)
            {
                MessageBox.Show($"{nomCompetence} est déjà débloqué !");
                return false;
            }

            if (GameState.PointsDeCompetence >= cout)
            {
                GameState.PointsDeCompetence -= cout;
                MettreAjourInterfaceXP();
                MessageBox.Show($"{nomCompetence} débloqué avec succès !");
                return true;
            }
            else
            {
                MessageBox.Show($"Points de compétence insuffisants (Il te faut {cout} Pts).");
                return false;
            }
        }

        // ---------------- COMPÉTENCES LIÉES AUX MINI-JEUX ----------------

        private void JeuDeCombat_Click(object sender, RoutedEventArgs e)
        {
            if (TenterAchatCompetence("Jeu de combat", GameState.IsCombatDebloque, 2))
            {
                GameState.IsCombatDebloque = true;
                MettreAjourInterfaceXP();
            }
        }

        private void Morpion_Click(object sender, RoutedEventArgs e)
        {
            if (TenterAchatCompetence("Jeu Morpion", GameState.IsMorpionDebloque, 2))
            {
                GameState.IsMorpionDebloque = true;
                MettreAjourInterfaceXP();
            }
        }

        private void Jeu4_Click(object sender, RoutedEventArgs e)
        {
            if (TenterAchatCompetence("Jeu 4", GameState.IsJeu4Debloque, 2))
            {
                GameState.IsJeu4Debloque = true;
                MettreAjourInterfaceXP();
            }
        }

        private void Jeu5_Click(object sender, RoutedEventArgs e)
        {
            if (TenterAchatCompetence("Jeu 5", GameState.IsJeu5Debloque, 2))
            {
                GameState.IsJeu5Debloque = true;
                MettreAjourInterfaceXP();
            }
        }

        private void Jeu6_Click(object sender, RoutedEventArgs e)
        {
            if (TenterAchatCompetence("Jeu 6", GameState.IsJeu6Debloque, 3))
            {
                GameState.IsJeu6Debloque = true;
                MettreAjourInterfaceXP();
            }
        }

        private void Jeu7_Click(object sender, RoutedEventArgs e)
        {
            if (TenterAchatCompetence("Jeu 7", GameState.IsJeu7Debloque, 3))
            {
                GameState.IsJeu7Debloque = true;
                MettreAjourInterfaceXP();
            }
        }

        private void Snake_Click(object sender, RoutedEventArgs e)
        {
            if (TenterAchatCompetence("Jeu Snake", GameState.IsSnakeDebloque, 3))
            {
                GameState.IsSnakeDebloque = true;
                MettreAjourInterfaceXP();
            }
        }

        private void Jeu8_Click(object sender, RoutedEventArgs e)
        {
            if (TenterAchatCompetence("Jeu 8", GameState.IsJeu8Debloque, 5))
            {
                GameState.IsJeu8Debloque = true;
                MettreAjourInterfaceXP();
            }
        }
    }
}