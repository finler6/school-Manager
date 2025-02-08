using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SchoolManage.App.Messages;
using SchoolManage.App.Resources.Texts;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;
using System.Collections.ObjectModel;

namespace SchoolManage.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class StudentDetailViewModel : ViewModelBase, IRecipient<StudentEditMessage>
{
    private readonly IStudentFacade _studentFacade;
    private readonly INavigationService _navigationService;
    private readonly IMessengerService _messengerService;
    private readonly IAlertService _alertService;

    public StudentDetailViewModel(
        IStudentFacade studentFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)
    {
        _studentFacade = studentFacade;
        _navigationService = navigationService;
        _messengerService = messengerService;
        _alertService = alertService;
    }

    public Guid Id { get; set; }
    public StudentDetailModel? Student { get; private set; }
    public IEnumerable<EvaluationListModel>? Evaluations { get; private set; }
    public ObservableCollection<SubjectListModel> Subjects { get; private set; } = new ObservableCollection<SubjectListModel>();

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        if (Id == Guid.Empty)
        {
            return;
        }

        Student = await _studentFacade.GetAsync(Id);

        if (Student == null)
        {
            await _navigationService.GoToAsync("..");
            return;
        }

        Evaluations = await _studentFacade.GetStudentEvaluationsAsync(Student.Id);
        var studentSubjects = await _studentFacade.GetStudentSubjectsAsync(Student.Id);

        Subjects.Clear();
        foreach (var subject in studentSubjects)
        {
            Subjects.Add(subject);
        }

        Student.Subjects = new ObservableCollection<SubjectListModel>(Subjects);
    }



    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Student is not null)
        {
                await _studentFacade.DeleteAsync(Student.Id);
                MessengerService.Send(new StudentDeleteMessage());
                _navigationService.GoBackToMainPageAsync();
        }
    }

    [RelayCommand]
    private async Task GoToEditAsync()
    {
        await _navigationService.GoToAsync("/edit",
            new Dictionary<string, object?> { [nameof(StudentEditViewModel.Student)] = Student });
    }

    [RelayCommand]
    private async Task GoToSubjectDetailAsync(Guid subjectId)
    {
        await _navigationService.GoToAsync<SubjectDetailViewModel>(
            new Dictionary<string, object?> { [nameof(SubjectDetailViewModel.Id)] = subjectId });
    }

    public async void Receive(StudentEditMessage message)
    {
        if (message.StudentId == Student?.Id)
        {
            await LoadDataAsync();
        }
    }
}
