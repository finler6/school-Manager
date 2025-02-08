using SchoolManage.App.ViewModels;
using SchoolManage.BL.Models;

namespace SchoolManage.App.Views.Student;

public partial class StudentSubjectEditView : ContentPage
{
    public StudentSubjectEditView(StudentSubjectEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        if (BindingContext is StudentSubjectEditViewModel viewModel)
        {
            viewModel.LoadDataCommand.Execute(null);
        }
    }

    private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BindingContext is StudentSubjectEditViewModel viewModel && sender is CheckBox checkBox && checkBox.BindingContext is SubjectListModel subject)
        {
            viewModel.ToggleSubjectSelection(subject);
        }
    }
}
