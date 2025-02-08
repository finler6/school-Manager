using SchoolManage.App.ViewModels;

namespace SchoolManage.App.Views.Activity;

public partial class ActivityListView
{
    public ActivityListView(ActivityListViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}