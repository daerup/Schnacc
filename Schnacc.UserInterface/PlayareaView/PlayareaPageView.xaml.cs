using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Schnacc.UserInterface.PlayareaView
{
    /// <summary>
    /// Interaction logic for PlayareaView.xaml
    /// </summary>
    public partial class PlayareaPageView : UserControl
    {
        public PlayareaPageView()
        {
            InitializeComponent();
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            PlayareaViewModel playareaPageViewModel = (PlayareaViewModel)this.DataContext;
            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyDownEvent, new KeyEventHandler(playareaPageViewModel.UpdateSnakeDirectionTo), true);
        }
    }
}
