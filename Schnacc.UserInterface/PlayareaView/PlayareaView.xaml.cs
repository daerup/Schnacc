using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Schnacc.UserInterface.PlayareaView
{
    /// <summary>
    /// Interaction logic for PlayareaView.xaml
    /// </summary>
    public partial class PlayareaView : UserControl
    {
        public PlayareaView()
        {
            InitializeComponent();
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
            PlayareaViewModel playareaViewModel = (PlayareaViewModel)this.DataContext;


            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyDownEvent, new KeyEventHandler(playareaViewModel.UpdateSnakeDirectionTo), true);
        }
    }
}
