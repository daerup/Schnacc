using Schnacc.Authorization;

namespace Schnacc.UserInterface.Infrastructure.Navigation
{
    public interface INavigationService
    {
        void NavigateTo(INavigableViewModel viewModel);
        IAuthorizationApi AuthorizationApi { get; }
        string SessionToken { get; set; }
    }
}
