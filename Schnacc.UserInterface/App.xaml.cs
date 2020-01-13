using System.Windows;
using Schnacc.UserInterface.HomeMenuView;
using Schnacc.UserInterface.Infrastructure.Navigation;

namespace Schnacc.UserInterface
{
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            NavigationService navigationService = new NavigationService();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(navigationService, new HomeMenuPageViewModel(navigationService));
            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = mainWindowViewModel;
            mainWindow.ShowDialog();
        }
    }
}
