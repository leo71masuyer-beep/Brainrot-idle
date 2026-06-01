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
            foreach (var p in participants)
            {
                if (p.PointsDeVie <= 0) continue;

                p.JaugeAction += p.VitesseAttaque;

                if (p.JaugeAction >= 100)
                {
                    FaireAttaquer(p);
                    p.JaugeAction -= 100;
                }
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
                switch (vague)
                {
                    case 1:
                        AddEnnemisAleatoire(
                            new Personnage("Petit Lutin", 20, 3, 1, 15, 0, 0, 20, false, 5, 5),
                            new Personnage("Lutin Guerrier", 60, 10, 5, 8, 5, 50, 60, false, 20, 25),
                            new Personnage("Lutin Enragé", 150, 20, 8, 12, 10, 50, 150, false, 50, 50)
                        );
                        ennemisDeLaVague.Add(new Personnage("Petit Lutin 2", 20, 3, 1, 15, 0, 0, 20, false, 5, 5));
                        break;

                    case 2:
                        ennemisDeLaVague.Add(new Personnage("Lutin Guerrier", 60, 10, 5, 8, 5, 50, 60, false, 20, 25));
                        ennemisDeLaVague.Add(new Personnage("Petit Lutin", 20, 3, 1, 15, 0, 0, 20, false, 5, 5));
                        break;

                    case 3:
                        ennemisDeLaVague.Add(new Personnage("Lutin Guerrier 1", 60, 10, 5, 8, 5, 50, 60, false, 20, 25));
                        ennemisDeLaVague.Add(new Personnage("Lutin Guerrier 2", 60, 10, 5, 8, 5, 50, 60, false, 20, 25));
                        break;

                    case 4:
                        ennemisDeLaVague.Add(new Personnage("Petit Lutin 1", 20, 3, 1, 15, 0, 0, 20, false, 5, 5));
                        ennemisDeLaVague.Add(new Personnage("Petit Lutin 2", 20, 3, 1, 15, 0, 0, 20, false, 5, 5));
                        ennemisDeLaVague.Add(new Personnage("Petit Lutin 3", 20, 3, 1, 15, 0, 0, 20, false, 5, 5));
                        ennemisDeLaVague.Add(new Personnage("Lutin Guerrier", 60, 10, 5, 8, 5, 50, 60, false, 20, 25));
                        break;

                    case 5:
                        ennemisDeLaVague.Add(new Personnage("Boss Lutin", 300, 25, 10, 10, 10, 50, 300, false, 150, 200));
                        break;

                    default:
                        ennemisDeLaVague.Add(new Personnage($"Lutin Enragé", 150, 20, 8, 12, 10, 50, 150, false, 50, 50));
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