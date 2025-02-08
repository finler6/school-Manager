using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Mappers;
using SchoolManage.BL.Models;
using SchoolManage.Common.Enums;
using SchoolManage.DAL.Entities;
using System.Diagnostics;
using System.Windows.Input;

namespace SchoolManage.App.ViewModels
{
    public partial class SubjectActivityListViewModel : ObservableObject
    {
        public ActivityListModel Activity { get; }
        public SubjectEntity Subject { get; }

        [ObservableProperty]
        private bool isDetailVisible;
        [ObservableProperty]
        public DateTime mainDate;

        public SubjectActivityListViewModel(ActivityListModel activity, SubjectEntity subject)
        {
            Activity = activity;
            Subject = subject;
            Activity = Activity with { Subjects = subject };

            mainDate = activity.Start;
            if (Activity.Type == ActivityType.Homework)
            {
                mainDate = Activity.End;
            }
            IsDetailVisible = false;
            ToggleDetailCommand = new RelayCommand(ToggleDetail);
        }

        public ICommand ToggleDetailCommand { get; }

        private void ToggleDetail()
        {
            IsDetailVisible = !IsDetailVisible;
        }
    }
}