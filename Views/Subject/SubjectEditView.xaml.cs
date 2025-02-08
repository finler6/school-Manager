using SchoolManage.App.ViewModels;
using SchoolManage.BL.Models;

namespace SchoolManage.App.Views.Subject;

public partial class SubjectEditView
{
    public SubjectEditView(SubjectEditViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
    private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BindingContext is SubjectEditViewModel viewModel && sender is CheckBox checkBox && checkBox.BindingContext is StudentListModel student)
        {
            viewModel.ToggleStudentSelection(student);
        }
    }
}