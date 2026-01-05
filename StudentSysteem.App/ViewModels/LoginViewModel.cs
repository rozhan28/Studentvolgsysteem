using CommunityToolkit.Mvvm.Input;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.ViewModels;

public partial class LoginViewModel : BasisViewModel
{
    private readonly GlobaleViewModel _globaal;
    private readonly IStudentService _studentService;
    private readonly IDocentService _docentService;

    public LoginViewModel(GlobaleViewModel globaal, IStudentService studentService, IDocentService docentService)
    {
        _globaal = globaal;
        _studentService = studentService;
        _docentService = docentService;
    }

    [RelayCommand]
    private void LoginStudent()
    {
        Student student = _studentService.LoginStudent();
        if (student != null)
        {
            _globaal.IngelogdeGebruiker = student;
            Application.Current.MainPage = new AppShell(_globaal);
        }
    }

    [RelayCommand]
    private void LoginDocent()
    {
        Docent docent = _docentService.LoginDocent();
        if (docent != null)
        {
            _globaal.IngelogdeGebruiker = docent;
            Application.Current.MainPage = new AppShell(_globaal);
        }
    }
}