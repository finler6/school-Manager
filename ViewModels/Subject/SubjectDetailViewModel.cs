using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SchoolManage.App.Messages;
using SchoolManage.App.Resources.Texts;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Mappers;
using SchoolManage.BL.Models;

namespace SchoolManage.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class SubjectDetailViewModel(
    ISubjectFacade subjectFacade,
    IActivityFacade activityFacade,
    ISubjectModelMapper subjectModelMapper,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<SubjectEditMessage>, IRecipient<SubjectActivityEditMassage>, IRecipient<SubjectActivityEvaluationEditMessage>
{
    public Guid Id { get; set; }
    public SubjectDetailModel? Subject { get; private set; }
    public ObservableCollection<SubjectActivityListViewModel> ActivityViewModels { get; set; } = null!;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Subject = await subjectFacade.GetAsync(Id);
        var sub = subjectModelMapper.MapToEntity(Subject);
        var activities = await subjectFacade.GetSubjectActivitiesAsync(Id);
        ActivityViewModels = new ObservableCollection<SubjectActivityListViewModel>(
                activities.Select(activity => new SubjectActivityListViewModel(activity, sub)));
    }

    [RelayCommand]
    private async Task DeleteSubjectAsync(Guid subjectId)
    {
        await subjectFacade.DeleteSubjectAsync(subjectId);
        
        MessengerService.Send(new SubjectDeleteMessage());

        navigationService.SendBackButtonPressed();
    }
    
    [RelayCommand]
    private async Task DeleteAsync(Guid activityId)
    {
        if (Subject is not null)
        {
                await activityFacade.DeleteAsync(activityId);
                await LoadDataAsync();
        }
    }

    [RelayCommand]
    private async Task GoToEditAsync()
    {
        await navigationService.GoToAsync("/editsub",
            new Dictionary<string, object?> { [nameof(SubjectEditViewModel.Subject)] = Subject });
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/activity",
            new Dictionary<string, object?> { [nameof(SubjectActivityEditViewModel.Id)] = Id });
    }
    
    [RelayCommand]
    private async Task GoToEditActivityAsync(Guid activityId)
    {
        await navigationService.GoToAsync("/activity",
            new Dictionary<string, object?> { [nameof(SubjectActivityEditViewModel.Id)] = activityId});
    }
    
    [RelayCommand]
    private async Task GoToEvaluateAsync(ActivityListModel activity)
    {
        await navigationService.GoToAsync("/evaluate",
            new Dictionary<string, object?> { [nameof(SubjectActivityEvaluationEditViewModel.Activity)] = activity });
    }

    public async void Receive(SubjectEditMessage message)
    {
        if (message.SubjectId == Subject?.Id)
        {
            await LoadDataAsync();
        }
    }

    public async void Receive(SubjectActivityEditMassage massage)
    {
        await LoadDataAsync();
    }

    public async void Receive(SubjectActivityEvaluationEditMessage massage)
    {
        await LoadDataAsync();
    }
}
