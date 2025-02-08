using SchoolManage.App.Shells;
using SchoolManage.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SchoolManage.App.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (IsValidUser(username, password))
            {
                lblErrorMessage.IsVisible = false;
                var navigationService = App.Current.Services.GetRequiredService<INavigationService>();
                Application.Current.MainPage = new AppShell(navigationService);
            }
            else
            {
                lblErrorMessage.IsVisible = true;
                txtUsername.Text = string.Empty;
                txtPassword.Text = string.Empty;
                linkForgotPassword.IsVisible = true;
            }
        }

        private bool IsValidUser(string username, string password)
        {
            return username == "test" && password == "test";
        }

        private async void OnForgotPasswordTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Forgot Password", "Password recovery is not implemented yet.", "OK :(");
        }
    }
}
