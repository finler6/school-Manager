using SchoolManage.App.Models;
using SchoolManage.App.ViewModels;
using SchoolManage.App.Views.Student;
using SchoolManage.App.Views.Subject;
using SchoolManage.App.Views.Evaluation;
using SchoolManage.App.Views.Activity;
using SchoolManage.App.Views;

namespace SchoolManage.App.Services;

public class NavigationService : INavigationService
{
    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//activities", typeof(ActivityListView), typeof(ActivityListViewModel)),
        new("//activities/activitydetail", typeof(ActivityDetailView), typeof(ActivityDetailViewModel)),
        new("//activities/activityedit", typeof(ActivityEditView), typeof(ActivityEditViewModel)),
        new("//activities/activitydetail/edit", typeof(ActivityEditView), typeof(ActivityEditViewModel)),

        new("//students", typeof(StudentListView), typeof(StudentListViewModel)),
        new("//mainpage/studentdetail", typeof(StudentDetailView), typeof(StudentDetailViewModel)),
        new("//mainpage/edit", typeof(StudentEditView), typeof(StudentEditViewModel)),
        new("//mainpage/studentdetail/edit", typeof(StudentEditView), typeof(StudentEditViewModel)),
        new("//mainpage/studentdetail/edit/subjectedit", typeof(StudentSubjectEditView), typeof(StudentSubjectEditViewModel)),

        new("//subjects", typeof(SubjectListView), typeof(SubjectListViewModel)),
        new("//mainpage/subjectdetail", typeof(SubjectDetailView), typeof(SubjectDetailViewModel)),
        new("//subjects/activity", typeof(SubjectActivityEditView), typeof(SubjectActivityEditViewModel)),
        new("//mainpage/subjectdetail/activity", typeof(SubjectActivityEditView), typeof(SubjectActivityEditViewModel)),
        new("//mainpage/subjectdetail/evaluate", typeof(SubjectActivityEvaluationEditView), typeof(SubjectActivityEvaluationEditViewModel)),
        new("//mainpage/editsub", typeof(SubjectEditView), typeof(SubjectEditViewModel)),
        new("//mainpage/subjectdetail/editsub", typeof(SubjectEditView), typeof(SubjectEditViewModel)),

        new("//evaluations", typeof(EvaluationListView), typeof(EvaluationListViewModel)),
        new("//evaluations/detail", typeof(EvaluationDetailView), typeof(EvaluationDetailViewModel)),
        new("//evaluations/edit", typeof(EvaluationEditView), typeof(EvaluationEditViewModel)),
        new("//evaluations/detail/edit", typeof(EvaluationEditView), typeof(EvaluationEditViewModel)),

        new("//mainpage", typeof(MainPageView), typeof(MainPageViewModel)),
    };

    public async Task GoToAsync<TViewModel>()
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route);
    }

    public async Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route, parameters);
    }

    public async Task GoToAsync(string route)
        => await Shell.Current.GoToAsync(route);

    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
        => await Shell.Current.GoToAsync(route, parameters);

    public bool SendBackButtonPressed()
        => Shell.Current.SendBackButtonPressed();

    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    public async Task GoBackToMainPageAsync()
    {
        await Shell.Current.GoToAsync("//mainpage");
    }

    private string GetRouteByViewModel<TViewModel>()
        where TViewModel : IViewModel
        => Routes.First(route => route.ViewModelType == typeof(TViewModel)).Route;
}
