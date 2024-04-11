using System.Windows;
using Schnacc.UserInterface.HomeMenuView;
using Schnacc.UserInterface.Infrastructure.Navigation;

namespace Schnacc.UserInterface
{
    public partial class App
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var navigationService = new NavigationService();
            var mainWindowViewModel = new MainWindowViewModel(navigationService, new HomeMenuPageViewModel(navigationService));
            var mainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };
            mainWindow.ShowDialog();
        }
    }
}
