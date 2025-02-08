using SchoolManage.App.Views;

namespace SchoolManage.App
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            MainPage = new NavigationPage(new LoginPage());
        }

        public new static App Current => (App)Application.Current;
        public IServiceProvider Services => _serviceProvider;
    }
}
