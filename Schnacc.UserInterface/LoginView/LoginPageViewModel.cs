using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using Schnacc.Authorization;
using Schnacc.Authorization.Exception;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.RegisterView;

namespace Schnacc.UserInterface.LoginView
{
    internal class LoginPageViewModel : ViewModelBase, INavigatableViewModel
    {
        private readonly AuthorizationApi authApi;
        public INavigationService NavigationService { get; set; }

        public RelayCommand<object> LoginCommand { get; }
        public RelayCommand<object> RegisterCommand { get; }

        public string Email { get; set; }

        public string ErrorMessage { get; private set; }

        public bool LoginButtonEnabled { get; private set; }

        public LoginPageViewModel(INavigationService navigationService)
        {
            this.LoginButtonEnabled = true;
            this.NavigationService = navigationService;
            this.ErrorMessage = string.Empty;
            this.LoginCommand = new RelayCommand<object>(async (o) => await this.Login(o));
            this.RegisterCommand = new RelayCommand<object>(this.Register);
            this.authApi = new AuthorizationApi();
            this.NavigationService.SetAuthApi(this.authApi);
        }

        private void Register(object obj)
        {
            this.NavigationService.NavigateTo(new RegisterPageViewModel(this.NavigationService));
        }

        private async Task Login(object obj)
        {
            this.LoginButtonEnabled = false;
            string plainPassword = (obj as PasswordBox)!.Password;
            if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(plainPassword))
            {
                this.ErrorMessage = "You have to fill both fields with normal stuff, duh";
                this.LoginButtonEnabled = true;
                return;
            }

            try
            {
                this.NavigationService.SessionToken = await this.authApi.SignInWithEmail(this.Email, plainPassword);
                this.NavigationService.NavigateTo(new LoginSuccessfulPageMenuViewModel(this.NavigationService));
            }
            catch (Exception e) when (e is IFirebaseHandledException)
            {
                this.ErrorMessage = e.Message;
                this.LoginButtonEnabled = true;
            }
        }
    }
}
