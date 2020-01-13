namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    using ViewModels;
    using System;

    public class NavigationEventArgs : EventArgs
    {
        public IViewModel ViewModelToNavigateTo { get; private set; }

        public NavigationEventArgs(IViewModel viewModelToNavigateTo)
        {
            this.ViewModelToNavigateTo = viewModelToNavigateTo;
        }
    }
}
