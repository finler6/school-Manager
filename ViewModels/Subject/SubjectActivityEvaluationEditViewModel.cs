using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SchoolManage.App.Messages;
using SchoolManage.App.Services;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Mappers;
using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SchoolManage.App.ViewModels;

[QueryProperty(nameof(Activity), nameof(Activity))]
public partial class SubjectActivityEvaluationEditViewModel(
    IStudentFacade studentFacade,
    ISubjectFacade subjectFacade,
    IEvaluationFacade evaluationFacade,
    ISubjectModelMapper subjectModelMapper,
    IStudentModelMapper studentModelMapper,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public ActivityListModel Activity { get; set; }
    public ObservableCollection<EvaluationListModel> Evaluations { get; set; } = new();
    public IEnumerable<StudentListModel> students { get; set; }

    protected override async Task LoadDataAsync()
    {
        var evaluations = await evaluationFacade.GetAllEvaluationsWithActivityIdAsync(Activity.Id);
        students = await studentFacade.GetAllStudentsWithSubjectAsync(Activity.Subjects);
        if (!evaluations.Any()) 
        {
            foreach (var student in students)
            {
                var sub = await subjectFacade.GetAsync(student.Id);
                var stud = await studentFacade.GetAsync(student.Id);
                var eval = new EvaluationListModel
                {
                    Points = "-",
                    Activities = new ActivityEntity
                    {
                        Id = Activity.Id,
                        Start = Activity.Start,
                        End = Activity.End,
                        Room = Activity.Room,
                        Type = Activity.Type,
                        SubjectsId = Activity.Subjects.Id,
                        Subjects = subjectModelMapper.MapToEntity(sub)
                    },
                    Students = studentModelMapper.MapToEntity(stud)
                };
                Evaluations.Add(eval);
            };
        }
        else
        {
            foreach (var evaluation in evaluations)
            {
                Evaluations.Add(evaluation);
            }
        }
  
    }
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!students.Any()) {
            foreach (var evaluation in Evaluations)
            {
                var evaluationsToSave = new EvaluationDetailModel
                {
                    Id = evaluation.Id,
                    Points = evaluation.Points,
                    Activities = evaluation.Activities,
                    Students = evaluation.Students,
                };
                await evaluationFacade.UpdateEvaluationAsync(evaluationsToSave);
            }
        }
        MessengerService.Send(new SubjectActivityEvaluationEditMessage());

        navigationService.SendBackButtonPressed();
    }
}