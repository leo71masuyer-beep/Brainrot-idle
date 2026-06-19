using System;
using System.Windows.Threading;
using System.Collections.Generic;

namespace Brainrot_idle.Ressources
{
    public static class GameState
    {

        // ---------------- DONNÉES DU JOUEUR ----------------
        public static double points = 0;
        public static double auraParSeconde = 0;
        public static double clicsCetteSeconde = 0;
        public static double ClicsParSeconde { get; set; } = 0;

        // ---------------- AMÉLIORATIONS DE BASE ----------------
        public static int[] nbAmeliorations = new int[10];
        public static double[] prixAmeliorations =
        {
            10,            // Amélioration 1
            100,           // Amélioration 2
            1_000,         // Amélioration 3
            10_000,        // Amélioration 4
            100_000,       // Amélioration 5
            1_000_000,     // Amélioration 6
            10_000_000,    // Amélioration 7
            100_000_000,   // Amélioration 8
            1_000_000_000, // Amélioration 9
            10_000_000_000 // Amélioration 10
        };

        // ---------------- MINI-JEUX ----------------
        public static int MeilleurScoreSnake { get; set; } = 0;
        public static int MeilleurScoreMorpion { get; set; } = 0;

        // États de déblocage de l'ensemble des jeux (Utilisés par MiniGamesFrame)
        public static bool IsSnakeDebloque { get; set; } = false;
        public static bool IsCombatDebloque { get; set; } = false;
        public static bool IsMorpionDebloque { get; set; } = false;
        public static bool IsJeu4Debloque { get; set; } = false;
        public static bool IsJeu5Debloque { get; set; } = false;
        public static bool IsJeu6Debloque { get; set; } = false;
        public static bool IsJeu7Debloque { get; set; } = false;
        public static bool IsJeu8Debloque { get; set; } = false;

        // ---------------- SYSTÈME DE NIVEAUX DANS L'ARBRE ----------------
        public static double XpActuelle { get; set; } = 0;
        public static double XpRequise { get; set; } = 10; // XP de départ pour le niveau 1
        public static int Niveau { get; set; } = 1;
        public static int PointsDeCompetence { get; set; } = 0;

        // ---------------- STATS DES COMPÉTENCES DÉBLOQUÉES ----------------
        public static int NiveauApprenti { get; set; } = 0;
        public static double MultiplicateurXp { get; set; } = 1.0; // Initialisé à 1.0 (très important !)
        public static int NiveauCliqueur { get; set; } = 0;
        public static double MultiplicateurAuraParClic { get; set; } = 1.0; // Initialisé à 1.0 (très important !)

        // ---------------- Systeme de GameCombat d'amelioration ----------------
        public static double AuraBonus { get; set; } = 0;
        public static double AuraBonusFlat { get; set; } = 0;

        // ---------------- MOTEUR IDLE CORE (BACKEND) ----------------
        private static DispatcherTimer globalTimer;
        private static DispatcherTimer autoSaveTimer;

        /// <summary>
        /// Initialise le cœur de calcul de l'économie. Lancé automatiquement une fois par App.xaml.cs.
        /// </summary>
        public static void InitialiserTimerGlobal()
        {
            // Timer principal du jeu
            if (globalTimer == null)
            {
                globalTimer = new DispatcherTimer();
                globalTimer.Interval = TimeSpan.FromSeconds(1);
                globalTimer.Tick += GlobalTimer_Tick;
                globalTimer.Start();
            }

            // Sauvegarde automatique toutes les 30 secondes
            if (autoSaveTimer == null)
            {
                autoSaveTimer = new DispatcherTimer();
                autoSaveTimer.Interval = TimeSpan.FromSeconds(30);

                autoSaveTimer.Tick += (sender, e) =>
                {
                    SaveManager.Save();
                };

                autoSaveTimer.Start();
            }
        }

        private static void GlobalTimer_Tick(object sender, EventArgs e)
        {
            // 1. Calcul du multiplicateur lié au score du jeu Snake (+10% par fruit)
            double multiplicateurSnake = (MeilleurScoreSnake * 0.1);

            // 1. Calcul du multiplicateur lié au Bonus d'aura obtenu par le jeu GameCombat (No limit)
            double multiplicateurGameCombat = (AuraBonus / 100);

            // 1. Somme de tout les multiplicateur
            double multiplicateurGlobal = 1.0 + multiplicateurSnake + multiplicateurGameCombat;

            // 2. Application de la production passive brute boostée
            double auraBoosteParSec = (auraParSeconde + AuraBonusFlat) * multiplicateurGlobal;

            // 3. Injection automatique des points dans le solde du joueur (Fonctionne en arrière-plan global)
            points += auraBoosteParSec;

            ClicsParSeconde = clicsCetteSeconde;

            // 4. Réinitialisation automatique des clics pour le calcul de la seconde suivante
            clicsCetteSeconde = 0;
            VerifierSucces();
        }

        // ---------------- SYSTÈME DE SUCCÈS ----------------
        public class Succes
        {
            public string? Id { get; set; }
            public string? Titre { get; set; }
            public string? Description { get; set; }
            public bool EstDebloque { get; set; }
            public double RecompensePoints { get; set; }

            public bool EstSecret { get; set; }
        }

        // ---------------- SUCCÈS (ACHIEVEMENTS) ----------------
        public static List<Succes> ListeSucces = new List<Succes>
{
    new Succes { Id = "CLIC_1", Titre = "Premier sang", Description = "Clique pour la première fois.", RecompensePoints = 10, EstDebloque = false, EstSecret = false },
    new Succes { Id = "ECO_1", Titre = "Capitaliste", Description = "Accumule 1 000 AuraPoints.", RecompensePoints = 500, EstDebloque = false, EstSecret = false },
    
    new Succes { Id = "SNAKE_1", Titre = "Reptile", Description = "Atteins un score de 10 au Snake.", RecompensePoints = 200, EstDebloque = false, EstSecret = false },
    
    new Succes { Id = "COMBAT_1", Titre = "Guerrier", Description = "Gagne ton premier combat.", RecompensePoints = 1000, EstDebloque = false, EstSecret = false },
    new Succes { Id = "LVL_10", Titre = "Niveau 10", Description = "Atteins le niveau 10.", RecompensePoints = 2000, EstDebloque = false, EstSecret = false },
    
    new Succes { Id = "SECRET_1", Titre = "Mode Brainrot", Description = "Accumule 1 000 000 points.", RecompensePoints = 50000, EstDebloque = false, EstSecret = true }
};

        private static void VerifierSucces()
        {
            foreach (var succes in ListeSucces)
            {
                if (succes.EstDebloque) continue;

                switch (succes.Id)
                {
                    case "CLIC_1":
                        if (points > 0) succes.EstDebloque = true;
                        break;
                    case "COMBAT_1":
                        break;
                    case "LVL_10":
                        if (Niveau >= 10) succes.EstDebloque = true;
                        break;
                    case "SECRET_1":
                        if (points >= 1000000) succes.EstDebloque = true;
                        break;
                        
                }

                if (succes.EstDebloque) points += succes.RecompensePoints;
            }
        }

        // ---------------- Musique ----------------
        public static List<string> MusiquesDisponibles { get; set; } = new();
        public static HashSet<string> MusiquesActives { get; set; } = new();

        public static event Action OnMusicChanged;
    }
}