using Schnacc.UserInterface.Infrastructure.Navigation;

namespace Schnacc.UserInterface
{
    using HomeMenuView;
    using Infrastructure.ViewModels;
    
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NavigationService navigationService;

        public MainWindowViewModel(NavigationService navigationService, HomeMenuPageViewModel homeMenuPageViewModel)
        {
            this.CurrentPageViewModel = homeMenuPageViewModel;
            this.navigationService = navigationService;
            this.navigationService.OnNavigation += this.Navigate;
        }

        public IViewModel CurrentPageViewModel { get; private set; }

        public string WindowTitle => "Schnacc";


        private void Navigate(object sender, NavigationEventArgs navigationArguments)
        {
            this.CurrentPageViewModel = navigationArguments.ViewModelToNavigateTo;
        }
    }
}