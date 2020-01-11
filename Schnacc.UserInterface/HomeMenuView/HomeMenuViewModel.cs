using System;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.PlayareaView;

namespace Schnacc.UserInterface.HomeMenuView
{
    public class HomeMenuViewModel : ViewModelBase, INavigatableViewModel
    {
        public RelayCommand GoToPlayareaView { get; }

        public HomeMenuViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.GoToPlayareaView = new RelayCommand(this.NavigateToPlayarea);
        }

        public INavigationService navigationService { get; set; }

        private void NavigateToPlayarea()
        {
            this.navigationService.NavigateTo(new PlayareaViewModel(this.navigationService));
        }
    };
}