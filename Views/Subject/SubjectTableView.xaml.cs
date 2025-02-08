using SchoolManage.App.ViewModels;
using SchoolManage.BL.Models;

namespace SchoolManage.App.Views.Subject
{
    public partial class SubjectTableView : ContentView
    {
        public SubjectTableView()
        {
            InitializeComponent();
            BindingContext = App.Current.Services.GetService<SubjectTableViewModel>();
        }
    }
}




