using System;
using System.Collections.Generic;
using System.Text;

namespace Brainrot_idle.Game.Combatgame.model
{
    public class Personnage
    {
        public string Nom { get; private set; }
        public double PointsDeVie { get; private set; }
        public double PointsDeVieMax { get; private set; }
        private double _Attaque;

        public double Attaque
        {
            get
            {
                if (this.EstJoueur)
                {
                    double multiplicateurPercentage = 1f + (SauvegardeJoueur.AttaqueBonus / 100f);
                    return (_Attaque + SauvegardeJoueur.AttaqueBonusFlat) * multiplicateurPercentage;
                }
                else
                {
                    return _Attaque;
                }
            }
            set
            {
                _Attaque = value;
            }
        }
        public double Defense { get; private set; }
        public double VitesseAttaque { get; private set; }
        public int ChanceCritique { get; private set; }
        public int DegatCritique { get; private set; }
        public bool EstJoueur { get; private set; }
        public int OrDeBase { get; private set; }
        public int ExpDeBase { get; private set; }
        public double JaugeAction { get; set; } = 0;

        private static Random _rng = new Random();

        public Personnage(string nom, double pv, double atk, double def, double vit, int pourcentageCrit, int degCrit, double pvmax, bool EstJ, int orBase = 0, int expBase = 0)
        {
            Nom = nom;
            PointsDeVie = pv;
            PointsDeVieMax = pvmax;
            Attaque = atk;
            Defense = def;
            VitesseAttaque = vit;
            ChanceCritique = pourcentageCrit;
            DegatCritique = degCrit;
            EstJoueur = EstJ;
            OrDeBase = orBase;
            ExpDeBase = expBase;
        }

        public void RecevoirDegats(double montantBrut)
        {
            double coefficient = 100 / (100 + Defense);
            double degatsFinaux = montantBrut * coefficient;

            PointsDeVie -= degatsFinaux;

            if (PointsDeVie < 0) PointsDeVie = 0;
        }

        public double CalculerDegatsSortants()
        {
            int jet = _rng.Next(1, 101);
            double degatsFinaux = Attaque;

            // Si le jet est inférieur ou égal à la chance, c'est un crit (ex: jet 5 pour 20% de chance)
            if (jet <= ChanceCritique)
            {
                degatsFinaux = degatsFinaux * (1 + DegatCritique / 100.0);
                Console.WriteLine("Coup Critique !");
            }

            return degatsFinaux;
        }

        // Fonctions Gacha pour le loot
        public int GenererOrAleatoire()
        {
            int pourcentage = _rng.Next(80, 121); // Entre 80% et 120%
            return (OrDeBase * pourcentage) / 100;
        }

        public int GenererExpAleatoire()
        {
            int pourcentage = _rng.Next(90, 111); // Entre 90% et 110%
            return (ExpDeBase * pourcentage) / 100;
        }

        public void AmeliorerStatistique(string stat, double valeur)
        {
            switch (stat)
            {
                case "Attaque":
                    Attaque += valeur;
                    break;
                case "Sante":
                    PointsDeVieMax += valeur;
                    PointsDeVie += valeur; // On soigne aussi le joueur
                    break;
                case "Vitesse":
                    VitesseAttaque += valeur;
                    break;
                case "Critique":
                    ChanceCritique += (int)valeur;
                    break;
            }
        }

        public void SoignerTotalement()
        {
            PointsDeVie = PointsDeVieMax;
        }
    }
}
