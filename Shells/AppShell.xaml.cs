using CommunityToolkit.Mvvm.Input;
using SchoolManage.App.Services;
using SchoolManage.App.ViewModels;
using Microsoft.Maui.Controls;

namespace SchoolManage.App.Shells
{
    public partial class AppShell : Shell
    {
        private readonly INavigationService _navigationService;

        public AppShell(INavigationService navigationService)
        {
            _navigationService = navigationService;
            InitializeComponent();
        }

        [RelayCommand]
        private async Task GoToStudentsAsync()
            => await _navigationService.GoToAsync<StudentListViewModel>();

        [RelayCommand]
        private async Task GoToSubjectsAsync()
            => await _navigationService.GoToAsync<SubjectListViewModel>();

        [RelayCommand]
        private async Task GoToActivitiesAsync()
            => await _navigationService.GoToAsync<ActivityListViewModel>();

        [RelayCommand]
        private async Task GoToMainAsync()
            => await _navigationService.GoToAsync<MainPageViewModel>();
    }
}
