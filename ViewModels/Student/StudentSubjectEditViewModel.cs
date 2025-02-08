using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManage.App.Messages;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SchoolManage.App.ViewModels;

[QueryProperty(nameof(Student), nameof(Student))]
public partial class StudentSubjectEditViewModel : ViewModelBase
{
    private readonly ISubjectFacade _subjectFacade;
    private readonly IStudentFacade _studentFacade;
    private readonly INavigationService _navigationService;
    private readonly IMessengerService _messengerService;

    public StudentSubjectEditViewModel(
        ISubjectFacade subjectFacade,
        IStudentFacade studentFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _subjectFacade = subjectFacade;
        _studentFacade = studentFacade;
        _navigationService = navigationService;
        _messengerService = messengerService;
    }

    public StudentDetailModel Student { get; set; } = StudentDetailModel.Empty;
    public ObservableCollection<SubjectListModel> AllSubjects { get; } = new();
    public ObservableCollection<Guid> SelectedSubjectIds { get; } = new();

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        var subjects = await _subjectFacade.GetAllSubjectsAsync();
        AllSubjects.Clear();
        foreach (var subject in subjects)
        {
            AllSubjects.Add(subject);
        }

        SelectedSubjectIds.Clear();
        foreach (var subject in Student.Subjects)
        {
            SelectedSubjectIds.Add(subject.Id);
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Student.Subjects = new ObservableCollection<SubjectListModel>(
            AllSubjects.Where(subject => SelectedSubjectIds.Contains(subject.Id)));
        await _studentFacade.UpdateStudentAsync(Student);
        _messengerService.Send(new StudentEditMessage { StudentId = Student.Id });
        await _navigationService.GoBackAsync();
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await _navigationService.GoBackAsync();
    }

    public void ToggleSubjectSelection(SubjectListModel subject)
    {
        if (SelectedSubjectIds.Contains(subject.Id))
        {
            SelectedSubjectIds.Remove(subject.Id);
        }
        else
        {
            SelectedSubjectIds.Add(subject.Id);
        }
    }

    public bool IsSubjectSelected(Guid subjectId)
    {
        return SelectedSubjectIds.Contains(subjectId);
    }
}
