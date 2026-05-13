using Brainrot_idle.Game.Combatgame.model;
using MonJeuCombat.Games.CombatGame.Logic;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading; // Indispensable pour le Timer

namespace Brainrot_idle.view
{
    public partial class MonJeuCombatFrame : Page
    {
        private GestionnaireCombat _gc;
        private Personnage _monHeros;
        private int _niveauActuel = 1;
        private int _vagueActuelle = 1;

        // NOUVEAU : Le chronomètre du jeu
        private DispatcherTimer _timerCombat;

        public MonJeuCombatFrame()
        {
            InitializeComponent();
            InitialiserCombat();
        }

        private void InitialiserCombat()
        {
            _gc = new GestionnaireCombat();
            _monHeros = new Personnage("Héros", 100, 20, 5, 15, 10, 50, 100, true);

            _gc.ChargerVague(1, 1);
            _gc.PreparerCombat(_monHeros, _gc.ennemisDeLaVague);

            // --- CONFIGURATION DU MOTEUR DU JEU ---
            _timerCombat = new DispatcherTimer();
            // Le timer se déclenche toutes les 50 millisecondes (20 fois par seconde)
            _timerCombat.Interval = TimeSpan.FromMilliseconds(50);
            _timerCombat.Tick += TimerCombat_Tick; // On lui dit quoi faire à chaque "tic"

            MettreAJourInterface();

            // On lance le combat automatique !
            _timerCombat.Start();
        }

        // NOUVEAU : La boucle principale du jeu
        private void TimerCombat_Tick(object sender, EventArgs e)
        {
            // 1. On fait avancer le temps (les jauges montent, les attaques partent)
            _gc.ExecuterTick();

            // 2. On met à jour l'écran
            MettreAJourInterface();

            // 3. On vérifie si quelqu'un a gagné ou perdu
            VerifierFinDeVague();
        }

        private void VerifierFinDeVague()
        {
            if (_monHeros.PointsDeVie <= 0)
            {
                _timerCombat.Stop(); // On arrête le temps
                MessageBox.Show("Vous êtes mort !");
            }
            else if (!_gc.participants.Any(p => !p.EstJoueur && p.PointsDeVie > 0))
            {
                _timerCombat.Stop(); // On met pause le temps de charger la suite
                MessageBox.Show($"Vague {_vagueActuelle} terminée ! Butin en cours : {_gc.OrCumule} Or");

                _vagueActuelle++;
                _gc.ChargerVague(_niveauActuel, _vagueActuelle);

                if (_gc.ennemisDeLaVague.Count > 0)
                {
                    _gc.PreparerCombat(_monHeros, _gc.ennemisDeLaVague);
                    MettreAJourInterface();
                    _timerCombat.Start(); // On relance le temps pour la vague suivante !
                }
                else
                {
                    MessageBox.Show("NIVEAU TERMINÉ ! Vous rentrez à la base.");
                }
            }
        }

        private void MettreAJourInterface()
        {
            int pvHerosAffichage = (int)_monHeros.PointsDeVie;
            BarrePVHeros.Value = pvHerosAffichage;
            TxtStatsHeros.Text = $"PV: {pvHerosAffichage} / {_monHeros.PointsDeVieMax}";

            var ennemi = _gc.participants.FirstOrDefault(p => !p.EstJoueur && p.PointsDeVie > 0);

            if (ennemi != null)
            {
                int pvEnnemiAffichage = (int)ennemi.PointsDeVie;
                TxtNomEnnemi.Text = ennemi.Nom;
                BarrePVEnnemi.Maximum = ennemi.PointsDeVieMax;
                BarrePVEnnemi.Value = pvEnnemiAffichage;
                TxtStatsEnnemi.Text = $"PV: {pvEnnemiAffichage} / {ennemi.PointsDeVieMax}";
            }
            else
            {
                TxtNomEnnemi.Text = "Aucun ennemi";
                BarrePVEnnemi.Value = 0;
                TxtStatsEnnemi.Text = "Mort !";
            }
        }

    }
}