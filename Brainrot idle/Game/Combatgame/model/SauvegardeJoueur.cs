using System;
using System.Collections.Generic;
using System.Text;

namespace Brainrot_idle.Game.Combatgame.model
{
    public static class SauvegardeJoueur
    {
        public static int OrTotal { get; set; } = 5000;
        public static int ExpTotal { get; set; } = 0;
        public static int Pierres { get; set; } = 0;
        public static int DiamantsTotal { get; set; } = 0;
        public static int NiveauHeros { get; set; } = 1;

        // ==========================================
        // STATS BONUS (Issues de l'arbre)
        // ==========================================
        public static int AttaqueBonusFlat { get; set; } = 0;
        public static float AttaqueBonus { get; set; } = 1f;
        public static float ChanceCritique { get; set; } = 0f;
        public static float DegatCritique { get; set; } = 0f;

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

        // ==========================================
        // PASSIFS UNIQUE / SPÉCIAUX
        // ==========================================
        public static bool PassifAllianceActif { get; set; } = false;



        // --- SAUVEGARDE DE L'ARBRE DE COMPÉTENCES ---
        // Nœud de départ
        public static int CompBaseAttaque { get; set; } = 0; // Max 5

        // Nœuds de spécialisation (Choix mutuellement exclusif)
        public static int CompVoieBrainrot { get; set; } = 0; // Max 1
        public static int CompVoieGigachad { get; set; } = 0; // Max 1

        public static int CalculerXpRequise(int niveau)
        {
            return (int)(50 * Math.Pow(1.5, niveau - 1));
        }

        public static void AjouterExp(int montantGagne)
        {
            ExpTotal += montantGagne;
            int xpRequise = CalculerXpRequise(NiveauHeros);

            while (ExpTotal >= xpRequise)
            {
                ExpTotal -= xpRequise;
                NiveauHeros++;
                if (NiveauHeros % 5 == 0) Pierres += 3;
                else Pierres += 1;
                xpRequise = CalculerXpRequise(NiveauHeros);
            }
        }
    }
}
