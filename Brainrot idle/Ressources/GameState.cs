using System;
using System.Windows.Threading;

namespace Brainrot_idle.Ressources
{
    public static class GameState
    {
        // ---------------- DONNÉES DU JOUEUR ----------------
        public static double points = 0;
        public static double auraParSeconde = 0;
        public static double clicsCetteSeconde = 0;

        // ---------------- AMÉLIORATIONS ----------------
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

        // ---------------- MOTEUR IDLE (TIMER BACKGROUND) ----------------
        private static DispatcherTimer globalTimer;

        /// <summary>
        /// Lance le cœur économique du jeu. À appeler UNE SEULE FOIS (dans App.xaml.cs).
        /// </summary>
        public static void InitialiserTimerGlobal()
        {
            if (globalTimer == null)
            {
                globalTimer = new DispatcherTimer();
                globalTimer.Interval = TimeSpan.FromSeconds(1);
                globalTimer.Tick += GlobalTimer_Tick;
                globalTimer.Start();
            }
        }

        private static void GlobalTimer_Tick(object sender, EventArgs e)
        {
            // 1. Calcul du multiplicateur linéaire du Snake (+15% par point)
            double multiplicateurSnake = 1.0 + (MeilleurScoreSnake * 0.15);

            // 2. Application du boost sur la production passive globale
            double auraBoosteParSec = auraParSeconde * multiplicateurSnake;

            // 3. Ajout des points en tâche de fond (marche partout, même sur la page Snake !)
            points += auraBoosteParSec;

            // 4. Reset des clics de la seconde écoulée
            clicsCetteSeconde = 0;
        }
    }
}