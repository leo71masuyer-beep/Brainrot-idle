using Brainrot_idle.Ressources;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Brainrot_idle.view
{
    public partial class MusicFrame : Page
    {
        private int currentMusicIndex = 0;

        private readonly Dictionary<string, string> musicImages = new()
        {
            {
                "BossLevel",
                "pack://application:,,,/Brainrot_idle;component/Ressources/cover/boss_level_overdrive.PNG"
            },
            {
                "LevelFailure",
                "pack://application:,,,/Brainrot_idle;component/Ressources/cover/level_failure.PNG"
            },
            {
                "PanicButton",
                "pack://application:,,,/Brainrot_idle;component/Ressources/cover/panic_button.PNG"
            },
            {
                "SahurOverdrive",
                "pack://application:,,,/Brainrot_idle;component/Ressources/cover/sahur_overdrive.PNG"
            },
            {
                "TurboCandy",
                "pack://application:,,,/Brainrot_idle;component/Ressources/cover/turbo_candy.PNG"
            }
        };

        public MusicFrame()
        {
            InitializeComponent();

            ChargerListeMusiques();

            if (GameState.MusiquesDisponibles.Count > 0)
            {
                UpdateCurrentMusicDisplay();
            }
        }

        private void UpdateCurrentMusicDisplay()
        {
            if (GameState.MusiquesDisponibles.Count == 0)
            {
                CurrentMusicText.Text = "Aucune musique";
                CurrentMusicImage.Source = null;
                return;
            }

            string musique =
                GameState.MusiquesDisponibles[currentMusicIndex];

            CurrentMusicText.Text = musique;

            try
            {
                if (musicImages.TryGetValue(musique, out string imagePath))
                {
                    CurrentMusicImage.Source =
                        new BitmapImage(new Uri(imagePath));
                }
                else
                {
                    CurrentMusicImage.Source = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur chargement image :\n{ex.Message}");

                CurrentMusicImage.Source = null;
            }
        }

        private void ChargerListeMusiques()
        {
            MusicListPanel.Children.Clear();

            foreach (string musique in GameState.MusiquesDisponibles)
            {
                CheckBox cb = new CheckBox
                {
                    Content = musique,
                    Margin = new Thickness(5),
                    IsChecked =
                        GameState.MusiquesActives.Contains(musique)
                };

                cb.Checked += Music_Checked;
                cb.Unchecked += Music_Unchecked;

                MusicListPanel.Children.Add(cb);
            }
        }

        private void Music_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (!GameState.MusiquesActives.Contains(
                    cb.Content.ToString()))
            {
                GameState.MusiquesActives.Add(
                    cb.Content.ToString());
            }
        }

        private void Music_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            GameState.MusiquesActives.Remove(
                cb.Content.ToString());
        }

        private void SkipButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.SkipCurrentMusic();

            currentMusicIndex++;

            if (currentMusicIndex >=
                GameState.MusiquesDisponibles.Count)
            {
                currentMusicIndex = 0;
            }

            UpdateCurrentMusicDisplay();
        }

        private void PreviousButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow.PreviousMusic();

            currentMusicIndex--;

            if (currentMusicIndex < 0)
            {
                currentMusicIndex =
                    GameState.MusiquesDisponibles.Count - 1;
            }

            UpdateCurrentMusicDisplay();
        }

        private void Return_Button_Click(
            object sender,
            RoutedEventArgs e)
        {
            NavigationService?.Navigate(new HomePage());
        }
    }
}