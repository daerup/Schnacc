using System;
using System.Collections.Generic;
using System.Text;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;

namespace Schnacc.UserInterface.HighscoreView
{
    class HighscoreViewModel : ViewModelBase, INavigatableViewModel
    {
        public HighscoreViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            if (this.navigationService.SessionToken is null)
            {
                
            }
        }

        public INavigationService navigationService { get; set; }
    }
}
