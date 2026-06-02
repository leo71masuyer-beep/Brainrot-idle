using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Brainrot_idle.Game.Combatgame.model;

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

            // On demande à la page de se mettre à jour à chaque fois qu'elle s'affiche à l'écran
            this.Loaded += MenuPrincipalCombat_Loaded;
        }

        private void MenuPrincipalCombat_Loaded(object sender, RoutedEventArgs e)
        {
            TxtOrGlobal.Text = SauvegardeJoueur.OrTotal.ToString();
            TxtPierreGlobal.Text = SauvegardeJoueur.PierresTotal.ToString();
            TxtDiamantGlobal.Text = SauvegardeJoueur.DiamantsTotal.ToString();

            TxtNiveauGlobal.Text = $"NIVEAU {SauvegardeJoueur.NiveauHeros}";

            int xpRequise = SauvegardeJoueur.CalculerXpRequise(SauvegardeJoueur.NiveauHeros);

            BarreXpGlobal.Maximum = xpRequise;
            BarreXpGlobal.Value = SauvegardeJoueur.ExpTotal;
            TxtXpGlobal.Text = $"{SauvegardeJoueur.ExpTotal} / {xpRequise}";
        }

        private void ChargerStatistiquesPersonnage()
        {
            // Création du héros avec ses statistiques de base
            _monHero = new Personnage("Tung Tung Sahur", 100, 10, 5, 10, 5, 150, 100, true);

            // ON APPLIQUE LES AMÉLIORATIONS DE LA SAUVEGARDE
            // (Évite que les statistiques durement achetées ne disparaissent en rechargeant la page)
            if (SauvegardeJoueur.CompBaseAttaque > 0)
            {
                _monHero.AmeliorerStatistique("Attaque", SauvegardeJoueur.CompBaseAttaque * 10);
            }
            if (SauvegardeJoueur.CompVoieBrainrot > 0)
            {
                _monHero.AmeliorerStatistique("Vitesse", 15);
                _monHero.AmeliorerStatistique("Critique", 10);
            }
            if (SauvegardeJoueur.CompVoieGigachad > 0)
            {
                _monHero.AmeliorerStatistique("Sante", 500);
            }

            // Liaison de toutes les statistiques aux TextBlocks de l'interface
            MettreAJourStatsUI();
        }

        private void MettreAJourStatsUI()
        {
            if (_monHero == null) return;

            TxtStatAttaque.Text = _monHero.Attaque.ToString();
            TxtStatDefense.Text = _monHero.Defense.ToString();
            TxtStatSante.Text = _monHero.PointsDeVieMax.ToString();
            TxtStatVitesse.Text = _monHero.VitesseAttaque.ToString();
            TxtStatCritique.Text = _monHero.ChanceCritique.ToString() + "%";
            TxtStatDegatCritique.Text = _monHero.DegatCritique.ToString() + "%";
        }

        // ==========================================
        // LOGIQUE VISUELLE DE L'ARBRE DE COMPÉTENCES
        // ==========================================
        private void MettreAJourArbreUI()
        {
            TxtNiveauBase.Text = $"{SauvegardeJoueur.CompBaseAttaque}/5";

            // 1. Changement de couleur de la bordure du nœud de base
            var bordureBase = (Border)BtnCompBase.Template.FindName("BordureBase", BtnCompBase);
            if (bordureBase != null)
            {
                bordureBase.BorderBrush = SauvegardeJoueur.CompBaseAttaque > 0 ? Brushes.Gold : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"));
            }

            // Le reste a été supprimé en attendant de recréer les nouveaux nœuds sur la carte géante !
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
        // 1. LES COMPÉTENCES DE STATISTIQUES (Actives)
        // ==========================================

        private void BtnSigmaBoy_Click(object sender, RoutedEventArgs e)
        {
            if (TxtSigmaBoy.Text == "0/1")
            {
                // Stats : +25% attaque
                TxtSigmaBoy.Text = "1/1";
                ChangerCouleurBordure(BtnSigmaBoy, "BordureSigmaBoy", Brushes.Gold);
            }
        }

        private void Btntungtungsahur_Click(object sender, RoutedEventArgs e)
        {
            if (Txttungtungsahur.Text == "0/1")
            {
                // Stats : +50 Attaque
                Txttungtungsahur.Text = "1/1";
                ChangerCouleurBordure(Btntungtungsahur, "Borduretungtungsahur", Brushes.Gold);
            }
        }

        private void Btntralala_Click(object sender, RoutedEventArgs e)
        {
            if (Txttralala.Text == "0/1")
            {
                // Stats : Chance de coup critique +5%
                Txttralala.Text = "1/1";
                ChangerCouleurBordure(Btntralala, "Borduretralala", Brushes.Gold);
            }
        }

        private void Btnfrulifrula_Click(object sender, RoutedEventArgs e)
        {
            if (Txtfrulifrula.Text == "0/1")
            {
                // Stats : Dégâts critiques +15%
                Txtfrulifrula.Text = "1/1";
                ChangerCouleurBordure(Btnfrulifrula, "Bordurefrulifrula", Brushes.Gold);
            }
        }

        private void Btnbombardilo_Click(object sender, RoutedEventArgs e)
        {
            if (Txtbombardilo.Text == "0/1")
            {
                // Stats : Chance de coup critique +5%
                Txtbombardilo.Text = "1/1";
                ChangerCouleurBordure(Btnbombardilo, "Bordurebombardilo", Brushes.Gold);
            }
        }

        private void Btnudindindindun_Click(object sender, RoutedEventArgs e)
        {
            if (Txtudindindindun.Text == "0/1")
            {
                // Stats : Dégâts critiques +15%
                Txtudindindindun.Text = "1/1";
                ChangerCouleurBordure(Btnudindindindun, "Bordureudindindindun", Brushes.Gold);
            }
        }

        private void Btnpatapim_Click(object sender, RoutedEventArgs e)
        {
            if (Txtpatapim.Text == "0/1")
            {
                // Stats : Chance de coup critique +5%
                Txtpatapim.Text = "1/1";
                ChangerCouleurBordure(Btnpatapim, "Bordurepatapim", Brushes.Gold);
            }
        }

        private void Btnbananini_Click(object sender, RoutedEventArgs e)
        {
            if (Txtbananini.Text == "0/1")
            {
                // Stats : Dégâts critiques +15%
                Txtbananini.Text = "1/1";
                ChangerCouleurBordure(Btnbananini, "Bordurebananini", Brushes.Gold);
            }
        }

        private void Btnlarila_Click(object sender, RoutedEventArgs e)
        {
            if (Txtlarila.Text == "0/1")
            {
                // Stats : Chance crit +5% ET Dégâts crit +15%
                Txtlarila.Text = "1/1";
                ChangerCouleurBordure(Btnlarila, "Bordurelarila", Brushes.Gold);
            }
        }

        // ==========================================
        // 2. LES COMPÉTENCES À PLUSIEURS NIVEAUX
        // ==========================================

        private void BtnMegaAura_Click(object sender, RoutedEventArgs e)
        {
            if (TxtMegaAura.Text == "0/2")
            {
                TxtMegaAura.Text = "1/2";
                ChangerCouleurBordure(BtnMegaAura, "BordureMegaAura", Brushes.Yellow);
            }
            else if (TxtMegaAura.Text == "1/2")
            {
                TxtMegaAura.Text = "2/2";
                ChangerCouleurBordure(BtnMegaAura, "BordureMegaAura", Brushes.Gold);
            }
        }

        private void BtnCompBase_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNiveauBase.Text == "0/5")
            {
                // Stats : +10 Attaque
                TxtNiveauBase.Text = "1/5";
                ChangerCouleurBordure(BtnCompBase, "BordureBase", Brushes.Yellow);
            }
            else if (TxtNiveauBase.Text == "1/5")
            {
                // Stats : +10 Attaque
                TxtNiveauBase.Text = "2/5";
                ChangerCouleurBordure(BtnCompBase, "BordureBase", Brushes.Yellow);
            }
            else if (TxtNiveauBase.Text == "2/5")
            {
                // Stats : +10 Attaque
                TxtNiveauBase.Text = "3/5";
                ChangerCouleurBordure(BtnCompBase, "BordureBase", Brushes.Yellow);
            }
            else if (TxtNiveauBase.Text == "3/5")
            {
                // Stats : +10 Attaque
                TxtNiveauBase.Text = "4/5";
                ChangerCouleurBordure(BtnCompBase, "BordureBase", Brushes.Yellow);
            }
            else if (TxtNiveauBase.Text == "4/5")
            {
                // Stats : +10 Attaque
                TxtNiveauBase.Text = "5/5";
                ChangerCouleurBordure(BtnCompBase, "BordureBase", Brushes.Gold);
            }
        }

        // ==========================================
        // 3. PRÉPARATION POUR LE SYSTÈME D'AURA
        // ==========================================

        private void BtnSuperAura_Click(object sender, RoutedEventArgs e)
        {
            if (TxtSuperAura.Text == "0/1")
            {
                TxtSuperAura.Text = "1/1";
                ChangerCouleurBordure(BtnSuperAura, "BordureSuperAura", Brushes.Gold);
            }
        }

        private void BtnSigmaAura_Click(object sender, RoutedEventArgs e)
        {
            if (TxtSigmaAura.Text == "0/1")
            {
                TxtSigmaAura.Text = "1/1";
                ChangerCouleurBordure(BtnSigmaAura, "BordureSigmaAura", Brushes.Gold);
            }
        }

        private void Btnalliance_Click(object sender, RoutedEventArgs e)
        {
            if (Txtalliance.Text == "0/1")
            {
                Txtalliance.Text = "1/1";
                ChangerCouleurBordure(Btnalliance, "Bordurealliance", Brushes.Gold);
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