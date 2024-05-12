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
        
        private bool _notificationHasBeenSuppressed;
        private bool _suppressNotification;
        public bool SuppressNotification
        {
            get => this._suppressNotification;
            set
            {
                this._suppressNotification = value;
                if (this._suppressNotification || !this._notificationHasBeenSuppressed)
                {
                    return;
                }

                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                this._notificationHasBeenSuppressed = false;
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.SuppressNotification)
            {
                this._notificationHasBeenSuppressed = true;
                return;
            }
            base.OnCollectionChanged(e);
        }
    }
}