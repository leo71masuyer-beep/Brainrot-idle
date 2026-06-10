using Brainrot_idle.Game.Combatgame.model;
using MonJeuCombat.Games.CombatGame.Logic;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Threading.Tasks;

namespace Brainrot_idle.view
{
    public partial class MonJeuCombatFrame2 : Page
    {
        private GestionnaireCombat _gc;
        private Personnage _monHeros;
        private int _niveauActuel = 2;
        private int _vagueActuelle = 1;

        private DispatcherTimer _timerCombat;

        public MonJeuCombatFrame2(Personnage herosEnvoyeDepuisLeMenu)
        {
            InitializeComponent();

            _monHeros = herosEnvoyeDepuisLeMenu;

            InitialiserCombat();
        }

        private void InitialiserCombat()
        {
            _gc = new GestionnaireCombat();

            _gc.OnAttaque += Gc_OnAttaque;

            _gc.ChargerVague(_niveauActuel, _vagueActuelle);
            _gc.PreparerCombat(_monHeros, _gc.ennemisDeLaVague);

            _timerCombat = new DispatcherTimer();
            _timerCombat.Interval = TimeSpan.FromMilliseconds(50);
            _timerCombat.Tick += TimerCombat_Tick;

            MettreAJourInterface();
            _timerCombat.Start();
        }

        private void Gc_OnAttaque(Personnage attaquant)
        {
            if (attaquant.EstJoueur)
            {
                // On envoie le nom de base du héros
                JouerAnimationAttaqueSprite(ImgHeros, "heros");
            }
            else
            {
                string nomBaseEnnemi = attaquant.Nom.ToLower().Replace(" ", "_");
                JouerAnimationAttaqueSprite(ImgEnnemi, nomBaseEnnemi);
            }
        }

        private async void JouerAnimationAttaqueSprite(Image imageAAnimer, string nomBase)
        {
            try
            {
                int vitesseAnimation = 80;

                // Frame d'attaque 1
                imageAAnimer.Source = new BitmapImage(new Uri($"pack://application:,,,/view/GameCombat/Ressources/Sprites/{nomBase}_atk1.png"));
                await Task.Delay(vitesseAnimation);

                // Frame d'attaque 2
                imageAAnimer.Source = new BitmapImage(new Uri($"pack://application:,,,/view/GameCombat/Ressources/Sprites/{nomBase}_atk2.png"));
                await Task.Delay(vitesseAnimation);

                // Frame d'attaque 3
                imageAAnimer.Source = new BitmapImage(new Uri($"pack://application:,,,/view/GameCombat/Ressources/Sprites/{nomBase}_atk3.png"));
                await Task.Delay(vitesseAnimation);

                // Retour à la position de repos (Idle)
                imageAAnimer.Source = new BitmapImage(new Uri($"pack://application:,,,/view/GameCombat/Ressources/Sprites/{nomBase}_idle.png"));
            }
            catch
            {
                // Si une frame manque dans le dossier, on ignore l'erreur pour ne pas faire crasher le jeu
            }
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

                try
                {
                    // Convertit "Petit Lutin" en "petit_lutin_idle.png"
                    string nomImage = ennemi.Nom.ToLower().Replace(" ", "_") + "_idle.png";
                    string cheminComplet = $"pack://application:,,,/view/GameCombat/Ressources/Sprites/{nomImage}";

                    string sourceActuelle = ImgEnnemi.Source?.ToString() ?? "";

                    // On ne change l'image QUE si elle est différente ET qu'on n'est pas en train de jouer une frame d'attaque ("_atk")
                    if (sourceActuelle != cheminComplet && !sourceActuelle.Contains("_atk"))
                    {
                        ImgEnnemi.Source = new BitmapImage(new Uri(cheminComplet));
                    }
                }
                catch
                {
                    // Si l'image n'est pas trouvée, l'image par défaut reste en place
                }
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

            SauvegardeJoueur.OrTotal += _gc.OrCumule;

            SauvegardeJoueur.AjouterExp(_gc.ExpCumule);

            _monHeros.SoignerTotalement();

            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}