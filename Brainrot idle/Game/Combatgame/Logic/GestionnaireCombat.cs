using Brainrot_idle.Game.Combatgame.model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonJeuCombat.Games.CombatGame.Logic
{
    public class GestionnaireCombat
    {
        public event Action<Personnage> OnAttaque;

        public List<Personnage> participants = new List<Personnage>();

        public int OrCumule { get; private set; }
        public int ExpCumule { get; private set; }

        private static Random _rng = new Random();

        public void PreparerCombat(Personnage heros, List<Personnage> ennemisDeLaVague)
        {
            participants.Clear();

            participants.Add(heros);
            participants.AddRange(ennemisDeLaVague);

            participants = participants.OrderByDescending(p => p.VitesseAttaque).ToList();
        }

        public void ExecuterTick()
        {
            // 1. On charge les jauges de TOUS les personnages vivants en même temps
            foreach (var p in participants)
            {
                if (p.PointsDeVie > 0)
                {
                    p.JaugeAction += p.VitesseAttaque;
                }
            }

            // 2. On récupère ceux qui ont atteint 100 de jauge, et on les TRIE 
            // Celui qui a la plus grande jauge (le plus rapide) est placé en premier
            var pretsAAttaquer = participants
                .Where(p => p.PointsDeVie > 0 && p.JaugeAction >= 100)
                .OrderByDescending(p => p.JaugeAction)
                .ToList();

            // 3. On les fait attaquer dans le bon ordre
            foreach (var attaquant in pretsAAttaquer)
            {
                // SÉCURITÉ CRUCIALE : Si un monstre a été tué par le héros 
                // juste avant dans cette même milliseconde, il ne peut plus attaquer !
                if (attaquant.PointsDeVie <= 0) continue;

                FaireAttaquer(attaquant);
                attaquant.JaugeAction -= 100;
            }

            VerifierFinDeCombat();
        }

        private void FaireAttaquer(Personnage attaquant)
        {
            Personnage cible = null;

            if (attaquant.EstJoueur)
            {
                cible = participants.FirstOrDefault(x => !x.EstJoueur && x.PointsDeVie > 0);
            }
            else
            {
                cible = participants.FirstOrDefault(x => x.EstJoueur && x.PointsDeVie > 0);
            }

            if (cible != null)
            {
                // On déclenche l'animation juste avant d'infliger les dégâts
                OnAttaque?.Invoke(attaquant);

                double degats = attaquant.CalculerDegatsSortants();
                cible.RecevoirDegats(degats);

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
            }
            else if (!ennemisVivants)
            {
                Console.WriteLine("Victoire ! Vague terminée.");
            }
        }

        public List<Personnage> ennemisDeLaVague = new List<Personnage>();

        public void AddEnnemisAleatoire(Personnage monstreCommun, Personnage monstreRare, Personnage monstreLegendaire)
        {
            int index = _rng.Next(1, 10001);

            if (index <= 7500)
            {
                ennemisDeLaVague.Add(monstreCommun);
            }
            else if (index <= 9500)
            {
                ennemisDeLaVague.Add(monstreRare);
            }
            else
            {
                ennemisDeLaVague.Add(monstreLegendaire);
            }
        }

        public void ChargerVague(int niveau, int vague)
        {
            ennemisDeLaVague.Clear();

            if (niveau == 1)
            {
                //string nom, double pv, double atk, double def, double vit, int pourcentageCrit, int degCrit, double pvmax, bool EstJ, int orBase = 0, int expBase = 0
                switch (vague)
                {
                    case 1:
                        ennemisDeLaVague.Add(new Personnage("CappuccinoAssassino", 100, 5, 5, 5, 0, 0, 100, false, 20, 10));
                        break;

                    case 2:
                        ennemisDeLaVague.Add(new Personnage("BallerinoLololo", 150, 8, 5, 5, 0, 0, 150, false, 30, 14));
                        break;

                    case 3:
                        ennemisDeLaVague.Add(new Personnage("BobritoBandito", 300, 10, 2, 10, 0, 0, 150, false, 45, 20));
                        break;

                    case 4:
                        ennemisDeLaVague.Add(new Personnage("BallerinaCappuccina", 125, 14, 20, 15, 0, 0, 75, false, 30, 20));
                        break;

                    case 5:
                        ennemisDeLaVague.Add(new Personnage($"tungtungGod", 500, 26, 5, 10, 10, 15, 500, false, 168, 84));
                        break;

                    default:
                        ennemisDeLaVague.Add(new Personnage($"tungtungGod", 500, 26, 5, 10, 10, 15, 500, false, 168, 84));
                        break;
                }
            }
            else if (niveau == 2)
            {
                switch (vague)
                {
                    case 1:
                        ennemisDeLaVague.Add(new Personnage("FDP", 20, 10000, 1, 15, 0, 0, 20, false, 5, 5));
                        break;

                    default:
                        ennemisDeLaVague.Add(new Personnage($"Lutin Enragé", 150, 20, 8, 12, 10, 50, 150, false, 50, 50));
                        break;
                }
            }
        }
    }
}