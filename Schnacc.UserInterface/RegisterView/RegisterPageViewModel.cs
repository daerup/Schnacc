using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using Schnacc.Authorization;
using Schnacc.Authorization.Exception;
using Schnacc.UserInterface.HomeMenuView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.LoginView;

namespace Schnacc.UserInterface.RegisterView
{
    public class RegisterPageViewModel : ViewModelBase, INavigableViewModel
    {
        private readonly IAuthorizationApi _authApi;
        private bool? _passwordMatch;
        public INavigationService NavigationService { get; set; }
        public RelayCommand LoginCommand { get; }
        public AsyncRelayCommand<object> RegisterCommand { get; }
        public RelayCommand CancelCommand { get; }

        public string Username { get; set; }
        public string Email { get; set; }

        public string ErrorMessage { get; private set; }

        public bool PasswordMatch
        {
            get => this._passwordMatch ?? false;
            set  
            {
                this.ErrorMessage = value ? string.Empty : "The passwords do not match";
                this._passwordMatch = value;
            }
        }

        public bool RegisterButtonEnabled => this.PasswordMatch && !string.IsNullOrEmpty(this.Email) && !string.IsNullOrEmpty(this.Username);

        public string LoginContent { get; private set; }

        public int LoginContentFontSize { get;  private set; }

        public RegisterPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.ErrorMessage = string.Empty;
            this.PasswordMatch = false;
            this.LoginCommand = new RelayCommand(this.Login);
            this.RegisterCommand = new AsyncRelayCommand<object>(this.Register);
            this.CancelCommand = new RelayCommand(this.Cancel);
            this._authApi = this.NavigationService.AuthorizationApi;
            this.LoginContent = "I already have an account";
            this.LoginContentFontSize = 12;
        }

        private async Task Register(object passwordBox)
        {
            try
            {
                string plainPassword = (passwordBox as PasswordBox)?.Password;
                await this._authApi.RegisterWithEmail(this.Email, plainPassword, this.Username);
                this.ErrorMessage = "Please confirm our email";
                this.LoginContent = "Login";
                this.LoginContentFontSize = 20;
            }
            catch (Exception e) when (e is IFirebaseHandledException)
            {
                this.ErrorMessage = e.Message;
            }
        }

        private void Login()
        {
            this.NavigationService.NavigateTo(new LoginPageViewModel(this.NavigationService));
        }

        private void Cancel()
        {
            this.NavigationService.NavigateTo(new HomeMenuPageViewModel(this.NavigationService));
        }
    }
}