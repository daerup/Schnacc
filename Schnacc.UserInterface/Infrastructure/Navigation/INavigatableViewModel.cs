namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    using ViewModels;

    public interface INavigatableViewModel : IViewModel
    {
        INavigationService NavigationService { get; set; }
    }
}
