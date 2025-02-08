using SchoolManage.App.ViewModels;

namespace SchoolManage.App.Views.Student;

public partial class StudentListView
{
    public StudentListView(StudentListViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}