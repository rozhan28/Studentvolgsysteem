using System.ComponentModel;
using System.Runtime.CompilerServices;

public class Criterium : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public int Id { get; set; }

    public string Beschrijving { get; set; }

    public string Niveau { get; set; }

    private bool _isGeselecteerd;
    public bool IsGeselecteerd
    {
        get => _isGeselecteerd;
        set
        {
            if (_isGeselecteerd == value) return;
            _isGeselecteerd = value;
            OnPropertyChanged();
        }
    }
    private string _toelichting;
    public string Toelichting
    {
        get => _toelichting;
        set
        {
            if (_toelichting == value) return;
            _toelichting = value;
            OnPropertyChanged(); 
        }
    }

    protected void OnPropertyChanged(
        [CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
