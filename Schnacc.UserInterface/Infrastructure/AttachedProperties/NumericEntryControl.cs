using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Schnacc.UserInterface.Infrastructure.AttachedProperties
{
    public class TextBoxHelpers : UserControl
    {
        public static bool GetIsNumeric(DependencyObject obj) => (bool)obj.GetValue(IsNumericProperty);

        public static void SetIsNumeric(DependencyObject obj, bool value)
        {
            obj.SetValue(IsNumericProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsNumeric.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNumericProperty =
            DependencyProperty.RegisterAttached("IsNumeric", typeof(bool), typeof(TextBoxHelpers), new PropertyMetadata(false, (s, e) =>
            {
                TextBox targetTextbox = s as TextBox;
                if (targetTextbox != null)
                {
                    if ((bool)e.OldValue && !((bool)e.NewValue))
                    {
                        targetTextbox.PreviewTextInput -= TextBoxHelpers.targetTextbox_PreviewTextInput;
                    }
                    if ((bool)e.NewValue)
                    {
                        targetTextbox.PreviewTextInput += TextBoxHelpers.targetTextbox_PreviewTextInput;
                        targetTextbox.PreviewKeyDown += TextBoxHelpers.targetTextbox_PreviewKeyDown;
                    }
                }
            }));

        static void targetTextbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        static void targetTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Char newChar = e.Text[0];
            e.Handled = !Char.IsNumber(newChar);
            TextBox textBox = sender as TextBox;
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                int textBoxNumber = Convert.ToInt32(textBox.Text);
                if (textBoxNumber > 25)
                {
                    textBox.Text = "25";
                }
            }
        }
    }
}