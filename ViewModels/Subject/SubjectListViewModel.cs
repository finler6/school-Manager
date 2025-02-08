using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SchoolManage.App.Messages;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;

namespace SchoolManage.App.ViewModels;

public partial class SubjectListViewModel(
    ISubjectFacade subjectFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<SubjectEditMessage>, IRecipient<SubjectDeleteMessage>
{
    public IEnumerable<SubjectListModel> Subject { get; set; } = null!;
    public IEnumerable<ActivityListModel> Activities { get; set; } = null!;

    [ObservableProperty] private string _searchText;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Subject = await subjectFacade.GetAsync();
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/editsub");
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync<SubjectDetailViewModel>(
            new Dictionary<string, object?> { [nameof(SubjectDetailViewModel.Id)] = id });
    }
    
    [RelayCommand]
    private async Task SearchAsync()
    {
        Subject = await subjectFacade.SearchSubjectsAsync(SearchText);
    }

    public async void Receive(SubjectEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(SubjectDeleteMessage message)
    {
        await LoadDataAsync();
    }
}
