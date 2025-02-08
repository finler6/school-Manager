using CommunityToolkit.Mvvm.Input;
using SchoolManage.App.Messages;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;
using System.Diagnostics;

namespace SchoolManage.App.ViewModels;

[QueryProperty(nameof(Activity), nameof(Activity))]
public partial class ActivityEditViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public ActivityDetailModel Activity { get; init; } = ActivityDetailModel.Empty;

    [RelayCommand]
    private async Task SaveAsync()
    {
        await activityFacade.SaveAsync(Activity);

        MessengerService.Send(new ActivityEditMessage { ActivityId = Activity.Id });

        navigationService.SendBackButtonPressed();
    }
}
