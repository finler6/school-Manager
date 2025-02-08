using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManage.App.Messages;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Mappers;
using SchoolManage.BL.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SchoolManage.App.ViewModels;

[QueryProperty(nameof(Subject), nameof(Subject))]
public partial class SubjectEditViewModel(
    ISubjectFacade subjectFacade,
    IStudentFacade studentFacade,
    ISubjectModelMapper subjectModelMapper,
    IStudentModelMapper studentModelMapper,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public SubjectDetailModel Subject { get; set; } = SubjectDetailModel.Empty;
    public ObservableCollection<StudentListModel> Students { get; set; } = new ObservableCollection<StudentListModel>();
    public ObservableCollection<Guid> SelectedStudentIds { get; } = new ObservableCollection<Guid>();
    private readonly List<Guid> assignedStudentIds = new List<Guid>();
    public bool creating = false;

    protected override async Task LoadDataAsync()
    {
        var subject = await subjectFacade.GetAsync(Subject.Id);
        if (subject != null)
        {
            Subject = subject;

            var allStudents = await studentFacade.GetAllStudentsAsync();
            assignedStudentIds.Clear();
            assignedStudentIds.AddRange(Subject.Students.Select(s => s.Id));
            Students.Clear();
            foreach (var student in allStudents)
            {
                Students.Add(student);
            }

            SelectedStudentIds.Clear();
            foreach (var student in Subject.Students)
            {
                SelectedStudentIds.Add(student.Id);
            }
            creating = false;
        }
        else
        {
            Subject = SubjectDetailModel.Empty;
            creating = true;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var updatedStudents = new ObservableCollection<StudentListModel>(
            Students.Where(student => SelectedStudentIds.Contains(student.Id)));
        var updatedSubject = new SubjectDetailModel
        {
            Id = Subject.Id,
            Name = Subject.Name,
            Acronym = Subject.Acronym,
            Students = updatedStudents
        };
        if (!creating)
        {
            await subjectFacade.UpdateSubjectAsync(Subject with { Students = updatedStudents });
            messengerService.Send(new SubjectEditMessage { SubjectId = updatedSubject.Id });
            await navigationService.GoBackAsync();
        }
        else
        {
            await subjectFacade.SaveAsync(Subject);
            messengerService.Send(new SubjectEditMessage { SubjectId = updatedSubject.Id });
            await navigationService.GoBackToMainPageAsync();
        }
        
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await navigationService.GoBackAsync();
    }

    public void ToggleStudentSelection(StudentListModel student)
    {
        if (SelectedStudentIds.Contains(student.Id))
        {
            SelectedStudentIds.Remove(student.Id);
        }
        else
        {
            SelectedStudentIds.Add(student.Id);
        }
    }

    public bool IsStudentSelected(Guid studentId)
    {
        return assignedStudentIds.Contains(studentId);
    }
}
