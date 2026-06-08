using System.Windows;
using System.Windows.Controls;
using Brainrot_idle.view;
using Brainrot_idle.Ressources;


namespace Brainrot_idle.view
{

    public partial class MusicFrame : Page
    {
        private MusicViewModel _viewModel;

        // Constructeur par défaut (utilisé par la navigation WPF)
        // Retire simplement le "void" !
        public MusicFrame()
        {
            InitializeComponent();
            _viewModel = new MusicViewModel();
            DataContext = _viewModel;
        }

        public MusicFrame(MusicViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SkipPrevious();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SkipNext();
        }


        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage());
        }
    }
}