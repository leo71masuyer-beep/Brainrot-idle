using Brainrot_idle.Ressources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Brainrot_idle.view
{
    public partial class MainWindow : Window
    {
        public MusicViewModel GlobalMusicViewModel { get; set; }
        public static MediaPlayer player = new MediaPlayer();
        public static double CurrentVolume = 1.0; 

        public MainWindow()
        {
            InitializeComponent();
            GlobalMusicViewModel = new MusicViewModel();

            MainFrame.Navigate(new HomePage());
        }
    }
}