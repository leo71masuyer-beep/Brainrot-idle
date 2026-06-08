using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

public class MusicViewModel : INotifyPropertyChanged
{
    private MediaPlayer _player;
    private Track _currentTrack;

    public ObservableCollection<Track> Playlist { get; set; }

    public Track CurrentTrack
    {
        get => _currentTrack;
        set { _currentTrack = value; OnPropertyChanged(nameof(CurrentTrack)); }
    }

    public MusicViewModel()
    {
        _player = new MediaPlayer();
        _player.MediaEnded += (s, e) => SkipNext();

        // Exemple d'initialisation (remplace par tes vrais chemins)
        // Les chemins d'images peuvent être relatifs si elles sont dans ton projet (ex: "Images/cover1.jpg")
        Playlist = new ObservableCollection<Track>
{
            new Track {
                Title = "Boss_Level_Overdrive",
                FilePath = @"pack://application:,,,/Ressources/systememusic/music/Boss_Level_Overdrive.mp3",
                CoverPath = "pack://application:,,,/Ressources/systememusic/cover/Boss_Level_Overdrive.png"
            },
            new Track {
                Title = "Level_Failure",
                FilePath = @"pack://application:,,,/Ressources/systememusic/music/Level_Failure.mp3",
                CoverPath = "pack://application:,,,/Ressources/systememusic/cover/Level_Failure.png"
            },
            new Track {
                Title = "Panic_Button_Joyride",
                FilePath = @"pack://application:,,,/Ressources/systememusic/music/Panic_Button_Joyride.mp3",
                CoverPath = "pack://application:,,,/Ressources/systememusic/cover/Panic_Button_Joyride.png"
            },
            new Track {
                Title = "Sahur_Overdrive",
                FilePath = @"pack://application:,,,/Ressources/systememusic/music/Sahur_Overdrive.mp3",
                CoverPath = "pack://application:,,,/Ressources/systememusic/cover/Sahur_Overdrive.png"
            },
            new Track {
                Title = "Sahur_Speedrun",
                FilePath = @"pack://application:,,,/Ressources/systememusic/music/Sahur_Speedrun.mp3",
                CoverPath = "pack://application:,,,/Ressources/systememusic/cover/Sahur_Speedrun.png"
            },
            new Track {
                Title = "Turbo_Candy_Rampage",
                FilePath = @"pack://application:,,,/Ressources/systememusic/music/Turbo_Candy_Rampage.mp3",
                CoverPath = "pack://application:,,,/Ressources/systememusic/cover/Turbo_Candy_Rampage.png"
            }
        };

        if (Playlist.Any())
        {
            CurrentTrack = Playlist.First();
            PlayCurrentTrack();
        }
    }

    public void PlayCurrentTrack()
    {
        if (CurrentTrack != null)
        {
            _player.Open(new Uri(CurrentTrack.FilePath, UriKind.Absolute));
            _player.Play();
        }
    }

    public void SkipNext()
    {
        // S'il n'y a aucune musique cochée, on ne fait rien
        if (!Playlist.Any(t => t.IsSelected)) return;

        int currentIndex = Playlist.IndexOf(CurrentTrack);
        int nextIndex = (currentIndex + 1) % Playlist.Count;

        // Cherche la prochaine musique cochée
        while (!Playlist[nextIndex].IsSelected)
        {
            nextIndex = (nextIndex + 1) % Playlist.Count;
        }

        CurrentTrack = Playlist[nextIndex];
        PlayCurrentTrack();
    }

    public void SkipPrevious()
    {
        if (!Playlist.Any(t => t.IsSelected)) return;

        int currentIndex = Playlist.IndexOf(CurrentTrack);
        int prevIndex = currentIndex - 1;
        if (prevIndex < 0) prevIndex = Playlist.Count - 1;

        // Cherche la précédente musique cochée
        while (!Playlist[prevIndex].IsSelected)
        {
            prevIndex--;
            if (prevIndex < 0) prevIndex = Playlist.Count - 1;
        }

        CurrentTrack = Playlist[prevIndex];
        PlayCurrentTrack();
    }

    public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
    }
}