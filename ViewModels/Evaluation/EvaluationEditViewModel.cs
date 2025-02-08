using CommunityToolkit.Mvvm.Input;
using SchoolManage.App.Messages;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;

namespace SchoolManage.App.ViewModels;

[QueryProperty(nameof(Evaluation), nameof(Evaluation))]
public partial class EvaluationEditViewModel(
    IEvaluationFacade evaluationFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public EvaluationDetailModel Evaluation { get; init; } = EvaluationDetailModel.Empty;

    [RelayCommand]
    private async Task SaveAsync()
    {
        await evaluationFacade.SaveAsync(Evaluation);

        MessengerService.Send(new EvaluationEditMessage { EvaluationId = Evaluation.Id });

        navigationService.SendBackButtonPressed();
    }
}
