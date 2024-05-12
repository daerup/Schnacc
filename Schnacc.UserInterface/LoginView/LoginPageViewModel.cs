using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using Schnacc.Authorization;
using Schnacc.Authorization.Exception;
using Schnacc.UserInterface.HomeMenuView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.RegisterView;

namespace Schnacc.UserInterface.LoginView
{
    internal class LoginPageViewModel : ViewModelBase, INavigableViewModel
    {
        private readonly IAuthorizationApi _authApi;
        public INavigationService NavigationService { get; set; }

        public AsyncRelayCommand<object> LoginCommand { get; }
        public RelayCommand RegisterCommand { get; }
        public RelayCommand CancelCommand { get; }

        public string Email { get; set; }

        public string ErrorMessage { get; private set; }

        public bool LoginButtonEnabled { get; private set; }

        public LoginPageViewModel(INavigationService navigationService)
        {
            this.LoginButtonEnabled = true;
            this.NavigationService = navigationService;
            this.ErrorMessage = string.Empty;
            this.LoginCommand = new AsyncRelayCommand<object>(this.Login);
            this.RegisterCommand = new RelayCommand(this.Register);
            this.CancelCommand = new RelayCommand(this.Cancel);
            this._authApi = this.NavigationService.AuthorizationApi;
        }

        private void Register()
        {
            this.NavigationService.NavigateTo(new RegisterPageViewModel(this.NavigationService));
        }

        private async Task Login(object passwordBox)
        {
            this.LoginButtonEnabled = false;
            string plainPassword = (passwordBox as PasswordBox)!.Password;
            if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(plainPassword))
            {
                this.ErrorMessage = "You have to fill both fields with normal stuff, duh";
                this.LoginButtonEnabled = true;
                return;
            }

            try
            {
                this.NavigationService.SessionToken = await this._authApi.SignInWithEmail(this.Email, plainPassword);
                this.NavigationService.NavigateTo(new LoginSuccessfulPageMenuViewModel(this.NavigationService));
            }
            catch (Exception e) when (e is IFirebaseHandledException)
            {
                this.ErrorMessage = e.Message;
                this.LoginButtonEnabled = true;
            }
        }
        private void Cancel()
        {
            this.NavigationService.NavigateTo(new HomeMenuPageViewModel(this.NavigationService));
        }
    }
}
