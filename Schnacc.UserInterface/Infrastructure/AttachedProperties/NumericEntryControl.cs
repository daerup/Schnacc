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
                if (!(s is TextBox targetTextBox))
                {
                    return;
                }

                if ((bool)e.OldValue && !(bool)e.NewValue)
                {
                    targetTextBox.PreviewTextInput -= TextBoxHelpers.TargetTextBoxPreviewTextInput;
                }

                if (!(bool)e.NewValue)
                {
                    return;
                }

                targetTextBox.PreviewTextInput += TextBoxHelpers.TargetTextBoxPreviewTextInput;
                targetTextBox.PreviewKeyDown += TextBoxHelpers.TargetTextBoxPreviewKeyDown;
            }));

        private static void TargetTextBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private static void TargetTextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char newChar = e.Text[0];
            e.Handled = !char.IsNumber(newChar);
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox!.Text))
            {
                return;
            }

            int textBoxNumber = Convert.ToInt32(textBox.Text);
            if (textBoxNumber > 25)
            {
                textBox.Text = "25";
            }
        }
    }
}