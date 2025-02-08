using SchoolManage.App.ViewModels;

namespace SchoolManage.App.Views
{
    public partial class MainPageView : ContentPage
    {
        public MainPageView()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            BindingContext = App.Current.Services.GetService<MainPageViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is MainPageViewModel viewModel)
            {
                await viewModel.OnNavigatedTo();
            }
        }
    }
}




