using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Schnacc.UserInterface.RegisterView {
    public class PasswordValidator : FrameworkElement
    {
        static IDictionary<PasswordBox, Brush> passwordBoxes = new Dictionary<PasswordBox, Brush>();

        public static readonly DependencyProperty Box1Property = DependencyProperty.Register("Box1", typeof(PasswordBox), typeof(PasswordValidator), new PropertyMetadata(Box1Changed));
        public static readonly DependencyProperty Box2Property = DependencyProperty.Register("Box2", typeof(PasswordBox), typeof(PasswordValidator), new PropertyMetadata(Box2Changed));

        public PasswordBox Box1
        {
            get { return (PasswordBox)this.GetValue(Box1Property); }
            set { this.SetValue(Box1Property, value); }
        }
        public PasswordBox Box2
        {
            get { return (PasswordBox)this.GetValue(Box2Property); }
            set { this.SetValue(Box2Property, value); }
        }

        private static void Box1Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pv = (PasswordValidator)d;
            pv.Box1.PasswordChanged += (obj, evt) =>
            {
                if (pv.Box1.Password != pv.Box2.Password)
                {
                    pv.Box2.Tag = "wrong";
                }
                else
                {
                    pv.Box2.Tag = "correct";
                }
            };
        }
        private static void Box2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pv = (PasswordValidator)d;
            pv.Box2.PasswordChanged += (obj, evt) =>
            {
                if (pv.Box1.Password != pv.Box2.Password)
                {
                    pv.Box2.Tag = "wrong";
                }
                else
                {
                    pv.Box2.Tag = "correct";
                }
            };
        }
    }
}