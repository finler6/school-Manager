using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class MainPageViewModel : ViewModelBase, IRecipient<SubjectEditMessage>, IRecipient<SubjectDeleteMessage>, IRecipient<StudentEditMessage>
    {
        private readonly ISubjectFacade _subjectFacade;
        private readonly IStudentFacade _studentFacade;
        private readonly INavigationService _navigationService;

        public MainPageViewModel(
            ISubjectFacade subjectFacade,
            IStudentFacade studentFacade,
            INavigationService navigationService,
            IMessengerService messengerService)
            : base(messengerService)
        {
            _subjectFacade = subjectFacade;
            _studentFacade = studentFacade;
            _navigationService = navigationService;

            Subjects = new List<SubjectListModel>();
            Students = new List<StudentListModel>();
        }

        public IEnumerable<SubjectListModel> Subjects { get; set; }
        public IEnumerable<StudentListModel> Students { get; set; }

        [ObservableProperty]
        private string studentSearchText;

        protected override async Task LoadDataAsync()
        {
            await base.LoadDataAsync();
            Debug.WriteLine("MainPageViewModel: Loading data...");

            try
            {
                Subjects = await _subjectFacade.GetAllSubjectsAsync();
                Debug.WriteLine("MainPageViewModel: Subjects loaded successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MainPageViewModel: Error loading subjects: {ex.Message}");
            }
        }

        private void LogStudents()
        {
            Debug.WriteLine("Current students:");
            foreach (var student in Students)
            {
                Debug.WriteLine($"Student: {student.Id}, {student.Name}, {student.Surname}");
            }
        }

        [RelayCommand]
        public async Task SearchStudentsAsync()
        {
            if (string.IsNullOrWhiteSpace(StudentSearchText))
            {
                Students = new List<StudentListModel>();
                return;
            }

            try
            {
                Debug.WriteLine($"Searching for students with query: {StudentSearchText}");
                Students = await _studentFacade.SearchStudentsAsync(StudentSearchText);
                Debug.WriteLine($"Found {Students.Count()} students");
                LogStudents();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred while searching for students: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task GoToStudentDetailAsync(Guid studentId)
        {
            await _navigationService.GoToAsync<StudentDetailViewModel>(
                new Dictionary<string, object?> { [nameof(StudentDetailViewModel.Id)] = studentId });
        }

        [RelayCommand]
        private async Task GoToCreateStudentAsync()
        {
            var newStudent = StudentDetailModel.Empty;
            await _navigationService.GoToAsync<StudentEditViewModel>(
                new Dictionary<string, object?> { [nameof(StudentEditViewModel.Student)] = newStudent });
        }
        
        [RelayCommand]
        private async Task GoToCreateSubjectAsync()
        {
            await _navigationService.GoToAsync<SubjectEditViewModel>();
        }

        [RelayCommand]
        private async Task GoToDetailAsync(Guid id)
        {
            await _navigationService.GoToAsync<SubjectDetailViewModel>(
                new Dictionary<string, object?> { [nameof(SubjectDetailViewModel.Id)] = id });
        }

        public async void Receive(SubjectEditMessage message)
        {
            Debug.WriteLine("Received SubjectEditMessage");
            await LoadDataAsync();
        }

        public async void Receive(SubjectDeleteMessage message)
        {
            Debug.WriteLine("Received SubjectDeleteMessage");
            await LoadDataAsync();
        }

        public async void Receive(StudentEditMessage message)
        {
            Debug.WriteLine("Received StudentEditMessage");
            await LoadDataAsync();
        }

        public async Task OnNavigatedTo()
        {
            Debug.WriteLine("MainPageViewModel: Navigated to MainPage, reloading data...");
            await LoadDataAsync();
        }
    }
}
