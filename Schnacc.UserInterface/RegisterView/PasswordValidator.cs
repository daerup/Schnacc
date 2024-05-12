using System.Windows;
using System.Windows.Controls;

namespace Schnacc.UserInterface.RegisterView {
    public class PasswordValidator : FrameworkElement
    {
        public static readonly DependencyProperty EnterPasswordProperty = DependencyProperty.Register(
            nameof(PasswordValidator.EnterPassword), typeof(PasswordBox), typeof(PasswordValidator),
            new PropertyMetadata((d, _) => PasswordValidator.EnterPasswordChanged(d)));
        public static readonly DependencyProperty RepeatPasswordProperty = DependencyProperty.Register(
            nameof(PasswordValidator.RepeatPassword), typeof(PasswordBox), typeof(PasswordValidator),
            new PropertyMetadata((d, _) => PasswordValidator.RepeatPasswordChanged(d)));

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

        private static void EnterPasswordChanged(DependencyObject d)
        {
            var pv = (PasswordValidator)d;
            pv.EnterPassword.PasswordChanged += (obj, evt) =>
            {
                pv.RepeatPassword.Tag = pv.EnterPassword.Password == pv.RepeatPassword.Password;
            };
        }
        private static void RepeatPasswordChanged(DependencyObject d)
        {
            var pv = (PasswordValidator)d;
            pv.RepeatPassword.PasswordChanged += (obj, evt) =>
            {
                pv.RepeatPassword.Tag = pv.EnterPassword.Password == pv.RepeatPassword.Password;
            };
        }
    }
}