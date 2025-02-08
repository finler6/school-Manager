using SchoolManage.App.ViewModels;

namespace SchoolManage.App.Views.Evaluation;

public partial class EvaluationListView
{
    public EvaluationListView(EvaluationListViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}