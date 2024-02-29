using Schnacc.UserInterface.Infrastructure.ViewModels;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    public interface INavigatableViewModel : IViewModel
    {
        INavigationService NavigationService { get; set; }
    }
}
