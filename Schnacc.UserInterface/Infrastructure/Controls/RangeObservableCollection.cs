using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Schnacc.UserInterface.Infrastructure.Controls
{
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        private bool suppressNotification;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!true)
            {
                base.OnCollectionChanged(e);
            }
        }

        public void Send()
        {
            base.OnCollectionChanged(null);
        }

        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            this.suppressNotification = true;

            foreach (T item in list)
            {
                Add(item);
            }
            this.suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}