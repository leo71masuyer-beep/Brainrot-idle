using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Brainrot_idle.view.GameCombat
{
    /// <summary>
    /// Logique d'interaction pour MenuPrincipalCombat.xaml
    /// </summary>
    public partial class MenuPrincipalCombat : Page
    {
        public MenuPrincipalCombat()
        {
            InitializeComponent();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            // On ferme la page et on retourne au menu précédent
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void BtnOngletStats_Click(object sender, RoutedEventArgs e)
        {
            // 1. On affiche le panneau des stats, on cache l'arbre
            PanneauStats.Visibility = Visibility.Visible;
            PanneauArbre.Visibility = Visibility.Collapsed;

            // 2. L'onglet Stats devient BLEU (actif)
            FondOngletStats.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#162C52"));
            FondOngletStats.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B62A4"));
            TexteOngletStats.Foreground = Brushes.White;

            // 3. L'onglet Arbre devient GRIS FONCÉ (inactif)
            FondOngletArbre.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#151310"));
            FondOngletArbre.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2C241E"));
            TexteOngletArbre.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A59C90"));
        }

        private void BtnOngletArbre_Click(object sender, RoutedEventArgs e)
        {
            // 1. On affiche l'arbre, on cache les stats
            PanneauArbre.Visibility = Visibility.Visible;
            PanneauStats.Visibility = Visibility.Collapsed;

            // 2. L'onglet Arbre devient BLEU (actif)
            FondOngletArbre.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#162C52"));
            FondOngletArbre.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B62A4"));
            TexteOngletArbre.Foreground = Brushes.White;

            // 3. L'onglet Stats devient GRIS FONCÉ (inactif)
            FondOngletStats.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#151310"));
            FondOngletStats.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2C241E"));
            TexteOngletStats.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A59C90"));
        }
    }
}
