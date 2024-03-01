using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Schnacc.UserInterface.Infrastructure.Commands
{
    public class EventToCommandBehavior : Behavior<FrameworkElement>
    {
        private Delegate handler;
        private EventInfo oldEvent;

        // Event
        public string Event { get => (string)this.GetValue(EventToCommandBehavior.EventProperty);
            set => this.SetValue(EventToCommandBehavior.EventProperty, value);
        }
        public static readonly DependencyProperty EventProperty = DependencyProperty.Register("Event", typeof(string), typeof(EventToCommandBehavior), new PropertyMetadata(null, EventToCommandBehavior.OnEventChanged));

        // Command
        public ICommand Command { get => (ICommand)this.GetValue(EventToCommandBehavior.CommandProperty);
            set => this.SetValue(EventToCommandBehavior.CommandProperty, value);
        }
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommandBehavior), new PropertyMetadata(null));

        // PassArguments (default: false)
        public bool PassArguments { get => (bool)this.GetValue(EventToCommandBehavior.PassArgumentsProperty);
            set => this.SetValue(EventToCommandBehavior.PassArgumentsProperty, value);
        }
        public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register("PassArguments", typeof(bool), typeof(EventToCommandBehavior), new PropertyMetadata(false));


        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var beh = (EventToCommandBehavior)d;

            if (beh.AssociatedObject != null) // is not yet attached at initial load
                beh.AttachHandler((string)e.NewValue);
        }

        protected override void OnAttached()
        {
            this.AttachHandler(this.Event); // initial set
        }

        /// <summary>
        /// Attaches the handler to the event
        /// </summary>
        private void AttachHandler(string eventName)
        {
            // detach old event
            if (this.oldEvent != null)
                this.oldEvent.RemoveEventHandler(this.AssociatedObject, this.handler);

            // attach new event
            if (!string.IsNullOrEmpty(eventName))
            {
                EventInfo ei = this.AssociatedObject.GetType().GetEvent(eventName);
                if (ei != null)
                {
                    MethodInfo mi = this.GetType().GetMethod("ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic);
                    this.handler = Delegate.CreateDelegate(ei.EventHandlerType!, this, mi!);
                    ei.AddEventHandler(this.AssociatedObject, this.handler);
                    this.oldEvent = ei; // store to detach in case the Event property changes
                }
                else
                    throw new ArgumentException(
                        $"The event '{eventName}' was not found on type '{this.AssociatedObject.GetType().Name}'");
            }
        }
    }
}
