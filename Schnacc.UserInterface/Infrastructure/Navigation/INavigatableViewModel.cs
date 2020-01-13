using System;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    using ViewModels;

    public interface INavigatableViewModel : IViewModel
    {
        public INavigationService navigationService { get; set; }
    }
}
