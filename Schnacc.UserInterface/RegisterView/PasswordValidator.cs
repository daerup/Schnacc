using System.Windows;
using System.Windows.Controls;

namespace Schnacc.UserInterface.RegisterView {
    public class PasswordValidator : FrameworkElement
    {
        public static readonly DependencyProperty EnterPasswordProperty = DependencyProperty.Register(
            nameof(PasswordValidator.EnterPassword), typeof(PasswordBox), typeof(PasswordValidator),
            new PropertyMetadata(PasswordValidator.Box1Changed));
        public static readonly DependencyProperty RepeatPasswordProperty = DependencyProperty.Register(
            nameof(PasswordValidator.RepeatPassword), typeof(PasswordBox), typeof(PasswordValidator),
            new PropertyMetadata(PasswordValidator.Box2Changed));

        public PasswordBox EnterPassword
        {
            get => (PasswordBox)this.GetValue(PasswordValidator.EnterPasswordProperty);
            set => this.SetValue(PasswordValidator.EnterPasswordProperty, value);
        }
        public PasswordBox RepeatPassword
        {
            get => (PasswordBox)this.GetValue(PasswordValidator.RepeatPasswordProperty);
            set => this.SetValue(PasswordValidator.RepeatPasswordProperty, value);
        }

        private static void Box1Changed(DependencyObject d, DependencyPropertyChangedEventArgs _)
        {
            var pv = (PasswordValidator)d;
            pv.EnterPassword.PasswordChanged += (obj, evt) =>
            {
                pv.RepeatPassword.Tag = pv.EnterPassword.Password == pv.RepeatPassword.Password;
            };
        }
        private static void Box2Changed(DependencyObject d, DependencyPropertyChangedEventArgs _)
        {
            var pv = (PasswordValidator)d;
            pv.RepeatPassword.PasswordChanged += (obj, evt) =>
            {
                pv.RepeatPassword.Tag = pv.EnterPassword.Password == pv.RepeatPassword.Password;
            };
        }
    }
}