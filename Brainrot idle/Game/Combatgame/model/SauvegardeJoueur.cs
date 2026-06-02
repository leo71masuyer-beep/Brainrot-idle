using System;
using System.Collections.Generic;
using System.Text;

namespace Brainrot_idle.Game.Combatgame.model
{
    public static class SauvegardeJoueur
    {
        public static int OrTotal { get; set; } = 0;
        public static int ExpTotal { get; set; } = 0;
        public static int PierresTotal { get; set; } = 0;
        public static int DiamantsTotal { get; set; } = 0;
        public static int NiveauHeros { get; set; } = 1;

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
                if (NiveauHeros % 5 == 0) PierresTotal += 3;
                else PierresTotal += 1;
                xpRequise = CalculerXpRequise(NiveauHeros);
            }
        }
    }
}
