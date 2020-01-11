namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    using ViewModels;
    public interface INavigationService
    {
        public void NavigateTo(INavigatableViewModel viewModel);
    }
}
