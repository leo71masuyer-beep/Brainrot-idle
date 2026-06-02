using Brainrot_idle.Game.Combatgame.model;
using MonJeuCombat.Games.CombatGame.Logic;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Brainrot_idle.view
{
    public partial class MonJeuCombatFrame : Page
    {
        private GestionnaireCombat _gc;
        private Personnage _monHeros;
        private int _niveauActuel = 1;
        private int _vagueActuelle = 1;

        private DispatcherTimer _timerCombat;

        // NOUVEAU : On exige un Personnage dans les parenthèses du constructeur
        public MonJeuCombatFrame(Personnage herosEnvoyeDepuisLeMenu)
        {
            InitializeComponent();

            // On sauvegarde le héros qu'on vient de recevoir dans notre variable locale
            _monHeros = herosEnvoyeDepuisLeMenu;

            InitialiserCombat();
        }

        private void InitialiserCombat()
        {
            _gc = new GestionnaireCombat();

            // On a supprimé la création du héros ici, puisqu'on l'a déjà !

            _gc.ChargerVague(1, 1);
            _gc.PreparerCombat(_monHeros, _gc.ennemisDeLaVague);

            _timerCombat = new DispatcherTimer();
            _timerCombat.Interval = TimeSpan.FromMilliseconds(50);
            _timerCombat.Tick += TimerCombat_Tick;

            MettreAJourInterface();
            _timerCombat.Start();
        }

        private void TimerCombat_Tick(object sender, EventArgs e)
        {
            _gc.ExecuterTick();
            MettreAJourInterface();
            VerifierFinDeVague();
        }

        private void VerifierFinDeVague()
        {
            if (_monHeros.PointsDeVie <= 0)
            {
                _timerCombat.Stop();
            }
            else if (!_gc.participants.Any(p => !p.EstJoueur && p.PointsDeVie > 0))
            {
                _timerCombat.Stop();

                _vagueActuelle++;
                _gc.ChargerVague(_niveauActuel, _vagueActuelle);

                if (_gc.ennemisDeLaVague.Count > 0)
                {
                    _gc.PreparerCombat(_monHeros, _gc.ennemisDeLaVague);
                    MettreAJourInterface();
                    _timerCombat.Start();
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
            TxtOrGagne.Text = $"{_gc.OrCumule} Or";
            TxtExpGagne.Text = $"{_gc.ExpCumule} Exp";
        }
        private void BtnArreterCombat_Click(object sender, RoutedEventArgs e)
        {
            _timerCombat.Stop();

            // 1. On ajoute l'or 
            SauvegardeJoueur.OrTotal += _gc.OrCumule;

            // 2. ON UTILISE UNIQUEMENT LA FONCTION POUR L'XP (Elle fait le calcul et le Level Up toute seule)
            SauvegardeJoueur.AjouterExp(_gc.ExpCumule);

            _monHeros.SoignerTotalement();

            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }   
    }
}