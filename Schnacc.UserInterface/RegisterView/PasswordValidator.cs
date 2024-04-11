using System.Windows;
using System.Windows.Controls;

namespace Schnacc.UserInterface.RegisterView {
    public class PasswordValidator : FrameworkElement
    {
        public static readonly DependencyProperty Box1Property = DependencyProperty.Register(
            nameof(PasswordValidator.Box1), typeof(PasswordBox), typeof(PasswordValidator),
            new PropertyMetadata(PasswordValidator.Box1Changed));
        public static readonly DependencyProperty Box2Property = DependencyProperty.Register(
            nameof(PasswordValidator.Box2), typeof(PasswordBox), typeof(PasswordValidator),
            new PropertyMetadata(PasswordValidator.Box2Changed));

        public PasswordBox Box1
        {
            get => (PasswordBox)this.GetValue(PasswordValidator.Box1Property);
            set => this.SetValue(PasswordValidator.Box1Property, value);
        }
        public PasswordBox Box2
        {
            get => (PasswordBox)this.GetValue(PasswordValidator.Box2Property);
            set => this.SetValue(PasswordValidator.Box2Property, value);
        }

        private static void Box1Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pv = (PasswordValidator)d;
            pv.Box1.PasswordChanged += (obj, evt) =>
            {
                pv.Box2.Tag = pv.Box1.Password != pv.Box2.Password ? "wrong" : "correct";
            };
        }
        private static void Box2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pv = (PasswordValidator)d;
            pv.Box2.PasswordChanged += (obj, evt) =>
            {
                pv.Box2.Tag = pv.Box1.Password != pv.Box2.Password ? "wrong" : "correct";
            };
        }
    }
}