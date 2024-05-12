﻿using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;

namespace Schnacc.UserInterface
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;

        public MainWindowViewModel(NavigationService navigationService, IViewModel homeMenuPageViewModel)
        {
            this.CurrentPageViewModel = homeMenuPageViewModel;
            this._navigationService = navigationService;
            this._navigationService.OnNavigation += this.Navigate;
        }
        public IViewModel CurrentPageViewModel { get; private set; }
        
        public string WindowTitle => "Schnacc";
        
        private void Navigate(object sender, NavigationEventArgs navigationArguments)
        {
            this.CurrentPageViewModel = navigationArguments.ViewModelToNavigateTo;
        }
    }
}