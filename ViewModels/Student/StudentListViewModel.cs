using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SchoolManage.App.Messages;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;

namespace SchoolManage.App.ViewModels;

public partial class StudentListViewModel : ViewModelBase, IRecipient<StudentEditMessage>, IRecipient<StudentDeleteMessage>
{
    private readonly IStudentFacade _studentFacade;
    private readonly INavigationService _navigationService;
    private readonly IMessengerService _messengerService;

    public StudentListViewModel(
        IStudentFacade studentFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _studentFacade = studentFacade;
        _navigationService = navigationService;
        _messengerService = messengerService;
        Students = new ObservableCollection<StudentListModel>();
    }

    public ObservableCollection<StudentListModel> Students { get; set; }

    public async Task InitializeAsync()
    {
        await LoadDataAsync();
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        var students = await _studentFacade.GetAsync();
        Students.Clear();
        foreach (var student in students)
        {
            Students.Add(student);
        }
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await _navigationService.GoToAsync<StudentEditViewModel>();
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {

        await _navigationService.GoToAsync<StudentDetailViewModel>(
            new Dictionary<string, object?> { [nameof(StudentDetailViewModel.Id)] = id });

    }

    public async void Receive(StudentEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(StudentDeleteMessage message)
    {
        await LoadDataAsync();
    }
}
