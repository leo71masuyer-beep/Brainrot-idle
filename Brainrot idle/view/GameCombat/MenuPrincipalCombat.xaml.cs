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

        public MenuPrincipalCombat()
        {
            InitializeComponent();
            ChargerStatistiquesPersonnage();

            this.Loaded += MenuPrincipalCombat_Loaded;
        }

        private void MenuPrincipalCombat_Loaded(object sender, RoutedEventArgs e)
        {
            RafraichirInterface();
        }
        private void RafraichirInterface()
        {
            TxtOrGlobal.Text = SauvegardeJoueur.OrTotal.ToString();
            TxtPierreGlobal.Text = SauvegardeJoueur.Pierres.ToString();
            TxtDiamantGlobal.Text = SauvegardeJoueur.DiamantsTotal.ToString();

            TxtNiveauGlobal.Text = $"NIVEAU {SauvegardeJoueur.NiveauHeros}";

            int xpRequise = SauvegardeJoueur.CalculerXpRequise(SauvegardeJoueur.NiveauHeros);

            BarreXpGlobal.Maximum = xpRequise;
            BarreXpGlobal.Value = SauvegardeJoueur.ExpTotal;
            TxtXpGlobal.Text = $"{SauvegardeJoueur.ExpTotal} / {xpRequise}";
            MettreAJourStatsUI();
        }

        private void ChargerStatistiquesPersonnage()
        {
            _monHero = new Personnage("Tung Tung Sahur", 100, 100, 5, 10, 5, 150, 100, true);

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

        // ==========================================
        // LA FONCTION MAGIQUE POUR ATTEINDRE LES BORDURES
        // ==========================================
        private void ChangerCouleurBordure(Button btn, string nomBordure, Brush couleur)
        {
            // On fouille dans le template du bouton pour trouver la bordure
            Border bordure = btn.Template.FindName(nomBordure, btn) as Border;
            if (bordure != null)
            {
                bordure.BorderBrush = couleur;
            }
        }
        // ==========================================
        // 1. CAS PARTICULIER : L'ATTAQUE DE BASE (Le point de départ)
        // ==========================================
        private void BtnCompBase_Click(object sender, RoutedEventArgs e)
        {
            int cout = (TxtNiveauBase.Text == "4/5") ? 5 : 1;

            if (SauvegardeJoueur.Pierres >= cout && TxtNiveauBase.Text != "5/5")
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.AttaqueBonus += 10;

                if (TxtNiveauBase.Text == "0/5") { TxtNiveauBase.Text = "1/5"; ChangerCouleurBordure(BtnCompBase, "BordureBase", Brushes.Yellow); }
                else if (TxtNiveauBase.Text == "1/5") { TxtNiveauBase.Text = "2/5"; }
                else if (TxtNiveauBase.Text == "2/5") { TxtNiveauBase.Text = "3/5"; }
                else if (TxtNiveauBase.Text == "3/5") { TxtNiveauBase.Text = "4/5"; }
                else if (TxtNiveauBase.Text == "4/5") { TxtNiveauBase.Text = "5/5"; ChangerCouleurBordure(BtnCompBase, "BordureBase", Brushes.Gold); }

                RafraichirInterface();
            }
        }

        // ==========================================
        // 2. LES AMÉLIORATIONS STANDARDS (Nécessitent la base à 5/5)
        // ==========================================

        private void BtnMegaAura_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNiveauBase.Text != "5/5") { MessageBox.Show("Améliorez l'attaque de base au maximum d'abord !"); return; }

            int cout = 1;
            if (TxtMegaAura.Text == "0/2" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.AttaqueBonus += 10;
                TxtMegaAura.Text = "1/2";
                ChangerCouleurBordure(BtnMegaAura, "BordureMegaAura", Brushes.Yellow);
                RafraichirInterface();
            }
            else if (TxtMegaAura.Text == "1/2" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.AttaqueBonus += 10;
                TxtMegaAura.Text = "2/2";
                ChangerCouleurBordure(BtnMegaAura, "BordureMegaAura", Brushes.Gold);
                RafraichirInterface();
            }
        }

        private void BtnSigmaBoy_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNiveauBase.Text != "5/5") { MessageBox.Show("Améliorez l'attaque de base au maximum d'abord !"); return; }

            int cout = 1;
            if (TxtSigmaBoy.Text == "0/1" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.AttaqueBonus += 25;
                TxtSigmaBoy.Text = "1/1";
                ChangerCouleurBordure(BtnSigmaBoy, "BordureSigmaBoy", Brushes.Gold);
                RafraichirInterface();
            }
        }

        private void Btntungtungsahur_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNiveauBase.Text != "5/5") { MessageBox.Show("Améliorez l'attaque de base au maximum d'abord !"); return; }

            int cout = 1;
            if (Txttungtungsahur.Text == "0/1" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.AttaqueBonusFlat += 50;
                Txttungtungsahur.Text = "1/1";
                ChangerCouleurBordure(Btntungtungsahur, "Borduretungtungsahur", Brushes.Gold);
                RafraichirInterface();
            }
        }

        private void Btnbombardilo_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNiveauBase.Text != "5/5") { MessageBox.Show("Améliorez l'attaque de base au maximum d'abord !"); return; }

            int cout = 1;
            if (Txtbombardilo.Text == "0/1" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.ChanceCritique += 5;
                Txtbombardilo.Text = "1/1";
                ChangerCouleurBordure(Btnbombardilo, "Bordurebombardilo", Brushes.Gold);
                RafraichirInterface();
            }
        }

        private void Btnpatapim_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNiveauBase.Text != "5/5") { MessageBox.Show("Améliorez l'attaque de base au maximum d'abord !"); return; }

            int cout = 1;
            if (Txtpatapim.Text == "0/1" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.ChanceCritique += 5;
                Txtpatapim.Text = "1/1";
                ChangerCouleurBordure(Btnpatapim, "Bordurepatapim", Brushes.Gold);
                RafraichirInterface();
            }
        }

        // ==========================================
        // 3. LES COMPÉTENCES DE NIVEAU 2 (Nécessitent les compétences juste avant)
        // ==========================================

        private void BtnSuperAura_Click(object sender, RoutedEventArgs e)
        {
            // Nécessite Mega Aura au max
            if (TxtMegaAura.Text != "2/2") { MessageBox.Show("Nécessite Mega Aura au niveau maximum !"); return; }

            int cout = 1;
            if (TxtSuperAura.Text == "0/1" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                GameState.AuraBonus += 5;
                TxtSuperAura.Text = "1/1";
                ChangerCouleurBordure(BtnSuperAura, "BordureSuperAura", Brushes.Gold);
                RafraichirInterface();
            }
        }

        private void Btntralala_Click(object sender, RoutedEventArgs e)
        {
            // Nécessite tungtungsahur
            if (Txttungtungsahur.Text != "1/1") { MessageBox.Show("Nécessite la compétence précédente !"); return; }

            int cout = 1;
            if (Txttralala.Text == "0/1" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.ChanceCritique += 5;
                Txttralala.Text = "1/1";
                ChangerCouleurBordure(Btntralala, "Borduretralala", Brushes.Gold);
                RafraichirInterface();
            }
        }

        private void Btnudindindindun_Click(object sender, RoutedEventArgs e)
        {
            // Nécessite bombardilo
            if (Txtbombardilo.Text != "1/1") { MessageBox.Show("Nécessite la compétence précédente !"); return; }

            int cout = 1;
            if (Txtudindindindun.Text == "0/1" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.DegatCritique += 15;
                Txtudindindindun.Text = "1/1";
                ChangerCouleurBordure(Btnudindindindun, "Bordureudindindindun", Brushes.Gold);
                RafraichirInterface();
            }
        }

        private void Btnbananini_Click(object sender, RoutedEventArgs e)
        {
            // Nécessite patapim
            if (Txtpatapim.Text != "1/1") { MessageBox.Show("Nécessite la compétence précédente !"); return; }

            int cout = 1;
            if (Txtbananini.Text == "0/1" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.DegatCritique += 15;
                Txtbananini.Text = "1/1";
                ChangerCouleurBordure(Btnbananini, "Bordurebananini", Brushes.Gold);
                RafraichirInterface();
            }
        }

        // ==========================================
        // 4. LES COMPÉTENCES DE NIVEAU 3 (Bouts de branches)
        // ==========================================

        private void Btnfrulifrula_Click(object sender, RoutedEventArgs e)
        {
            // Nécessite tralala
            if (Txttralala.Text != "1/1") { MessageBox.Show("Nécessite la compétence précédente !"); return; }

            int cout = 1;
            if (Txtfrulifrula.Text == "0/1" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.DegatCritique += 15;
                Txtfrulifrula.Text = "1/1";
                ChangerCouleurBordure(Btnfrulifrula, "Bordurefrulifrula", Brushes.Gold);
                RafraichirInterface();
            }
        }

        private void Btnlarila_Click(object sender, RoutedEventArgs e)
        {
            // Nécessite bananini
            if (Txtbananini.Text != "1/1") { MessageBox.Show("Nécessite la compétence précédente !"); return; }

            int cout = 1;
            if (Txtlarila.Text == "0/1" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.ChanceCritique += 5;
                SauvegardeJoueur.DegatCritique += 15;
                Txtlarila.Text = "1/1";
                ChangerCouleurBordure(Btnlarila, "Bordurelarila", Brushes.Gold);
                RafraichirInterface();
            }
        }

        // ==========================================
        // 5. LES COMPÉTENCES ULTIMES (Coût : 5)
        // ==========================================

        private void BtnSigmaAura_Click(object sender, RoutedEventArgs e)
        {
            // Nécessite Super Aura
            if (TxtSuperAura.Text != "1/1") { MessageBox.Show("Nécessite Super Aura !"); return; }

            int cout = 3;
            if (TxtSigmaAura.Text == "0/1" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                GameState.AuraBonus += 20;
                TxtSigmaAura.Text = "1/1";
                ChangerCouleurBordure(BtnSigmaAura, "BordureSigmaAura", Brushes.Gold);
                RafraichirInterface();
            }
        }

        private void Btnalliance_Click(object sender, RoutedEventArgs e)
        {
            if (Txtfrulifrula.Text != "1/1") { MessageBox.Show("Vous devez atteindre le bout de la branche de droite d'abord !"); return; }

            int cout = 5;
            if (Txtalliance.Text == "0/1" && SauvegardeJoueur.Pierres >= cout)
            {
                SauvegardeJoueur.Pierres -= cout;
                SauvegardeJoueur.PassifAllianceActif = true;
                Txtalliance.Text = "1/1";
                ChangerCouleurBordure(Btnalliance, "Bordurealliance", Brushes.Gold);
                RafraichirInterface();
            }
        }

        // ==========================================
        // BOUTONS DE NAVIGATION & NAVIGATION ONGLETS
        // ==========================================
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

        private void BtnOngletStats_Click(object sender, RoutedEventArgs e)
        {
            PanneauStats.Visibility = Visibility.Visible;
            PanneauArbre.Visibility = Visibility.Collapsed;

            FondOngletStats.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2A004D"));
            FondOngletStats.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9D00FF"));
            TexteOngletStats.Foreground = Brushes.White;

            ResetOngletCouleur(FondOngletArbre, TexteOngletArbre);
        }

        private void BtnOngletArbre_Click(object sender, RoutedEventArgs e)
        {
            PanneauArbre.Visibility = Visibility.Visible;
            PanneauStats.Visibility = Visibility.Collapsed;

            // Couleurs d'onglet Actif
            FondOngletArbre.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2A004D"));
            FondOngletArbre.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9D00FF"));
            TexteOngletArbre.Foreground = Brushes.White;

            ResetOngletCouleur(FondOngletStats, TexteOngletStats);

            // On force l'attente maximale pour que la fenêtre soit 100% calculée
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                CarteScroll.UpdateLayout(); // Force le calcul des dimensions

                // 1535 = Position (1500) + la moitié de la taille de ton bouton (35)
                CarteScroll.ScrollToHorizontalOffset(1535 - (CarteScroll.ViewportWidth / 2));
                CarteScroll.ScrollToVerticalOffset(1535 - (CarteScroll.ViewportHeight / 2));

            }), System.Windows.Threading.DispatcherPriority.ContextIdle);
        }

        private void ResetOngletCouleur(Border fond, TextBlock texte)
        {
            fond.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0A0410"));
            fond.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4D0099"));
            texte.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A080C0"));
        }

        // 1. Quand on clique sur le fond de l'arbre
        private void ZoneArbre_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _enDeplacement = true;
            _pointDepartClic = e.GetPosition(CarteScroll); // On mémorise la position de départ

            ZoneArbre.CaptureMouse(); // Verrouille la souris pour ne pas la perdre si on bouge vite
            ZoneArbre.Cursor = System.Windows.Input.Cursors.SizeAll; // Transforme la souris en croix directionnelle
        }

        // 2. Quand on lâche le clic
        private void ZoneArbre_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _enDeplacement = false;
            ZoneArbre.ReleaseMouseCapture(); // On libère la souris
            ZoneArbre.Cursor = System.Windows.Input.Cursors.Arrow; // Remet la souris normale
        }

        // 3. Quand on bouge la souris (seulement si le clic est enfoncé)
        private void ZoneArbre_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_enDeplacement)
            {
                Point positionActuelle = e.GetPosition(CarteScroll);

                // On calcule de combien de pixels la souris a bougé
                double deplacementX = positionActuelle.X - _pointDepartClic.X;
                double deplacementY = positionActuelle.Y - _pointDepartClic.Y;

                // On fait glisser les barres de défilement (Scroll) dans le sens inverse du mouvement
                CarteScroll.ScrollToHorizontalOffset(CarteScroll.HorizontalOffset - deplacementX);
                CarteScroll.ScrollToVerticalOffset(CarteScroll.VerticalOffset - deplacementY);

                // On met à jour la position pour que le mouvement soit fluide en continu
                _pointDepartClic = positionActuelle;
            }
        }
    }
}