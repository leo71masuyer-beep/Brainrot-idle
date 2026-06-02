using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Brainrot_idle.Ressources;

namespace Brainrot_idle.view
{
    public partial class StatistiquesFrames : Page
    {
        private DispatcherTimer affichageTimer;

        public StatistiquesFrames()
        {
            InitializeComponent();

            affichageTimer = new DispatcherTimer();
            affichageTimer.Interval = TimeSpan.FromMilliseconds(200);
            affichageTimer.Tick += AffichageTimer_Tick;

            Loaded += StatistiquesFrames_Loaded;
            Unloaded += StatistiquesFrames_Unloaded;
        }
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
        private void StatistiquesFrames_Loaded(object sender, RoutedEventArgs e)
        {
            affichageTimer.Start();
            MettreAJourStatistiques();
        }

        private void StatistiquesFrames_Unloaded(object sender, RoutedEventArgs e)
        {
            affichageTimer.Stop();
        }

        private void AffichageTimer_Tick(object sender, EventArgs e)
        {
            MettreAJourStatistiques();
        }

        private void MettreAJourStatistiques()
        {
            TxtPoints.Text = $"Points : {GameState.points:F2}";
            TxtAuraSec.Text = $"Aura/s : {GameState.auraParSeconde:F2}";
            TxtClicsSec.Text = $"Clics/s : {GameState.clicsCetteSeconde}";

            TxtNiveau.Text = $"Niveau : {GameState.Niveau}";
            TxtXp.Text = $"XP : {GameState.XpActuelle:F0} / {GameState.XpRequise:F0}";
            TxtPointsComp.Text = $"Points de comp. : {GameState.PointsDeCompetence}";

            TxtScoreSnake.Text = $"Record Snake : {GameState.MeilleurScoreSnake}";
            TxtMultXp.Text = $"Mult. XP : x{GameState.MultiplicateurXp:F1}";
            TxtMultAura.Text = $"Mult. Aura/Clic : x{GameState.MultiplicateurAuraParClic:F1}";
        }
    }
}