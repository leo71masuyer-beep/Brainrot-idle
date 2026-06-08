using Brainrot_idle.view;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Brainrot_idle.Ressources.systememusic
{
    public class MusicViewModel : INotifyPropertyChanged
    {
        private Track _currentTrack;

        public ObservableCollection<Track> Playlist { get; set; }

        public Track CurrentTrack
        {
            get => _currentTrack;
            set
            {
                _currentTrack = value;
                OnPropertyChanged();
            }
        }

        public MusicViewModel()
        {
            // On connecte la fin de piste au lecteur global
            MainWindow.player.MediaEnded += (s, e) => SkipNext();

            // Récupère le dossier où le jeu s'exécute
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            Playlist = new ObservableCollection<Track>
            {
                new Track {
                    Title = "Boss_Level_Overdrive",
                    FilePath = Path.Combine(baseDir, "Ressources", "systememusic", "music", "Boss_Level_Overdrive.mp3"),
                    CoverPath = "pack://application:,,,/Ressources/systememusic/cover/Boss_Level_Overdrive.png"
                },
                new Track {
                    Title = "Level_Failure",
                    FilePath = Path.Combine(baseDir, "Ressources", "systememusic", "music", "Level_Failure.mp3"),
                    CoverPath = "pack://application:,,,/Ressources/systememusic/cover/Level_Failure.png"
                },
                new Track {
                    Title = "Panic_Button_Joyride",
                    FilePath = Path.Combine(baseDir, "Ressources", "systememusic", "music", "Panic_Button_Joyride.mp3"),
                    CoverPath = "pack://application:,,,/Ressources/systememusic/cover/Panic_Button_Joyride.png"
                },
                new Track {
                    Title = "Sahur_Overdrive",
                    FilePath = Path.Combine(baseDir, "Ressources", "systememusic", "music", "Sahur_Overdrive.mp3"),
                    CoverPath = "pack://application:,,,/Ressources/systememusic/cover/Sahur_Overdrive.png"
                },
                new Track {
                    Title = "Sahur_Speedrun",
                    FilePath = Path.Combine(baseDir, "Ressources", "systememusic", "music", "Sahur_Speedrun.mp3"),
                    CoverPath = "pack://application:,,,/Ressources/systememusic/cover/Sahur_Speedrun.png"
                },
                new Track {
                    Title = "Turbo_Candy_Rampage",
                    FilePath = Path.Combine(baseDir, "Ressources", "systememusic", "music", "Turbo_Candy_Rampage.mp3"),
                    CoverPath = "pack://application:,,,/Ressources/systememusic/cover/Turbo_Candy_Rampage.png"
                }
            };

            CurrentTrack = Playlist.First();
            PlayTrack(CurrentTrack);
        }

        // C'est cette méthode qui manquait à ton fichier !
        private void PlayTrack(Track track)
        {
            if (track == null) return;

            MainWindow.player.Stop();
            MainWindow.player.Open(new Uri(track.FilePath, UriKind.Absolute));
            MainWindow.player.Volume = MainWindow.CurrentVolume;
            MainWindow.player.Play();
        }

        public void SkipNext()
        {
            var activeTracks = Playlist.Where(t => t.IsSelected).ToList();
            if (!activeTracks.Any()) return;

            int index = activeTracks.IndexOf(CurrentTrack);
            if (index == -1 || index >= activeTracks.Count - 1)
                CurrentTrack = activeTracks.First();
            else
                CurrentTrack = activeTracks[index + 1];

            PlayTrack(CurrentTrack);
        }

        public void SkipPrevious()
        {
            var activeTracks = Playlist.Where(t => t.IsSelected).ToList();
            if (!activeTracks.Any()) return;

            int index = activeTracks.IndexOf(CurrentTrack);
            if (index <= 0)
                CurrentTrack = activeTracks.Last();
            else
                CurrentTrack = activeTracks[index - 1];

            PlayTrack(CurrentTrack);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}