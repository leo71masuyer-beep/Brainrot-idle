using System;
using System.Collections.Generic;
using System.Text;

namespace Brainrot_idle.Game.Combatgame.model
{
    public class Personnage
    {
        // Stats de base
        public string Nom { get; private set; }
        public double PointsDeVie { get; private set; }
        public double PointsDeVieMax { get; private set; }
        public double Attaque { get; private set; }
        public double Defense { get; private set; }
        public double Vitesse { get; private set; }
        public int ChanceCritique { get; private set; }
        public int DegatCritique { get; private set; }
        public bool EstJoueur { get; private set; }
        private static Random _rng = new Random();

        // Le constructeur : c'est ce qui permet de créer un perso avec des stats précises
        public Personnage(string nom, double pv, double atk , double def, double vit, int pourcentageCrit, int degCrit, double pvmax, bool EstJ)
        {
            Nom = nom;
            PointsDeVie = pv;
            PointsDeVieMax = pvmax;
            Attaque = atk;
            Defense = def;
            Vitesse = vit;
            ChanceCritique = pourcentageCrit;
            DegatCritique = degCrit;
            EstJoueur = EstJ;
        }

        public void RecevoirDegats(double montantBrut, double PV)
        {
            // Calcul de la defense
            double coefficient = 100 / (100 + Defense);
            double degatsFinaux = montantBrut * coefficient;

            // On applique aux PV
            PV -= degatsFinaux;

            // Sécurité
            if (PV < 0) PV = 0;
        }

        public double CalculerDegatsSortants()
        {
            // 1. Tirage au sort (entre 1 et 101 car le max est exclus)
            int jet = _rng.Next(1, 101);

            double degatsFinaux = Attaque;

            // 2. Test du coup critique
            if (jet >= ChanceCritique)
            {
                degatsFinaux = degatsFinaux * (1+DegatCritique / 100.0);
                
                Console.WriteLine("Coup Critique !");
            }

            return degatsFinaux;
        }
    }
}
