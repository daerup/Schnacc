using Microsoft.Extensions.Configuration;
using Schnacc.Authorization;
using System.Windows;
using Schnacc.UserInterface.HomeMenuView;
using Schnacc.UserInterface.Infrastructure.Navigation;
using System.IO;
using System.Threading.Tasks;

namespace Schnacc.UserInterface
{
    public partial class App
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var appSettings = configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                                                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                  .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
                                                  .Build();
            
            var config = appSettings.GetSection(nameof(AuthConfig)).Get<AuthConfig>();
            var api = new AuthorizationApi(config);
            var navigationService = new NavigationService(api);
            
            Application.Current?.Dispatcher?.Invoke(async ()=>
            {
                navigationService.SessionToken = await api.SignInAnonymous();
                var mainWindowViewModel = new MainWindowViewModel(navigationService, new HomeMenuPageViewModel(navigationService));
                var mainWindow = new MainWindow
                {
                    DataContext = mainWindowViewModel
                };
                mainWindow.ShowDialog();
            });

            
        }
    }
}
