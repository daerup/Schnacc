using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.Authorization;
using Schnacc.Authorization.Exception;
using Schnacc.UserInterface.RegisterView;

namespace Schnacc.UserInterface.LoginView
{
    class LoginPageViewModel : ViewModelBase, INavigatableViewModel
    {
        private AuthorizationApi authApi;
        public INavigationService navigationService { get; set; }
        public RelayCommand<object> LoginCommand { get; }
        public RelayCommand<object> RegisterCommand { get; }

        public string Email { get; set; }

        public string ErrorMessage { get; private set; }

        public LoginPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.ErrorMessage = string.Empty;
            this.LoginCommand = new RelayCommand<object>(this.Login);
            this.RegisterCommand = new RelayCommand<object>(this.Register);
            this.authApi = new AuthorizationApi();
        }

        private void Register(object obj)
        {
            this.navigationService.NavigateTo(new RegisterPageViewModel(this.navigationService));
        }

        private async void Login(object obj)
        {
            string plainPassword = (obj as PasswordBox).Password;
            if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(plainPassword))
            {
                this.ErrorMessage = "You have to fill both fields with normal stuff, duh";
                return;
            }

            try
            {
                this.navigationService.SessionToken = await this.authApi.SignInWithEmail(this.Email, plainPassword);
                this.navigationService.EmailIsVerified = this.authApi.userHasVerifiedEmail();
                this.navigationService.NavigateTo(new LoginSuccessfulPageViewModel(this.navigationService));
            }
            catch (Exception e) when (e is IFirebaseHandledException)
            {
                this.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
