using System.Windows;
using System.Windows.Input;

namespace Schnacc.UserInterface.PlayAreaView
{
    /// <summary>
    /// Interaction logic for PlayareaView.xaml
    /// </summary>
    public partial class PlayAreaPageView
    {
        public PlayAreaPageView()
        {
            this.InitializeComponent();
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            var playAreaPageViewModel = (PlayAreaViewModel)this.DataContext;
            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyDownEvent, new KeyEventHandler(playAreaPageViewModel.UpdateSnakeDirectionTo), true);
        }
    }
}
