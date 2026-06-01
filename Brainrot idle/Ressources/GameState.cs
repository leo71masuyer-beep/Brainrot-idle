using System;
using System.Collections.Generic;
using System.Text;

namespace Brainrot_idle.Ressources
{
    public static class GameState
    {
        public static double points = 0;
        public static double auraParSeconde = 0;

        public static double clicsCetteSeconde = 0;

        public static int[] nbAmeliorations = new int[10];

        public static double[] prixAmeliorations =
        {
            10,
            100,
            1_000,
            10_000,
            100_000,
            1_000_000,
            10_000_000,
            100_000_000,
            1_000_000_000,
            10_000_000_000
        };

        public static int MeilleurScoreSnake { get; set; } = 0;
    }
}
