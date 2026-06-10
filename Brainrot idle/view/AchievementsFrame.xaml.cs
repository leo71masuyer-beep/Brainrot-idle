using Brainrot_idle.Ressources;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Brainrot_idle.view
{
    public partial class AchievementsFrame : Page
    {
        private DispatcherTimer refreshTimer;

        public AchievementsFrame()
        {
            InitializeComponent();

            // On actualise la page régulièrement pour voir les succès se débloquer en direct
            refreshTimer = new DispatcherTimer();
            refreshTimer.Interval = TimeSpan.FromMilliseconds(500);
            refreshTimer.Tick += RefreshTimer_Tick;

            Loaded += AchievementsFrame_Loaded;
            Unloaded += AchievementsFrame_Unloaded;
        }

        private void AchievementsFrame_Loaded(object sender, RoutedEventArgs e)
        {
            GenererAffichageSucces();
            refreshTimer.Start();
        }

        private void AchievementsFrame_Unloaded(object sender, RoutedEventArgs e)
        {
            refreshTimer.Stop();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            GenererAffichageSucces();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            // Modifie avec ta frame d'origine (ex: NavigationService.GoBack() ou nouvelle instance)
            NavigationService.GoBack();
        }

        private void GenererAffichageSucces()
        {
            ConteneurSucces.Children.Clear(); // On vide avant de redessiner

            // On convertit nos codes couleurs hexadécimaux en objets Brush compréhensibles par WPF
            Brush bgDebloque = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A0028"));
            Brush bordureDebloque = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7A00FF"));
            Brush texteDebloque = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCFF00"));

            Brush bgVerrouille = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            Brush bordureVerrouille = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            Brush texteVerrouille = Brushes.Gray;

            foreach (var succes in GameState.ListeSucces)
            {
                // La carte (Border)
                Border carte = new Border
                {
                    Width = 220,
                    Height = 120,
                    Margin = new Thickness(10),
                    CornerRadius = new CornerRadius(10),
                    BorderThickness = new Thickness(2),
                    Background = succes.EstDebloque ? bgDebloque : bgVerrouille,
                    BorderBrush = succes.EstDebloque ? bordureDebloque : bordureVerrouille,
                    Padding = new Thickness(10)
                };

                // L'organisation du texte dedans (StackPanel)
                StackPanel panel = new StackPanel { VerticalAlignment = VerticalAlignment.Center };

                TextBlock txtTitre = new TextBlock
                {
                    Text = (succes.EstSecret && !succes.EstDebloque) ? "???" : succes.Titre,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Foreground = succes.EstDebloque ? texteDebloque : texteVerrouille,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center
                };

                TextBlock txtDesc = new TextBlock
                {
                    Text = (succes.EstSecret && !succes.EstDebloque) ? "Succès caché..." : succes.Description,
                    FontSize = 12,
                    Foreground = Brushes.White,
                    Margin = new Thickness(0, 10, 0, 5),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center
                };

                TextBlock txtRecompense = new TextBlock
                {
                    Text = $"+ {succes.RecompensePoints} Points",
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Foreground = succes.EstDebloque ? Brushes.LightGreen : texteVerrouille,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                panel.Children.Add(txtTitre);
                panel.Children.Add(txtDesc);
                panel.Children.Add(txtRecompense);
                carte.Child = panel;

                ConteneurSucces.Children.Add(carte);
            }
        }
    }
}