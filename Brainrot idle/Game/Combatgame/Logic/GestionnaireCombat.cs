using Brainrot_idle.Game.Combatgame.model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonJeuCombat.Games.CombatGame.Logic
{
    public class GestionnaireCombat
    {
        public List<Personnage> participants = new List<Personnage>();

        public int OrCumule { get; private set; }
        public int ExpCumule { get; private set; }

        public void PreparerCombat(Personnage heros, List<Personnage> ennemisDeLaVague)
        {
            participants.Clear();
            OrCumule = 0;
            ExpCumule = 0;

            participants.Add(heros);
            participants.AddRange(ennemisDeLaVague);

            participants = participants.OrderByDescending(p => p.VitesseAttaque).ToList();
        }

        // 1. NOUVELLE MÉTHODE : Le temps qui passe
        public void ExecuterTick()
        {
            // On regarde chaque personnage sur le terrain
            foreach (var p in participants)
            {
                // Les morts ne font rien
                if (p.PointsDeVie <= 0) continue;

                // La jauge se remplit avec la vitesse (ex: si Vitesse = 10, ça ajoute 10 à chaque fois)
                // On peut multiplier par un petit chiffre plus tard si ça va trop vite
                p.JaugeAction += p.VitesseAttaque;

                // Si la jauge déborde (atteint ou dépasse 100)
                if (p.JaugeAction >= 100)
                {
                    FaireAttaquer(p);

                    // On retire 100 à la jauge au lieu de la mettre à 0
                    // Comme ça, s'il était à 105, il lui reste 5 pour la prochaine attaque (il ne perd pas de temps !)
                    p.JaugeAction -= 100;
                }
            }

            VerifierFinDeCombat();
        }

        // 2. NOUVELLE MÉTHODE : L'action de frapper
        private void FaireAttaquer(Personnage attaquant)
        {
            Personnage cible = null;

            if (attaquant.EstJoueur)
            {
                // Le joueur cible le premier ennemi vivant
                cible = participants.FirstOrDefault(x => !x.EstJoueur && x.PointsDeVie > 0);
            }
            else
            {
                // L'ennemi cible le joueur
                cible = participants.FirstOrDefault(x => x.EstJoueur && x.PointsDeVie > 0);
            }

            // S'il a trouvé une cible valide, il frappe !
            if (cible != null)
            {
                double degats = attaquant.CalculerDegatsSortants();
                cible.RecevoirDegats(degats);

                // On vérifie s'il l'a tué pour récupérer le loot
                if (cible.PointsDeVie <= 0 && !cible.EstJoueur)
                {
                    OrCumule += cible.GenererOrAleatoire();
                    ExpCumule += cible.GenererExpAleatoire();
                }
            }
        }

        public void VerifierFinDeCombat()
        {
            bool joueurVivant = participants.Any(x => x.EstJoueur && x.PointsDeVie > 0);
            bool ennemisVivants = participants.Any(x => !x.EstJoueur && x.PointsDeVie > 0);

            if (!joueurVivant)
            {
                Console.WriteLine("Défaite... Vous rentrez avec votre butin.");
                TerminerCombat();
            }
            else if (!ennemisVivants)
            {
                Console.WriteLine("Victoire ! Vague terminée.");
                TerminerCombat();
            }
        }

        private void TerminerCombat()
        {
            Console.WriteLine($"Butin total : {OrCumule} Or et {ExpCumule} Exp.");
        }

        // Ta liste globale est bien là
        public List<Personnage> ennemisDeLaVague = new List<Personnage>();

        public void ChargerVague(int niveau, int vague)
        {
            ennemisDeLaVague.Clear();

            if (niveau == 1)
            {
                if (vague == 1)
                {
                    ennemisDeLaVague.Add(new Personnage("Lutin", 30, 5, 2, 10, 0, 0, 30, false, 10, 15));
                }
                if (vague == 2)
                {
                    ennemisDeLaVague.Add(new Personnage("Lutin A", 30, 5, 2, 10, 5, 50, 30, false, 10, 15));
                    ennemisDeLaVague.Add(new Personnage("Lutin B", 30, 5, 2, 10, 5, 50, 30, false, 10, 15));
                }
                if (vague == 3)
                {
                    ennemisDeLaVague.Add(new Personnage("LutinBoss", 300, 5, 2, 10, 5, 50, 300, false, 100, 150));
                }
            }
        }
    }
}