using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SchoolManage.App.Messages;
using SchoolManage.App.Resources.Texts;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;

namespace SchoolManage.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class EvaluationDetailViewModel(
    IEvaluationFacade evaluationFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<EvaluationEditMessage>
{
    public Guid Id { get; set; }
    public EvaluationDetailModel? Evaluation { get; private set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Evaluation = await evaluationFacade.GetAsync(Id);
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Evaluation is not null)
        {
                await evaluationFacade.DeleteAsync(Evaluation.Id);
                MessengerService.Send(new EvaluationDeleteMessage());
                navigationService.SendBackButtonPressed();
        }
    }

    [RelayCommand]
    private async Task GoToEditAsync()
    {
        await navigationService.GoToAsync("/edit",
            new Dictionary<string, object?> { [nameof(EvaluationEditViewModel.Evaluation)] = Evaluation });
    }

    public async void Receive(EvaluationEditMessage message)
    {
        if (message.EvaluationId == Evaluation?.Id)
        {
            await LoadDataAsync();
        }
    }
}
