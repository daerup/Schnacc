using Microsoft.Extensions.Configuration;
using Schnacc.Authorization;
using System.Windows;
using Schnacc.UserInterface.HomeMenuView;
using Schnacc.UserInterface.Infrastructure.Navigation;
using System.IO;

namespace Schnacc.UserInterface
{
    public partial class App
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var appSettings = configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                                                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                  .Build();
            
            var config = appSettings.GetSection(nameof(AuthConfig)).Get<AuthConfig>();
            var navigationService = new NavigationService(new AuthorizationApi(config));
            var mainWindowViewModel = new MainWindowViewModel(navigationService, new HomeMenuPageViewModel(navigationService));
            var mainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };
            mainWindow.ShowDialog();
        }
    }
}
