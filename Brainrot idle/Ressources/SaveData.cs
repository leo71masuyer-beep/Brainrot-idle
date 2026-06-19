using System.Collections.Generic;

namespace Brainrot_idle.Ressources
{
    public class SaveData
    {
        public double Points { get; set; }
        public double AuraParSeconde { get; set; }

        public int[] NbAmeliorations { get; set; }

        public double[] PrixAmeliorations { get; set; }

        public int MeilleurScoreSnake { get; set; }
        public int MeilleurScoreMorpion { get; set; }

        public bool IsSnakeDebloque { get; set; }
        public bool IsCombatDebloque { get; set; }
        public bool IsMorpionDebloque { get; set; }

        public double XpActuelle { get; set; }
        public double XpRequise { get; set; }
        public int Niveau { get; set; }
        public int PointsDeCompetence { get; set; }

        public int NiveauApprenti { get; set; }
        public double MultiplicateurXp { get; set; }

        public int NiveauCliqueur { get; set; }
        public double MultiplicateurAuraParClic { get; set; }

        public double AuraBonus { get; set; }
        public double AuraBonusFlat { get; set; }

        public List<string> MusiquesActives { get; set; }
    }
}