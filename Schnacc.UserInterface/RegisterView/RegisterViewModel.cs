﻿using System;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Media;
using Schnacc.Authorization;
using Schnacc.Authorization.Exception;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.LoginView;

namespace Schnacc.UserInterface.RegisterView
{
    public class RegisterViewModel : ViewModelBase, INavigatableViewModel
    {
        private AuthorizationApi authApi;
        private string errorCheck;
        public INavigationService navigationService { get; set; }
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

        public bool passwordMatch => !this.errorCheck.Equals("wrong") && !string.IsNullOrEmpty(this.Email) && string.IsNullOrEmpty(this.ErrorMessage);

        public RegisterViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.ErrorMessage = string.Empty;
            this.ErrorCheck = "wrong";
            this.LoginCommand = new RelayCommand<object>(this.Login);
            this.RegisterCommand = new RelayCommand<object>(this.Register);
            this.authApi = new AuthorizationApi();
        }

        private void Register(object obj)
        {
            string plainPassword = (obj as PasswordBox).Password;
            this.authApi.RegisterWithEmail(this.Email, plainPassword, this.Username);
            this.ErrorMessage = "Please confirm our email";
        }

        private async void Login(object obj)
        {
            this.navigationService.NavigateTo(new LoginViewModel(this.navigationService));
        }
    }
}