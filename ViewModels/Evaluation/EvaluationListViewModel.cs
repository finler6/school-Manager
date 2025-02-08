using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SchoolManage.App.Messages;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;

namespace SchoolManage.App.ViewModels;

public partial class EvaluationListViewModel(
    IEvaluationFacade evaluationFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<EvaluationEditMessage>, IRecipient<EvaluationDeleteMessage>
{
    public IEnumerable<EvaluationListModel> Evaluation { get; set; } = null!;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Evaluation = await evaluationFacade.GetAsync();
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/edit");
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync<EvaluationDetailViewModel>(
            new Dictionary<string, object?> { [nameof(EvaluationDetailViewModel.Id)] = id });
    }

    public async void Receive(EvaluationEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(EvaluationDeleteMessage message)
    {
        await LoadDataAsync();
    }
}
