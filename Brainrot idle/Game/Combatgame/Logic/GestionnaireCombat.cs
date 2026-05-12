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

        // Ajout d'un dé pour le choix aléatoire des monstres
        private static Random _rng = new Random();

        public void PreparerCombat(Personnage heros, List<Personnage> ennemisDeLaVague)
        {
            participants.Clear();
            OrCumule = 0;
            ExpCumule = 0;

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

        // === LE NOUVEAU SYSTÈME DE VAGUES PAR POINTS ===
        public void ChargerVague(int niveau, int vague)
        {
            ennemisDeLaVague.Clear();

            if (niveau == 1)
            {
                // 1. Calcul du budget. Plus la vague est haute, plus on a de points !
                // Exemple : Vague 1 = 15 pts, Vague 2 = 25 pts, Vague 10 = 105 pts.
                int budgetPoints = 5 + (vague * 10);
                int numeroEnnemi = 1;

                // 2. Le jeu fait ses courses tant qu'il lui reste des points
                while (budgetPoints > 0)
                {
                    // On fait la liste de ce que le jeu PEUT acheter avec son budget actuel
                    List<string> monstresAchetables = new List<string>();

                    if (budgetPoints >= 5) monstresAchetables.Add("PetitLutin");   // Coûte 5 points
                    if (budgetPoints >= 15) monstresAchetables.Add("LutinGuerrier"); // Coûte 15 points
                    if (budgetPoints >= 50 && vague % 5 == 0) monstresAchetables.Add("BossLutin"); // Coûte 50 pts, dispo que toutes les 5 vagues

                    // Si on n'a même plus assez pour le monstre le moins cher (5 pts), on arrête les courses
                    if (monstresAchetables.Count == 0) break;

                    // 3. On choisit un monstre au hasard parmi ceux qu'on peut s'offrir
                    int indexAleatoire = _rng.Next(monstresAchetables.Count);
                    string choix = monstresAchetables[indexAleatoire];
                    //string nom, double pv, double atk, double def, double vit, int pourcentageCrit, int degCrit, double pvmax, bool EstJ, int orBase = 0, int expBase = 0
                    // 4. On crée le monstre, on l'ajoute, et on DÉDUIT son prix du budget
                    if (choix == "PetitLutin")
                    {
                        ennemisDeLaVague.Add(new Personnage($"Petit Lutin {numeroEnnemi}", 20, 3, 1, 15, 0, 0, 20, false, 5, 5));
                        budgetPoints -= 5;
                    }
                    else if (choix == "LutinGuerrier")
                    {
                        ennemisDeLaVague.Add(new Personnage($"Lutin Guerrier {numeroEnnemi}", 60, 10, 5, 8, 5, 50, 60, false, 20, 25));
                        budgetPoints -= 15;
                    }
                    else if (choix == "BossLutin")
                    {
                        ennemisDeLaVague.Add(new Personnage($"Boss Lutin", 300, 25, 10, 10, 10, 50, 300, false, 150, 200));
                        budgetPoints -= 50;
                    }

                    numeroEnnemi++; // Pour donner un numéro différent (Ex: Petit Lutin 1, Petit Lutin 2...)
                }
            }
        }
    }
}