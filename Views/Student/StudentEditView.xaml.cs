using SchoolManage.App.ViewModels;

namespace SchoolManage.App.Views.Student;

public partial class StudentEditView : ContentPage
{
    public StudentEditView(StudentEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
