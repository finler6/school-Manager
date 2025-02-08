using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SchoolManage.App.Messages;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;

namespace SchoolManage.App.ViewModels
{
    public partial class SubjectTableViewModel : ViewModelBase, IRecipient<SubjectEditMessage>, IRecipient<SubjectDeleteMessage>
    {
        private readonly ISubjectFacade _subjectFacade;
        private readonly INavigationService _navigationService;

        public SubjectTableViewModel(ISubjectFacade subjectFacade, INavigationService navigationService, IMessengerService messengerService)
            : base(messengerService)
        {
            _subjectFacade = subjectFacade;
            _navigationService = navigationService;

            Messenger.Register<SubjectEditMessage>(this);
            Messenger.Register<SubjectDeleteMessage>(this);

            LoadDataAsync().ConfigureAwait(false);
        }

        public IEnumerable<SubjectListModel> Subjects { get; set; } = null!;

        protected override async Task LoadDataAsync()
        {
            Subjects = await _subjectFacade.GetAllSubjectsAsync();
            OnPropertyChanged(nameof(Subjects));
        }

        [RelayCommand]
        private async Task GoToDetailAsync(Guid id)
        {
            await _navigationService.GoToAsync<SubjectDetailViewModel>(
                new Dictionary<string, object?> { [nameof(SubjectDetailViewModel.Id)] = id });
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
}





