﻿using System;
using System.Windows.Controls;
using Schnacc.Authorization;
using Schnacc.Authorization.Exception;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.LoginView;

namespace Schnacc.UserInterface.RegisterView
{
    public class RegisterPageViewModel : ViewModelBase, INavigatableViewModel
    {
        private AuthorizationApi authApi;
        private string errorCheck;
        public INavigationService NavigationService { get; set; }
        public RelayCommand<object> LoginCommand { get; }
        public RelayCommand<object> RegisterCommand { get; }

        public string Username { get; set; }
        public string Email { get; set; }

        public string ErrorMessage { get; private set; }

        public string ErrorCheck
        {
            set
            {
                if (value == "wrong")
                {
                    this.ErrorMessage = "The passwords do not match";
                }
                else
                {
                    this.ErrorMessage = string.Empty;
                }
                this.errorCheck = value;
            }
        }

        public bool RegisterButtonEnabled => !this.errorCheck.Equals("wrong") && !string.IsNullOrEmpty(this.Email) && !string.IsNullOrEmpty(this.Username);

        public string LoginContent { get; set; }

        public int LoginContentFontSize { get;  private set; }

        public RegisterPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.ErrorMessage = string.Empty;
            this.ErrorCheck = "wrong";
            this.LoginCommand = new RelayCommand<object>(this.Login);
            this.RegisterCommand = new RelayCommand<object>(this.Register);
            this.authApi = new AuthorizationApi();
            this.LoginContent = "I already have an account";
            this.LoginContentFontSize = 12;
        }

        private async void Register(object obj)
        {
            try
            {
                string plainPassword = (obj as PasswordBox).Password;
                await this.authApi.RegisterWithEmail(this.Email, plainPassword, this.Username);
                this.ErrorMessage = "Please confirm our email";
                this.LoginContent = "Login";
                this.LoginContentFontSize = 20;
            }
            catch (Exception e) when (e is IFirebaseHandledException)
            {
                this.ErrorMessage = e.Message;
            }
        }

        private void Login(object obj)
        {
            this.NavigationService.NavigateTo(new LoginPageViewModel(this.NavigationService));
        }
    }
}