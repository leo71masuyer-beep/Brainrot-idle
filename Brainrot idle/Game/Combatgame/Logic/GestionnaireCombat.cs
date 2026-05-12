using Brainrot_idle.Game.Combatgame.model;
using MonJeuCombat.Games.CombatGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MonJeuCombat.Games.CombatGame.Logic
{
    public class GestionnaireCombat
    {
        // La liste qui contient tout le monde (Héros + Ennemis)
        public List<Personnage> participants = new List<Personnage>();

        public void PreparerCombat(Personnage heros, List<Personnage> ennemisDeLaVague)
        {
            // 1. On vide la liste au cas où il restait des gens du combat d'avant
            participants.Clear();

            // 2. On ajoute le héros
            participants.Add(heros);

            // 3. On ajoute tous les ennemis d'un coup
            participants.AddRange(ennemisDeLaVague);

            // 4. LE TRI PAR VITESSE
            // On demande à LINQ de trier par vitesse descendante (du plus grand au plus petit)
            participants = participants.OrderByDescending(p => p.Vitesse).ToList();
        }

        public void ExecuterTour()
        {
            foreach (var p in participants)
            {
                if (p.PV <= 0) continue;

                if (p.EstJoueur)
                {
                    Console.WriteLine("C'est à vous de jouer !");
                    // Plus tard, ici on arrêtera la boucle pour attendre ton clic
                }
                else
                {
                    // C'est un ENNEMI qui attaque
                    // Il calcule ses dégâts
                    double degats = p.CalculerDegatsSortants();

                    // Il attaque le héros (on doit le trouver dans la liste)
                    Personnage cible = participants.FirstOrDefault(x => x.EstJoueur);

                    if (cible != null)
                    {
                        cible.RecevoirDegats(degats);
                        Console.WriteLine($"{p.Nom} attaque le joueur et inflige {degats} dégâts !");
                    }
                }
            }
            VerifierFinDeCombat();
        }

        public List<Personnage> ennemisDeLaVague = new List<Personnage>();

        public void ChargerVague(int niveau, int vague)
        {
            ennemisActuels.Clear();
            //(     Nom | Pv | atk | def | vit | int pourcentageCrit | int degCrit | double pvmax | bool EstJ    )
            if (niveau == 1) // On est dans la forêt par exemple
            {
                if (vague == 1) 
                {
                    ennemisActuels.Add(new Personnage("Lutin", 30, 5, 2, 10, 0, 0, 30, false));
                }
                if (vague == 2) 
                {
                    ennemisActuels.Add(new Personnage("Lutin", 30, 5, 2, 10, 5, 50, 30, false));
                    ennemisActuels.Add(new Personnage("Lutin", 30, 5, 2, 10, 5, 50, 30, false));
                }
                if (vague == 4) { ennemisActuels.Add(new Personnage("LutinBoss", 300, 5, 2, 10, 5, 50, 300, false)); }
            }
            else if (niveau == 2) // On est dans la grotte
            {
                if (vague == 1) { /* Ajouter 2 chauve-souris */ }
            }
        }
    }
}
