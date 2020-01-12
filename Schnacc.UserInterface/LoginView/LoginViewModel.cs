using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.Authorization;

namespace Schnacc.UserInterface.LoginView
{
    class LoginViewModel : ViewModelBase, INavigatableViewModel
    {
        private AuthorizationApi authApi;
        public INavigationService navigationService { get; set; }
        public RelayCommand<object> LoginCommand { get; }
        public RelayCommand<object> RegisterCommand { get; }

        public string Email { get; set; }

        public LoginViewModel()
        {
            this.LoginCommand = new RelayCommand<object>(this.Login);
            this.RegisterCommand = new RelayCommand<object>(this.Register);
            this.authApi = new AuthorizationApi();
        }

        private void Register(object obj)
        {
            throw new NotImplementedException();
        }

        private void Login(object obj)
        {
            PasswordBox pwBox = obj as PasswordBox;
            this.navigationService.SessionToken = this.authApi.SignInWithEmail(this.Email, pwBox.Password);
        }
    }
}
