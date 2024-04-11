using Schnacc.UserInterface.Infrastructure.ViewModels;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    public interface INavigableViewModel : IViewModel
    {
        public INavigationService NavigationService { get; set; }
    }
}
