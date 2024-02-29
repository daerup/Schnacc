using System;
using Schnacc.UserInterface.Infrastructure.ViewModels;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    public class NavigationEventArgs : EventArgs
    {
        public IViewModel ViewModelToNavigateTo { get; private set; }

        public NavigationEventArgs(IViewModel viewModelToNavigateTo) => this.ViewModelToNavigateTo = viewModelToNavigateTo;
    }
}
