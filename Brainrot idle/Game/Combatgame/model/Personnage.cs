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
                    // 1. On additionne TOUS les flats (Base + Bonus fixe + Passifs GigaChad)
                    double atkFlat = _Attaque + SauvegardeJoueur.AttaqueBonusFlat;
                    atkFlat += ObtenirFlatsAttaque();

                    // 2. On calcule TOUS les multiplicateurs
                    double atkMultiplicateur = 1f + (SauvegardeJoueur.AttaqueBonus / 100f);
                    atkMultiplicateur *= ObtenirMultiplicateursAttaque();

                    // 3. On multiplie le tout
                    return atkFlat * atkMultiplicateur;
                }
                else
                {
                    return _Attaque;
                }
            }
            set { _Attaque = value; }
        }

        private double _Defense;
        public double Defense
        {
            get
            {
                if (this.EstJoueur)
                {
                    // FIX : On utilise DefenseBonusFlat pour les +10 (Pecs, Lombaires, etc.)
                    double defFlat = _Defense + SauvegardeJoueur.DefenseBonusFlat;

                    // On calcule TOUS les multiplicateurs (Corp Gigachad, Chad Ultime...)
                    double defMultiplicateur = 1f + (SauvegardeJoueur.DefenseBonus / 100f);
                    defMultiplicateur *= ObtenirMultiplicateursDefense();

                    // On multiplie le tout
                    return defFlat * defMultiplicateur;
                }
                else
                {
                    return _Defense;
                }
            }
            set { _Defense = value; }
        }

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

        #region ================= CALCUL DES PASSIFS (FLATS ET MULTIPLICATEURS) =================

        // --- ATTAQUE ---

        // C'est cette fonction qui manquait pour lier la défense à l'attaque !
        private double ObtenirFlatsAttaque()
        {
            double flat = 0;
            // Remplacement : Mega Chad écrase Giga Chad
            if (SauvegardeJoueur.LvlMegaChad > 0)
            {
                flat += (this.Defense / 1.5f);
            }
            else if (SauvegardeJoueur.LvlGigaChad > 0)
            {
                flat += (this.Defense / 2f);
            }
            return flat;
        }

        private double ObtenirMultiplicateursAttaque()
        {
            double multiplicateur = 1f;
            // Muscle Giga Chad
            if (SauvegardeJoueur.LvlMuscleGigaChad > 0 && this.PointsDeVie < (this.PointsDeVieMax / 2f))
            {
                multiplicateur *= 1.50f; // Multiplie par 1.5 (+50%)
            }
            return multiplicateur;
        }

        // --- DEFENSE ---

        private double ObtenirMultiplicateursDefense()
        {
            double multiplicateur = 1f;

            // Corp de Gigachad (s'ajoute au multiplicateur de base)
            if (SauvegardeJoueur.LvlCorpGigachad > 0)
            {
                multiplicateur += 0.30f;
            }

            // Chad Ultime (Multiplie tout par 3)
            if (SauvegardeJoueur.LvlChadUltime > 0 && SauvegardeJoueur.LvlMuscleGigaChad > 0 && this.PointsDeVie < (this.PointsDeVieMax / 2f))
            {
                multiplicateur *= 3f;
            }

            return multiplicateur;
        }

        #endregion

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
            int pourcentage = _rng.Next(80, 121);
            return (OrDeBase * pourcentage) / 100;
        }

        public int GenererExpAleatoire()
        {
            int pourcentage = _rng.Next(90, 111);
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
                    PointsDeVie += valeur;
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