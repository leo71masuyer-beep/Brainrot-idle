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
            _monHero = new Personnage("Tung Tung Sahur", 100, 10, 5, 10, 5, 150, 100, true);
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

            TxtStatAura.Text = GameState.AuraBonusFlat.ToString("0");
            TxtStatAuraMulti.Text = "+" + GameState.AuraBonus.ToString("0") + "%";

            TxtStatBonusAtk.Text = "+" + SauvegardeJoueur.AttaqueBonus.ToString("0") + "%";

            double vraiBonusDef = SauvegardeJoueur.DefenseBonus;

            if (SauvegardeJoueur.LvlCorpGigachad > 0)
            {
                vraiBonusDef += 30;
            }

            TxtStatBonusDef.Text = "+" + vraiBonusDef.ToString("0") + "%";
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
            // 1. Mise à jour des textes (Anciennes branches)
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

            // Mise à jour des textes (Branche Giga Chad)
            if (TxtGigaChad != null) TxtGigaChad.Text = $"{SauvegardeJoueur.LvlGigaChad}/1";
            if (TxtPecsChad != null) TxtPecsChad.Text = $"{SauvegardeJoueur.LvlPecsChad}/1";
            if (TxtLombaireChad != null) TxtLombaireChad.Text = $"{SauvegardeJoueur.LvlLombaireChad}/1";
            if (TxtTricepsChad != null) TxtTricepsChad.Text = $"{SauvegardeJoueur.LvlTricepsChad}/1";
            if (TxtCorpGigachad != null) TxtCorpGigachad.Text = $"{SauvegardeJoueur.LvlCorpGigachad}/1";
            if (TxtMegaChad != null) TxtMegaChad.Text = $"{SauvegardeJoueur.LvlMegaChad}/1";
            if (TxtMuscleGigaChad != null) TxtMuscleGigaChad.Text = $"{SauvegardeJoueur.LvlMuscleGigaChad}/1";
            if (TxtQuadricepsChad != null) TxtQuadricepsChad.Text = $"{SauvegardeJoueur.LvlQuadricepsChad}/1";
            if (TxtAbdoChad != null) TxtAbdoChad.Text = $"{SauvegardeJoueur.LvlAbdoChad}/1";
            if (TxtChadUltime != null) TxtChadUltime.Text = $"{SauvegardeJoueur.LvlChadUltime}/1";

            // 2. Remise en place des couleurs (Anciennes branches)
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

            // Remise en place des couleurs (Branche Giga Chad)
            if (SauvegardeJoueur.LvlGigaChad == 1) ChangerCouleurBordure(BtnGigaChad, "BordureGigaChad", Brushes.Gold);
            if (SauvegardeJoueur.LvlPecsChad == 1) ChangerCouleurBordure(BtnPecsChad, "BordurePecsChad", Brushes.Gold);
            if (SauvegardeJoueur.LvlLombaireChad == 1) ChangerCouleurBordure(BtnLombaireChad, "BordureLombaireChad", Brushes.Gold);
            if (SauvegardeJoueur.LvlTricepsChad == 1) ChangerCouleurBordure(BtnTricepsChad, "BordureTricepsChad", Brushes.Gold);
            if (SauvegardeJoueur.LvlCorpGigachad == 1) ChangerCouleurBordure(BtnCorpGigachad, "BordureCorpGigachad", Brushes.Gold);
            if (SauvegardeJoueur.LvlMegaChad == 1) ChangerCouleurBordure(BtnMegaChad, "BordureMegaChad", Brushes.Gold);
            if (SauvegardeJoueur.LvlMuscleGigaChad == 1) ChangerCouleurBordure(BtnMuscleGigaChad, "BordureMuscleGigaChad", Brushes.Gold);
            if (SauvegardeJoueur.LvlQuadricepsChad == 1) ChangerCouleurBordure(BtnQuadricepsChad, "BordureQuadricepsChad", Brushes.Gold);
            if (SauvegardeJoueur.LvlAbdoChad == 1) ChangerCouleurBordure(BtnAbdoChad, "BordureAbdoChad", Brushes.Gold);
            if (SauvegardeJoueur.LvlChadUltime == 1) ChangerCouleurBordure(BtnChadUltime, "BordureChadUltime", Brushes.Gold);
        }

        private void BtnCompBase_Click(object sender, RoutedEventArgs e)
        {
            int cout = 1;
            if (SauvegardeJoueur.Pierres >= cout && SauvegardeJoueur.LvlBase < 5)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.AttaqueBonusFlat += 10;
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
            if (SauvegardeJoueur.LvlSigmaAura == 0) return;
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
            if (SauvegardeJoueur.LvlSigmaBoy == 0) return;
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
            if (SauvegardeJoueur.Lvltralala == 0) return;
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
            if (SauvegardeJoueur.Lvlbombardilo == 0) return;
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
            if (SauvegardeJoueur.Lvlfrulifrula == 0) return;
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
            if (SauvegardeJoueur.Lvludindindindun == 0) return;
            int cout = 1;
            if (SauvegardeJoueur.Lvlbananini == 0 || SauvegardeJoueur.Pierres >= cout)
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
            if (SauvegardeJoueur.Lvltungtungsahur == 0) return;
            int cout = 1;
            if (SauvegardeJoueur.Lvlfrulifrula == 0 || SauvegardeJoueur.Pierres >= cout)
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
            if (SauvegardeJoueur.Lvlbananini == 0 || SauvegardeJoueur.Lvlpatapim == 0) return;
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
            if (SauvegardeJoueur.LvlMegaAura <= 1) return;
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
            if (SauvegardeJoueur.Lvlfrulifrula == 0
                || SauvegardeJoueur.Lvludindindindun == 0
                || SauvegardeJoueur.Lvlbananini == 0
                || SauvegardeJoueur.Lvllarila == 0
                || SauvegardeJoueur.Lvltungtungsahur == 0
                || SauvegardeJoueur.Lvlbombardilo == 0
                || SauvegardeJoueur.Lvltralala == 0
                || SauvegardeJoueur.Lvlpatapim == 0) return;
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

        #region ================== BRANCHE GIGA CHAD ==================

        private void BtnGigaChad_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlSigmaAura == 0) return; // Dépend de Sigma Aura
            int cout = 1;
            if (SauvegardeJoueur.LvlGigaChad == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                // La stat (atk + def/2) est calculée dynamiquement dans Personnage.cs
                SauvegardeJoueur.LvlGigaChad++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void BtnPecsChad_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlGigaChad == 0) return; // Dépend de Giga Chad
            int cout = 1;
            if (SauvegardeJoueur.LvlPecsChad == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.DefenseBonusFlat += 10;
                SauvegardeJoueur.LvlPecsChad++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void BtnLombaireChad_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlGigaChad == 0) return; // Dépend de Giga Chad
            int cout = 1;
            if (SauvegardeJoueur.LvlLombaireChad == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.DefenseBonusFlat += 10;
                SauvegardeJoueur.LvlLombaireChad++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void BtnTricepsChad_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlGigaChad == 0) return; // Dépend de Giga Chad
            int cout = 1;
            if (SauvegardeJoueur.LvlTricepsChad == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.DefenseBonusFlat += 10;
                SauvegardeJoueur.LvlTricepsChad++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void BtnCorpGigachad_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlPecsChad == 0) return; // Dépend de Pecs Chad
            int cout = 1;
            if (SauvegardeJoueur.LvlCorpGigachad == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                // Le passif (+30% def) est calculé dynamiquement dans Personnage.cs
                SauvegardeJoueur.LvlCorpGigachad++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void BtnMegaChad_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlLombaireChad == 0) return; // Dépend de Lombaire Chad
            int cout = 1;
            if (SauvegardeJoueur.LvlMegaChad == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                // La stat (atk + def/1.5) est calculée dynamiquement dans Personnage.cs
                SauvegardeJoueur.LvlMegaChad++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void BtnMuscleGigaChad_Click(object sender, RoutedEventArgs e)
        {
            if (SauvegardeJoueur.LvlTricepsChad == 0) return; // Dépend de Triceps Chad
            int cout = 1;
            if (SauvegardeJoueur.LvlMuscleGigaChad == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                // Le passif est calculé dynamiquement dans Personnage.cs
                SauvegardeJoueur.LvlMuscleGigaChad++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void BtnQuadricepsChad_Click(object sender, RoutedEventArgs e)
        {
            // Dépend de Corp de Gigachad OU Mega Chad
            if (SauvegardeJoueur.LvlCorpGigachad == 0 || SauvegardeJoueur.LvlMegaChad == 0) return;
            int cout = 1;
            if (SauvegardeJoueur.LvlQuadricepsChad == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.DefenseBonusFlat += 10;
                SauvegardeJoueur.LvlQuadricepsChad++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void BtnAbdoChad_Click(object sender, RoutedEventArgs e)
        {
            // Dépend de Mega Chad OU Muscle Giga Chad
            if (SauvegardeJoueur.LvlMegaChad == 0 || SauvegardeJoueur.LvlMuscleGigaChad == 0) return;
            int cout = 1;
            if (SauvegardeJoueur.LvlAbdoChad == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.DefenseBonusFlat += 10;
                SauvegardeJoueur.LvlAbdoChad++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        private void BtnChadUltime_Click(object sender, RoutedEventArgs e)
        {
            // Dépend de Quadriceps Chad OU Abdo Chad
            if (SauvegardeJoueur.LvlQuadricepsChad == 0 || SauvegardeJoueur.LvlAbdoChad == 0) return;
            int cout = 5;
            if (SauvegardeJoueur.LvlChadUltime == 0 && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                // Le passif (x3) est calculé dynamiquement dans Personnage.cs
                SauvegardeJoueur.LvlChadUltime++;
                RestaurerArbre();
                RafraichirInterface();
            }
        }

        #endregion

        #endregion
        #region ================== SYSTÈME DE GACHA ==================

        private void BtnOuvrirCommun_Click(object sender, RoutedEventArgs e)
        {
            int cout = 1000;
            if (SauvegardeJoueur.OrTotal >= cout)
            {
                SauvegardeJoueur.OrTotal -= cout;

                int tirage = _rngGacha.Next(1, 101);

                // ÉPIQUE (Total 2%)
                if (tirage <= 1)
                {
                    SauvegardeJoueur.OrTotal += 2000;
                    AfficherResultat("ÉPIQUE!!! Or + 2000", Brushes.Magenta);
                }
                else if (tirage <= 2)
                {
                    SauvegardeJoueur.VitesseBonusFlat += 5;
                    _monHero?.AmeliorerStatistique("Vitesse", 5);
                    AfficherResultat("ÉPIQUE!!! Vitesse + 5", Brushes.Magenta);
                }

                // RARE (Total 18%)
                else if (tirage <= 10)
                {
                    SauvegardeJoueur.Pierres += 1;
                    AfficherResultat("RARE Pierres + 1", Brushes.Blue);
                }
                else if (tirage <= 20)
                {
                    SauvegardeJoueur.OrTotal += 250;
                    AfficherResultat("RARE Or + 250", Brushes.Blue);
                }

                // COMMUN (Total 80%)
                else if (tirage <= 30)
                {
                    SauvegardeJoueur.PvBonusFlat += 5;
                    _monHero?.AmeliorerStatistique("Sante", 5);
                    AfficherResultat("COMMUN PV + 5", Brushes.Black);
                }
                else if (tirage <= 40)
                {
                    SauvegardeJoueur.DefenseBonusFlat += 5;
                    AfficherResultat("COMMUN Defense + 5", Brushes.Black);
                }
                else if (tirage <= 50)
                {
                    SauvegardeJoueur.AttaqueBonusFlat += 5;
                    AfficherResultat("COMMUN Attaque + 5", Brushes.Black);
                }
                else
                {
                    AfficherResultat("COMMUN RIEN", Brushes.Black);
                }

                RafraichirInterface();
            }
        }

        private void BtnOuvrirRare_Click(object sender, RoutedEventArgs e)
        {
            int cout = 10000;
            if (SauvegardeJoueur.OrTotal >= cout)
            {
                SauvegardeJoueur.OrTotal -= cout;

                int tirage = _rngGacha.Next(1, 101);

                // LÉGENDAIRE (Total 5%)
                if (tirage <= 2)
                {
                    GameState.AuraBonusFlat += 10;
                    AfficherResultat("LÉGENDAIRE !!! Aura Flat + 10", Brushes.Orange);
                }
                else if (tirage <= 5)
                {
                    SauvegardeJoueur.ChanceCritique += 2;
                    SauvegardeJoueur.DegatCritique += 10;
                    AfficherResultat("LÉGENDAIRE !! Crit +2% / Dégât Crit +10%", Brushes.Orange);
                }

                // ÉPIQUE (Total 15%)
                else if (tirage <= 12)
                {
                    SauvegardeJoueur.Pierres += 5;
                    AfficherResultat("ÉPIQUE ! Pierres + 5", Brushes.Magenta);
                }
                else if (tirage <= 20)
                {
                    SauvegardeJoueur.OrTotal += 5000;
                    AfficherResultat("ÉPIQUE ! Or + 5000", Brushes.Magenta);
                }

                // RARE (Total 40%)
                else if (tirage <= 40)
                {
                    SauvegardeJoueur.AttaqueBonusFlat += 20;
                    SauvegardeJoueur.DefenseBonusFlat += 20;
                    AfficherResultat("RARE. Attaque & Défense + 20", Brushes.Blue);
                }
                else if (tirage <= 60)
                {
                    SauvegardeJoueur.PvBonusFlat += 50;
                    _monHero?.AmeliorerStatistique("Sante", 50);
                    AfficherResultat("RARE. PV + 50", Brushes.Blue);
                }

                // COMMUN (Total 40%)
                else if (tirage <= 80)
                {
                    SauvegardeJoueur.Pierres += 1;
                    AfficherResultat("COMMUN. Pierres + 1", Brushes.Black);
                }
                else
                {
                    AfficherResultat("COMMUN. Rien d'intéressant...", Brushes.Black);
                }

                RafraichirInterface();
            }
        }

        private void BtnOuvrirMythique_Click(object sender, RoutedEventArgs e)
        {
            int cout = 100000;
            if (SauvegardeJoueur.OrTotal >= cout)
            {
                SauvegardeJoueur.OrTotal -= cout;

                int tirage = _rngGacha.Next(1, 101);

                // MYTHIQUE (Total 5%)
                if (tirage <= 2)
                {
                    if (!SauvegardeJoueur.PassifEpeeDeLAnomalie)
                    {
                        SauvegardeJoueur.PassifEpeeDeLAnomalie = true;
                        SauvegardeJoueur.ChanceCritique += 15;
                        SauvegardeJoueur.DegatCritique += 100;
                        AfficherResultat("🏆 UNIQUE : ÉPÉE DE L'ANOMALIE (Crit +15%, Dgts Crit +100%)", Brushes.Red);
                    }
                    else
                    {
                        GameState.AuraBonus += 25;
                        AfficherResultat("MYTHIQUE !!! Aura +25%", Brushes.Red);
                    }
                }
                else if (tirage <= 4)
                {
                    if (!SauvegardeJoueur.PassifArmureEtoilee)
                    {
                        SauvegardeJoueur.PassifArmureEtoilee = true;
                        SauvegardeJoueur.DefenseBonusFlat += 500;
                        SauvegardeJoueur.PvBonusFlat += 2000;
                        _monHero?.AmeliorerStatistique("Sante", 2000);
                        AfficherResultat("🏆 UNIQUE : ARMURE ÉTOILÉE (Défense +500, PV +2000)", Brushes.Red);
                    }
                    else
                    {
                        GameState.AuraBonusFlat += 50;
                        AfficherResultat("MYTHIQUE !!! Aura Flat +50", Brushes.Red);
                    }
                }
                else if (tirage <= 5)
                {
                    SauvegardeJoueur.VitesseBonusFlat += 15;
                    _monHero?.AmeliorerStatistique("Vitesse", 15);
                    AfficherResultat("MYTHIQUE !!! Vitesse + 15", Brushes.Red);
                }

                // LÉGENDAIRE (Total 20%)
                else if (tirage <= 15)
                {
                    SauvegardeJoueur.ChanceCritique += 5;
                    SauvegardeJoueur.DegatCritique += 25;
                    AfficherResultat("LÉGENDAIRE !! Crit +5% / Dégât Crit +25%", Brushes.Orange);
                }
                else if (tirage <= 25)
                {
                    GameState.AuraBonusFlat += 25;
                    AfficherResultat("LÉGENDAIRE !! Aura Flat + 25", Brushes.Orange);
                }

                // ÉPIQUE (Total 35%)
                else if (tirage <= 42)
                {
                    SauvegardeJoueur.Pierres += 15;
                    AfficherResultat("ÉPIQUE ! Pierres + 15", Brushes.Magenta);
                }
                else if (tirage <= 60)
                {
                    SauvegardeJoueur.AttaqueBonusFlat += 150;
                    SauvegardeJoueur.DefenseBonusFlat += 150;
                    AfficherResultat("ÉPIQUE ! Attaque & Défense + 150", Brushes.Magenta);
                }

                // RARE (Total 30%)
                else if (tirage <= 75)
                {
                    SauvegardeJoueur.PvBonusFlat += 200;
                    _monHero?.AmeliorerStatistique("Sante", 200);
                    AfficherResultat("RARE. PV + 200", Brushes.Blue);
                }
                else if (tirage <= 90)
                {
                    SauvegardeJoueur.OrTotal += 15000;
                    AfficherResultat("RARE. Or + 15000", Brushes.Blue);
                }

                // COMMUN (Total 10%)
                else
                {
                    AfficherResultat("COMMUN... Dommage, RIEN !", Brushes.Black);
                }

                RafraichirInterface();
            }
        }

        private void AfficherResultat(string rarete, Brush couleur)
        {
            TxtResultatTirage.Text = $"Tu as obtenu : {rarete}";
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