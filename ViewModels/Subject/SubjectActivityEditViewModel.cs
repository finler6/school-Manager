using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SchoolManage.App.Messages;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Mappers;
using SchoolManage.BL.Models;
using SchoolManage.Common.Enums;
using SchoolManage.DAL.Entities;

namespace SchoolManage.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class SubjectActivityEditViewModel(
    IActivityFacade activityFacade,
    ISubjectFacade subjectFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public ActivityDetailModel Activity { get; set; } = ActivityDetailModel.Empty;
    public Guid Id { get; set; }

    public List<ActivityType> ActivityTypes { get; set; } = new((ActivityType[])Enum.GetValues(typeof(ActivityType)));
    public List<Room> Rooms { get; set; } = new((Room[])Enum.GetValues(typeof(Room)));

    public bool creating = false;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;
    [ObservableProperty]
    private TimeSpan selectedStartTime = TimeSpan.Zero;
    [ObservableProperty]
    private TimeSpan selectedEndTime = TimeSpan.Zero;

    protected override async Task LoadDataAsync()
    {
        var sub = await subjectFacade.GetAsync(Id);
        
        if (sub is not null)
        {
            creating = true;
            Activity = ActivityDetailModel.Empty;
        }
        else
        {
            creating = false;
            Activity = await activityFacade.GetAsync(Id);
            SelectedDate = Activity.Start.Date;
            SelectedStartTime = Activity.Start.TimeOfDay;
            SelectedEndTime = Activity.End.TimeOfDay;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Activity.Start = selectedDate + selectedStartTime;
        Activity.End = selectedDate + selectedEndTime;

        if (creating)
        {
            await activityFacade.SaveAsync(Activity with { Evaluations = default! }, Id);
        }
        else
        {
            await activityFacade.UpdateActivityAsync(Activity);
        }

        messengerService.Send(new SubjectActivityEditMassage());

        navigationService.SendBackButtonPressed();
    }
    
    public async void Receive(SubjectActivityEditMassage message)
    {
        await ReloadDataAsync();
    }
    
    private async Task ReloadDataAsync()
    {
        Activity = await activityFacade.GetAsync(Activity.Id)
                 ?? ActivityDetailModel.Empty;
    }
}