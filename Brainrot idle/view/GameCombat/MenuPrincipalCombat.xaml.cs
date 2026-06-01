using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Brainrot_idle.Game.Combatgame.model;

namespace Brainrot_idle.view.GameCombat
{
    public partial class MenuPrincipalCombat : Page
    {
        private Personnage _monHero;

        public MenuPrincipalCombat()
        {
            InitializeComponent();
            ChargerStatistiquesPersonnage();

            // On demande à la page de se mettre à jour à chaque fois qu'elle s'affiche à l'écran
            this.Loaded += MenuPrincipalCombat_Loaded;
        }

        private void MenuPrincipalCombat_Loaded(object sender, RoutedEventArgs e)
        {
            // On lit les données du coffre-fort et on les affiche
            TxtOrGlobal.Text = SauvegardeJoueur.OrTotal.ToString();
            TxtPierreGlobal.Text = SauvegardeJoueur.PierresTotal.ToString();
            TxtDiamantGlobal.Text = SauvegardeJoueur.DiamantsTotal.ToString();

            // Affichage de l'XP
            TxtXpGlobal.Text = $"{SauvegardeJoueur.ExpTotal} / 50";
            BarreXpGlobal.Value = SauvegardeJoueur.ExpTotal;

            // Plus tard, tu pourras ajouter la logique ici : si ExpTotal >= 50, alors Niveau++
        }

        private void ChargerStatistiquesPersonnage()
        {
            // On le crée une seule et unique fois ici !
            _monHero = new Personnage("Tung Tung Sahur", 10000, 10, 5, 10, 5, 150, 10000, true);

            // On met à jour les textes à l'écran
            TxtStatAttaque.Text = _monHero.Attaque.ToString();
            TxtStatSante.Text = _monHero.PointsDeVieMax.ToString();
            TxtStatVitesse.Text = _monHero.VitesseAttaque.ToString();
            TxtStatCritique.Text = _monHero.ChanceCritique.ToString() + "%";
        }

        private void BtnStartCombat_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem itemSelectionne = (ComboBoxItem)ComboDifficulte.SelectedItem;
            string difficulte = itemSelectionne.Content.ToString();

            if (difficulte == "Facile")
            {
                // LA MAGIE EST ICI : On passe _monHero dans les parenthèses pour l'envoyer au combat
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

            FondOngletStats.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#162C52"));
            FondOngletStats.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B62A4"));
            TexteOngletStats.Foreground = Brushes.White;

            ResetOngletCouleur(FondOngletArbre, TexteOngletArbre);
        }

        private void BtnOngletArbre_Click(object sender, RoutedEventArgs e)
        {
            PanneauArbre.Visibility = Visibility.Visible;
            PanneauStats.Visibility = Visibility.Collapsed;

            FondOngletArbre.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#162C52"));
            FondOngletArbre.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B62A4"));
            TexteOngletArbre.Foreground = Brushes.White;

            ResetOngletCouleur(FondOngletStats, TexteOngletStats);
        }

        private void ResetOngletCouleur(Border fond, TextBlock texte)
        {
            fond.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#151310"));
            fond.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2C241E"));
            texte.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A59C90"));
        }
    }
}