using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.ViewModels;

// Handelt de niveaus en de criteria af
public partial class CriteriumViewModel : BasisViewModel, INotifyPropertyChanged
{
    private readonly ICriteriumService _criteriumService;
    private Prestatiedoel _prestatiedoel;
    
    public List<Criterium> OpNiveauCriteria { get; set; }
    public List<Criterium> BovenNiveauCriteria { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;
    
    private bool _isPrestatieNiveauInvalid;
    public bool IsPrestatieNiveauInvalid //Rode box om het criterium menu
    {
        get => _isPrestatieNiveauInvalid;
        set { _isPrestatieNiveauInvalid = value; Notify(); }
    }
    private async Task InitialiseerPaginaAsync()
    {
        try
        {
            LaadCriteria();
        }
        catch (Exception ex)
        {
            StatusMelding = "Fout bij laden formulier.";
            Debug.WriteLine(ex);
        }
    }

    private void LaadCriteria()
    {
        OpNiveauCriteria = CriteriaBijNiveau(Niveauaanduiding.OpNiveau);
        BovenNiveauCriteria = CriteriaBijNiveau(Niveauaanduiding.BovenNiveau);
    }

    public CriteriumViewModel(Prestatiedoel prestatiedoel, ICriteriumService criteriumService)
    {
        _criteriumService = criteriumService;
        _prestatiedoel = prestatiedoel;
        InitialiseerPaginaAsync();
    }

    private List<Criterium> CriteriaBijNiveau(Niveauaanduiding niveauaanduiding)
    {
        return _prestatiedoel.Criteria.Where(c => c.Niveau == niveauaanduiding).ToList();
    }
    public string PrestatieNiveau
    {
        get
        {
            if (IsBovenNiveau) return "Boven niveau";
            if (IsOpNiveau) return "Op niveau";
            if (InOntwikkeling) return "In ontwikkeling";
            return string.Empty;
        }
    }
    
    private bool _inOntwikkeling;
    public bool InOntwikkeling
    {
        get => _inOntwikkeling;
        set
        {
            if (_inOntwikkeling == value) return;
            _inOntwikkeling = value;
            UpdateStatus();
            Notify();
        }
    }
    
    public bool IsOpNiveau => !InOntwikkeling && CriteriaBijNiveau(Niveauaanduiding.OpNiveau).Any(c => c.IsGeselecteerd);
    public bool IsBovenNiveau => !InOntwikkeling && CriteriaBijNiveau(Niveauaanduiding.BovenNiveau).Any(c => c.IsGeselecteerd);
    
    public void UpdateStatus()
    {
        if (InOntwikkeling)
        {
            GeselecteerdNiveau = Niveauaanduiding.InOntwikkeling;
        }
        else if (IsBovenNiveau)
        {
            GeselecteerdNiveau = Niveauaanduiding.BovenNiveau;
        }
        else if (IsOpNiveau)
        {
            GeselecteerdNiveau = Niveauaanduiding.OpNiveau;
        }
        else
        {
            GeselecteerdNiveau = Niveauaanduiding.NietIngeleverd;
        }
    }
    
    private Niveauaanduiding _geselecteerdNiveau = Niveauaanduiding.NietIngeleverd;
    
    public Niveauaanduiding GeselecteerdNiveau
    {
        get => _geselecteerdNiveau;
        set
        {
            if (_geselecteerdNiveau == value) return;
            _geselecteerdNiveau = value;
            Notify(nameof(GeselecteerdNiveau));
            Notify(nameof(PrestatieNiveau));
        }
    }
    
    public bool CheckValidatie()
    {
        bool niveauOk = InOntwikkeling || IsOpNiveau || IsBovenNiveau;
        IsPrestatieNiveauInvalid = !niveauOk;

        return niveauOk;
    }
    
    private void Notify([CallerMemberName] string prop = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}