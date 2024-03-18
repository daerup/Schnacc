using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Schnacc.UserInterface.Infrastructure.AttachedProperties
{
    public class TextBoxHelpers : UserControl
    {
        public static bool GetIsNumeric(DependencyObject obj) => (bool)obj.GetValue(TextBoxHelpers.IsNumericProperty);

        public static void SetIsNumeric(DependencyObject obj, bool value)
        {
            obj.SetValue(TextBoxHelpers.IsNumericProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsNumeric.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNumericProperty =
            DependencyProperty.RegisterAttached("IsNumeric", typeof(bool), typeof(TextBoxHelpers), new PropertyMetadata(false, (s, e) =>
            {
                var targetTextbox = s as TextBox;
                if (targetTextbox != null)
                {
                    if ((bool)e.OldValue && !(bool)e.NewValue)
                    {
                        targetTextbox.PreviewTextInput -= TextBoxHelpers.targetTextboxPreviewTextInput;
                    }
                    if ((bool)e.NewValue)
                    {
                        targetTextbox.PreviewTextInput += TextBoxHelpers.targetTextboxPreviewTextInput;
                        targetTextbox.PreviewKeyDown += TextBoxHelpers.targetTextboxPreviewKeyDown;
                    }
                }
            }));

        private static void targetTextboxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private static void targetTextboxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char newChar = e.Text[0];
            e.Handled = !char.IsNumber(newChar);
            var textBox = sender as TextBox;
            if (!string.IsNullOrEmpty(textBox!.Text))
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