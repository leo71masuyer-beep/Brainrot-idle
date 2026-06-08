using System.ComponentModel;
using System.Runtime.CompilerServices;

public class Track : INotifyPropertyChanged
{
    private string _title;
    private string _filePath;
    private string _coverPath;
    private bool _isSelected = true;

    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); }
    }

    public string FilePath
    {
        get => _filePath;
        set { _filePath = value; OnPropertyChanged(); }
    }

    public string CoverPath
    {
        get => _coverPath;
        set { _coverPath = value; OnPropertyChanged(); }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set { _isSelected = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}