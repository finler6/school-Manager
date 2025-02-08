using SchoolManage.App.Models;
using SchoolManage.App.ViewModels;

namespace SchoolManage.App.Services;

public interface INavigationService
{
    IEnumerable<RouteModel> Routes { get; }

    Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
        where TViewModel : IViewModel;

    Task GoToAsync(string route);
    bool SendBackButtonPressed();
    Task GoToAsync(string route, IDictionary<string, object?> parameters);
    Task GoToAsync<TViewModel>()
        where TViewModel : IViewModel;
    Task GoBackAsync();
    Task GoBackToMainPageAsync();
}

