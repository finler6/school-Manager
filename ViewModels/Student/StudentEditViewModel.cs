using CommunityToolkit.Mvvm.Input;
using SchoolManage.App.Messages;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;
using System.Diagnostics;

namespace SchoolManage.App.ViewModels;

[QueryProperty(nameof(Student), nameof(Student))]
public partial class StudentEditViewModel(
    IStudentFacade studentFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public StudentDetailModel Student { get; init; } = StudentDetailModel.Empty;
    private bool CreateStudentNavigation = false;

    [RelayCommand]
    private async Task SaveAsync()
    {
        Debug.WriteLine($"Saving student: {Student.Id}, {Student.Name}, {Student.Surname}");
        Debug.WriteLine($"Subjects before save: {string.Join(", ", Student.Subjects.Select(s => s.Name))}");

        try
        {
            if (Student.Id == Guid.Empty)
            {
                await studentFacade.CreateStudentAsync(Student);
                CreateStudentNavigation = true;
                Debug.WriteLine($"New student created: {Student.Name}, {Student.Surname}");
            }
            else
            {
                await studentFacade.UpdateStudentAsync(Student);
                Debug.WriteLine($"Student updated: {Student.Id}, {Student.Name}, {Student.Surname}");
            }

            MessengerService.Send(new StudentEditMessage { StudentId = Student.Id });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error saving student: {ex.Message}");
        }
        if (CreateStudentNavigation == true)
        {
            await navigationService.GoBackToMainPageAsync();
        }
        else
        {
            await navigationService.GoToAsync("..");
        }
    }

    [RelayCommand]
    private async Task GoToSubjectEditAsync()
    {
        Debug.WriteLine($"Navigating to subject edit with student: {Student.Name}, {Student.Surname}");
        await navigationService.GoToAsync<StudentSubjectEditViewModel>(new Dictionary<string, object?> { { nameof(Student), Student } });
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Student.Id != Guid.Empty)
        {
            Debug.WriteLine($"Deleting student: {Student.Id}, {Student.Name}, {Student.Surname}");
            try
            {
                await studentFacade.DeleteStudentAsync(Student.Id);
                Debug.WriteLine($"Student deleted: {Student.Id}, {Student.Name}, {Student.Surname}");

                MessengerService.Send(new StudentEditMessage { StudentId = Student.Id });
                await navigationService.GoBackToMainPageAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting student: {ex.Message}");
            }
        }
    }
}
