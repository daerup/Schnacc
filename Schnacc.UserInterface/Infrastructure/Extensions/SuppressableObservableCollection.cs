using System.Collections.Generic;
using System.Collections.Specialized;
using Collection.Utils;

namespace Schnacc.UserInterface.Infrastructure.Extensions
{
    public class SuppressableObservableCollection<T> : FastObservableCollection<T>
    {
        public SuppressableObservableCollection(IEnumerable<T> collection) : base(collection)
        {
        }
        
        private bool notificationHasBeenSuppressed;
        private bool suppressNotification;
        public bool SuppressNotification
        {
            get => this.suppressNotification;
            set
            {
                this.suppressNotification = value;
                if (this.suppressNotification || !this.notificationHasBeenSuppressed) return;
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                this.notificationHasBeenSuppressed = false;
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.SuppressNotification)
            {
                this.notificationHasBeenSuppressed = true;
                return;
            }
            base.OnCollectionChanged(e);
        }
    }
}