using System.Windows;
using System.Windows.Controls;
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
            for (int i = 0; i < playareaViewModel.NumberOfColumns; i++)
            {
                this.Grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < playareaViewModel.NumberOfRows; i++)
            {
                this.Grid.RowDefinitions.Add(new RowDefinition());
            }

            this.Grid.Focus();

            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyDownEvent, new KeyEventHandler(playareaViewModel.UpdateSnakeDirectionTo), true);
        }
    }
}
