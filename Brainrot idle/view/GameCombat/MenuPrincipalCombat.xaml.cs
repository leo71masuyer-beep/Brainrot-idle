using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Brainrot_idle.Game.Combatgame.model;
using Brainrot_idle.Ressources;

namespace Brainrot_idle.view.GameCombat
{
    public partial class MenuPrincipalCombat : Page
    {
        private Personnage _monHero;
        private Point _pointDepartClic;
        private bool _enDeplacement = false;
        private Random _rngGacha = new Random();

        public MenuPrincipalCombat()
        {
            InitializeComponent();
            ChargerStatistiquesPersonnage();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RestaurerArbre();
            RafraichirInterface();
        }

        #region ================== MISE À JOUR DE L'INTERFACE ==================

        private void ChargerStatistiquesPersonnage()
        {
            _monHero = new Personnage("Tung Tung Sahur", 100, 100, 5, 10, 5, 150, 100, true);
            MettreAJourStatsUI();
        }

        private void RafraichirInterface()
        {
            // Ressources globales
            TxtOrGlobal.Text = SauvegardeJoueur.OrTotal.ToString();
            TxtPierreGlobal.Text = SauvegardeJoueur.Pierres.ToString();
            TxtDiamantGlobal.Text = SauvegardeJoueur.DiamantsTotal.ToString();

            // Mise à jour de l'or spécifique à l'onglet Gacha s'il existe
            if (TxtOrGacha != null) TxtOrGacha.Text = SauvegardeJoueur.OrTotal.ToString();

            // Expérience et Niveau
            TxtNiveauGlobal.Text = $"NIVEAU {SauvegardeJoueur.NiveauHeros}";
            int xpRequise = SauvegardeJoueur.CalculerXpRequise(SauvegardeJoueur.NiveauHeros);

            BarreXpGlobal.Maximum = xpRequise;
            BarreXpGlobal.Value = SauvegardeJoueur.ExpTotal;
            TxtXpGlobal.Text = $"{SauvegardeJoueur.ExpTotal} / {xpRequise}";

            MettreAJourStatsUI();
        }

        private void MettreAJourStatsUI()
        {
            if (_monHero == null) return;

            TxtStatAttaque.Text = _monHero.Attaque.ToString("0");
            TxtStatDefense.Text = _monHero.Defense.ToString("0");
            TxtStatSante.Text = _monHero.PointsDeVieMax.ToString("0");
            TxtStatVitesse.Text = _monHero.VitesseAttaque.ToString("0");

            float chanceCritFinale = _monHero.ChanceCritique + SauvegardeJoueur.ChanceCritique;
            float degatCritFinal = _monHero.DegatCritique + SauvegardeJoueur.DegatCritique;

            TxtStatCritique.Text = chanceCritFinale.ToString("0") + "%";
            TxtStatDegatCritique.Text = degatCritFinal.ToString("0") + "%";
        }

        #endregion

        #region ================== GESTION DES ONGLETS ==================

        // Fonction centralisée pour changer d'onglet proprement sans répéter le code
        private void ActiverOnglet(Grid panneauActif, Border fondActif, TextBlock texteActif)
        {
            // 1. On cache tout
            PanneauStats.Visibility = Visibility.Collapsed;
            PanneauArbre.Visibility = Visibility.Collapsed;
            PanneauGacha.Visibility = Visibility.Collapsed;

            // 2. On éteint tous les boutons
            ResetOngletCouleur(FondOngletStats, TexteOngletStats);
            ResetOngletCouleur(FondOngletArbre, TexteOngletArbre);
            ResetOngletCouleur(FondOngletGacha, TexteOngletGacha);

            // 3. On affiche seulement le bon panneau et on allume le bon bouton
            panneauActif.Visibility = Visibility.Visible;
            fondActif.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2A004D"));
            fondActif.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9D00FF"));
            texteActif.Foreground = Brushes.White;
        }

        private void ResetOngletCouleur(Border fond, TextBlock texte)
        {
            fond.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0A0410"));
            fond.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4D0099"));
            texte.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A080C0"));
        }

        private void BtnOngletStats_Click(object sender, RoutedEventArgs e)
        {
            ActiverOnglet(PanneauStats, FondOngletStats, TexteOngletStats);
        }

        private void BtnOngletArbre_Click(object sender, RoutedEventArgs e)
        {
            ActiverOnglet(PanneauArbre, FondOngletArbre, TexteOngletArbre);

            // On force l'attente maximale pour que la fenêtre soit 100% calculée et on centre la vue
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                CarteScroll.UpdateLayout();
                CarteScroll.ScrollToHorizontalOffset(1535 - (CarteScroll.ViewportWidth / 2));
                CarteScroll.ScrollToVerticalOffset(1535 - (CarteScroll.ViewportHeight / 2));
            }), System.Windows.Threading.DispatcherPriority.ContextIdle);
        }

        private void BtnOngletGacha_Click(object sender, RoutedEventArgs e)
        {
            ActiverOnglet(PanneauGacha, FondOngletGacha, TexteOngletGacha);
            RafraichirInterface();
        }

        #endregion

        #region ================== ARBRE DE COMPÉTENCES ==================

        private void ChangerCouleurBordure(Button btn, string nomBordure, Brush couleur)
        {
            if (btn.Template.FindName(nomBordure, btn) is Border bordure)
            {
                bordure.BorderBrush = couleur;
            }
        }

        private void RestaurerArbre()
        {
            // 1. Mise à jour des textes
            TxtNiveauBase.Text = $"{SauvegardeJoueur.LvlBase}/5";
            TxtSuperAura.Text = $"{SauvegardeJoueur.LvlSuperAura}/1";
            TxtMegaAura.Text = $"{SauvegardeJoueur.LvlMegaAura}/2";
            TxtSigmaAura.Text = $"{SauvegardeJoueur.LvlSigmaAura}/1";
            TxtSigmaBoy.Text = $"{SauvegardeJoueur.LvlSigmaBoy}/1";
            Txttungtungsahur.Text = $"{SauvegardeJoueur.Lvltungtungsahur}/1";
            Txttralala.Text = $"{SauvegardeJoueur.Lvltralala}/1";
            Txtfrulifrula.Text = $"{SauvegardeJoueur.Lvlfrulifrula}/1";
            Txtbombardilo.Text = $"{SauvegardeJoueur.Lvlbombardilo}/1";
            Txtudindindindun.Text = $"{SauvegardeJoueur.Lvludindindindun}/1";
            Txtpatapim.Text = $"{SauvegardeJoueur.Lvlpatapim}/1";
            Txtbananini.Text = $"{SauvegardeJoueur.Lvlbananini}/1";
            Txtlarila.Text = $"{SauvegardeJoueur.Lvllarila}/1";
            Txtalliance.Text = $"{SauvegardeJoueur.Lvlalliance}/1";

            // 2. Remise en place des couleurs
            if (SauvegardeJoueur.LvlBase > 0) ChangerCouleurBordure(BtnCompBase, "BordureBase", Brushes.Yellow);
            if (SauvegardeJoueur.LvlBase == 5) ChangerCouleurBordure(BtnCompBase, "BordureBase", Brushes.Gold);
            if (SauvegardeJoueur.LvlSuperAura == 1) ChangerCouleurBordure(BtnSuperAura, "BordureSuperAura", Brushes.Gold);
            if (SauvegardeJoueur.LvlMegaAura == 1) ChangerCouleurBordure(BtnMegaAura, "BordureMegaAura", Brushes.Yellow);
            if (SauvegardeJoueur.LvlMegaAura == 2) ChangerCouleurBordure(BtnMegaAura, "BordureMegaAura", Brushes.Gold);
            if (SauvegardeJoueur.LvlSigmaAura == 1) ChangerCouleurBordure(BtnSigmaAura, "BordureSigmaAura", Brushes.Gold);
            if (SauvegardeJoueur.LvlSigmaBoy == 1) ChangerCouleurBordure(BtnSigmaBoy, "BordureSigmaBoy", Brushes.Gold);
            if (SauvegardeJoueur.Lvltungtungsahur == 1) ChangerCouleurBordure(Btntungtungsahur, "Borduretungtungsahur", Brushes.Gold);
            if (SauvegardeJoueur.Lvltralala == 1) ChangerCouleurBordure(Btntralala, "Borduretralala", Brushes.Gold);
            if (SauvegardeJoueur.Lvlfrulifrula == 1) ChangerCouleurBordure(Btnfrulifrula, "Bordurefrulifrula", Brushes.Gold);
            if (SauvegardeJoueur.Lvlbombardilo == 1) ChangerCouleurBordure(Btnbombardilo, "Bordurebombardilo", Brushes.Gold);
            if (SauvegardeJoueur.Lvludindindindun == 1) ChangerCouleurBordure(Btnudindindindun, "Bordureudindindindun", Brushes.Gold);
            if (SauvegardeJoueur.Lvlpatapim == 1) ChangerCouleurBordure(Btnpatapim, "Bordurepatapim", Brushes.Gold);
            if (SauvegardeJoueur.Lvlbananini == 1) ChangerCouleurBordure(Btnbananini, "Bordurebananini", Brushes.Gold);
            if (SauvegardeJoueur.Lvllarila == 1) ChangerCouleurBordure(Btnlarila, "Bordurelarila", Brushes.Gold);
            if (SauvegardeJoueur.Lvlalliance == 1) ChangerCouleurBordure(Btnalliance, "Bordurealliance", Brushes.Gold);
        }

        private void BtnCompBase_Click(object sender, RoutedEventArgs e)
        {
            int cout = 1;
            if (SauvegardeJoueur.Pierres >= cout && SauvegardeJoueur.LvlBase < 5)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.AttaqueBonus += 10;
                SauvegardeJoueur.LvlBase++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }
        private void BtnSuperAura_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlBase < 5) return;
            int cout = 1;
            if (SauvegardeJoueur.LvlSuperAura == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                GameState.AuraBonus += 5;
                SauvegardeJoueur.LvlSuperAura++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }
        private void BtnMegaAura_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlSuperAura == 0) return;
            int cout = 1;
            if (SauvegardeJoueur.LvlMegaAura < 2 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.AttaqueBonus += 10;
                SauvegardeJoueur.LvlMegaAura++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void BtnSigmaBoy_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlBase < 5) return;
            int cout = 1;
            if (SauvegardeJoueur.LvlSigmaBoy == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.AttaqueBonus += 25;
                SauvegardeJoueur.LvlSigmaBoy++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void Btntungtungsahur_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlBase < 5) return;
            int cout = 1;
            if (SauvegardeJoueur.Lvltungtungsahur == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.AttaqueBonusFlat += 50;
                SauvegardeJoueur.Lvltungtungsahur++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void Btnbombardilo_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlBase < 5) return;
            int cout = 1;
            if (SauvegardeJoueur.Lvlbombardilo == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.ChanceCritique += 5;
                SauvegardeJoueur.Lvlbombardilo++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void Btnpatapim_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlBase < 5) return;
            int cout = 1;
            if (SauvegardeJoueur.Lvlpatapim == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.ChanceCritique += 5;
                SauvegardeJoueur.Lvlpatapim++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }



        private void Btntralala_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.Lvltungtungsahur == 0) return;
            int cout = 1;
            if (SauvegardeJoueur.Lvltralala == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.ChanceCritique += 5;
                SauvegardeJoueur.Lvltralala++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void Btnudindindindun_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.Lvlbombardilo == 0) return;
            int cout = 1;
            if (SauvegardeJoueur.Lvludindindindun == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.DegatCritique += 15;
                SauvegardeJoueur.Lvludindindindun++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void Btnbananini_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.Lvlpatapim == 0) return;
            int cout = 1;
            if (SauvegardeJoueur.Lvlbananini == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.DegatCritique += 15;
                SauvegardeJoueur.Lvlbananini++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void Btnfrulifrula_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.Lvltralala == 0) return;
            int cout = 1;
            if (SauvegardeJoueur.Lvlfrulifrula == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.DegatCritique += 15;
                SauvegardeJoueur.Lvlfrulifrula++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void Btnlarila_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.Lvlbananini == 0) return;
            int cout = 1;
            if (SauvegardeJoueur.Lvllarila == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.ChanceCritique += 5;
                SauvegardeJoueur.DegatCritique += 15;
                SauvegardeJoueur.Lvllarila++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void BtnSigmaAura_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlSuperAura == 0) return;
            int cout = 5;
            if (SauvegardeJoueur.LvlSigmaAura == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                GameState.AuraBonus += 20;
                SauvegardeJoueur.LvlSigmaAura++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void Btnalliance_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.Lvlfrulifrula == 0) return;
            int cout = 5;
            if (SauvegardeJoueur.Lvlalliance == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.PassifAllianceActif = true;
                SauvegardeJoueur.Lvlalliance++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        #endregion

        #region ================== SYSTÈME DE GACHA ==================

        private void BtnOuvrirCommun_Click(object sender, RoutedEventArgs e)
        {
            int cout = 500;
            if (SauvegardeJoueur.OrTotal >= cout)
            {
                SauvegardeJoueur.OrTotal -= cout;
                RafraichirInterface();

                int tirage = _rngGacha.Next(1, 101);

                // ÉPIQUE (Total 2%)
                if (tirage <= 1) { AfficherResultat("ÉPIQUE!!! gold + 2000", Brushes.Magenta); }        
                else if (tirage <= 2) { AfficherResultat("ÉPIQUE!!! vitesse + 5", Brushes.Magenta); }   

                // RARE (Total 18%)
                else if (tirage <= 10) { AfficherResultat("RARE Pierres + 1", Brushes.Blue); }         
                else if (tirage <= 20) { AfficherResultat("RARE gold + 250", Brushes.Blue); }         

                // COMMUN (Total 80%)
                else if (tirage <= 30) { AfficherResultat("COMMUN PV + 5", Brushes.Black); }         
                else if (tirage <= 40) { AfficherResultat("COMMUN Defense + 5", Brushes.Black); }       
                else if (tirage <= 50) { AfficherResultat("COMMUN Attaque + 5", Brushes.Black); }       
                else { AfficherResultat("COMMUN RIEN", Brushes.Black); }                                
            }
        }

        private void BtnOuvrirRare_Click(object sender, RoutedEventArgs e)
        {
            int cout = 2000;
            if (SauvegardeJoueur.OrTotal >= cout)
            {
                SauvegardeJoueur.OrTotal -= cout;
                RafraichirInterface();

                int tirage = _rngGacha.Next(1, 101);

                if (tirage <= 5) { AfficherResultat("LÉGENDAIRE", Brushes.Orange); }
                else if (tirage <= 20) { AfficherResultat("ÉPIQUE", Brushes.Magenta); }
                else if (tirage <= 60) { AfficherResultat("RARE", Brushes.Blue); }
                else { AfficherResultat("COMMUN", Brushes.Black); }
            }
        }

        private void BtnOuvrirMythique_Click(object sender, RoutedEventArgs e)
        {
            int cout = 10000;
            if (SauvegardeJoueur.OrTotal >= cout)
            {
                SauvegardeJoueur.OrTotal -= cout;
                RafraichirInterface();

                int tirage = _rngGacha.Next(1, 101);

                if (tirage <= 5) { AfficherResultat("MYTHIQUE", Brushes.Red); }
                else if (tirage <= 25) { AfficherResultat("LÉGENDAIRE", Brushes.Orange); }
                else if (tirage <= 60) { AfficherResultat("ÉPIQUE", Brushes.Magenta); }
                else if (tirage <= 90) { AfficherResultat("RARE", Brushes.Blue); }
                else { AfficherResultat("COMMUN", Brushes.Black); }
            }
        }

        private void AfficherResultat(string rarete, Brush couleur)
        {
            TxtResultatTirage.Text = $"Tu as obtenu un objet {rarete} !";
            TxtResultatTirage.Foreground = couleur;
        }

        #endregion

        #region ================== NAVIGATION & DÉPLACEMENT SOURIS ==================

        private void BtnStartCombat_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem itemSelectionne = (ComboBoxItem)ComboDifficulte.SelectedItem;
            string difficulte = itemSelectionne.Content.ToString();

            if (difficulte == "Facile")
            {
                this.NavigationService.Navigate(new MonJeuCombatFrame(_monHero));
            }
            else
            {
                MessageBox.Show("La difficulté " + difficulte + " n'est pas encore disponible !");
            }
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void ZoneArbre_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _enDeplacement = true;
            _pointDepartClic = e.GetPosition(CarteScroll);
            ZoneArbre.CaptureMouse();
            ZoneArbre.Cursor = System.Windows.Input.Cursors.SizeAll;
        }

        private void ZoneArbre_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _enDeplacement = false;
            ZoneArbre.ReleaseMouseCapture();
            ZoneArbre.Cursor = System.Windows.Input.Cursors.Arrow;
        }

        private void ZoneArbre_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_enDeplacement)
            {
                Point positionActuelle = e.GetPosition(CarteScroll);

                double deplacementX = positionActuelle.X - _pointDepartClic.X;
                double deplacementY = positionActuelle.Y - _pointDepartClic.Y;

                CarteScroll.ScrollToHorizontalOffset(CarteScroll.HorizontalOffset - deplacementX);
                CarteScroll.ScrollToVerticalOffset(CarteScroll.VerticalOffset - deplacementY);

                _pointDepartClic = positionActuelle;
            }
        }

        #endregion
    }
}