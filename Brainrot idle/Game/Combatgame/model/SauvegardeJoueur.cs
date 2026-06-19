using System;
using System.Collections.Generic;
using System.Text;

namespace Brainrot_idle.Game.Combatgame.model
{
    public static class SauvegardeJoueur
    {
        public static int OrTotal { get; set; } = 0;
        public static int ExpTotal { get; set; } = 0;
        public static int Pierres { get; set; } = 0;
        public static int DiamantsTotal { get; set; } = 0;
        public static int NiveauHeros { get; set; } = 1;

        // ==========================================
        // STATS BONUS (Issues de l'arbre)
        // ==========================================
        public static int AttaqueBonusFlat { get; set; } = 0;
        public static float AttaqueBonus { get; set; } = 0f;

        public static double DefenseBonusFlat { get; set; } = 0;
        public static double DefenseBonus { get; set; } = 0;

        public static float ChanceCritique { get; set; } = 0f;
        public static float DegatCritique { get; set; } = 0f;

        // ==========================================
        // STATS BONUS GACHA (Permanentes)
        // ==========================================
        public static double PvBonusFlat { get; set; } = 0;
        public static double VitesseBonusFlat { get; set; } = 0;

        // ==========================================
        // RELIQUES UNIQUES MYTHIQUES (Gacha)
        // ==========================================
        public static bool PassifEpeeDeLAnomalie { get; set; } = false;
        public static bool PassifArmureEtoilee { get; set; } = false;

        // ==========================================
        // Variables pour sauvegarder l'état de l'arbre de compétences
        // ==========================================
        public static int LvlBase = 0;
        public static int LvlSuperAura = 0;
        public static int LvlMegaAura = 0;
        public static int LvlSigmaAura = 0;
        public static int LvlSigmaBoy = 0;
        public static int Lvltungtungsahur = 0;
        public static int Lvltralala = 0;
        public static int Lvlfrulifrula = 0;
        public static int Lvlbombardilo = 0;
        public static int Lvludindindindun = 0;
        public static int Lvlpatapim = 0;
        public static int Lvlbananini = 0;
        public static int Lvllarila = 0;
        public static int Lvlalliance = 0;

        // Branche Giga Chad
        public static int LvlGigaChad = 0;
        public static int LvlPecsChad = 0;
        public static int LvlLombaireChad = 0;
        public static int LvlTricepsChad = 0;
        public static int LvlCorpGigachad = 0;
        public static int LvlMegaChad = 0;
        public static int LvlMuscleGigaChad = 0;
        public static int LvlQuadricepsChad = 0;
        public static int LvlAbdoChad = 0;
        public static int LvlChadUltime = 0;

        // ==========================================
        // PASSIFS UNIQUE / SPÉCIAUX
        // ==========================================
        public static bool PassifAllianceActif { get; set; } = false;

        public static int CalculerXpRequise(int niveau)
        {
            return (int)(50 * Math.Pow(1.1, niveau - 1));
        }

        public static void AjouterExp(int montantGagne)
        {
            ExpTotal += montantGagne;
            int xpRequise = CalculerXpRequise(NiveauHeros);

            while (ExpTotal >= xpRequise)
            {
                ExpTotal -= xpRequise;
                NiveauHeros++;
                if (NiveauHeros % 5 == 0) Pierres += 5;
                else Pierres += 1;
                xpRequise = CalculerXpRequise(NiveauHeros);
            }
        }
    }
}