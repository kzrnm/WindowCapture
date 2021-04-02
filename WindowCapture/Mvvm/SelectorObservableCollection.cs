using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Kzrnm.WindowCapture.Mvvm
{
    public class SelectorObservableCollection<T> : ObservableCollection<T>
    {
        private int _SelectedIndex = -1;
        public int SelectedIndex
        {
            set
            {
                var e = new SelectedIndexChangedEventArgs(
                    _SelectedIndex, value,
                    (uint)_SelectedIndex < (uint)this.Count ? this[_SelectedIndex] : default,
                    (uint)value < (uint)this.Count ? this[value] : default);
                _SelectedIndex = value;
                SelectedIndexChanged?.Invoke(this, e);
            }
            get => _SelectedIndex;
        }

        [MaybeNull]
        public T SelectedItem => SelectedIndex < 0 ? default : this[SelectedIndex];

        public event EventHandler<SelectedIndexChangedEventArgs>? SelectedIndexChanged;

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            SelectedIndex = index;
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            SelectedIndex = -1;
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
            if (oldIndex == SelectedIndex)
            {
                SelectedIndex = newIndex;
            }
        }
        protected override void RemoveItem(int index)
        {
            var prevSelected = SelectedIndex;
            base.RemoveItem(index);
            if (this.Count == 0)
            {
                SelectedIndex = -1;
            }
            else if (index == prevSelected)
            {
                SelectedIndex = Math.Min(prevSelected, this.Count - 1);
            }
            else if (index < prevSelected)
            {
                SelectedIndex = prevSelected - 1;
            }
        }
        protected override void SetItem(int index, T item)
        {
            SelectedIndexChangedEventArgs? e = null;
            if (index == _SelectedIndex)
            {
                var oldValue = (uint)_SelectedIndex < (uint)this.Count ? this[_SelectedIndex] : default;
                e = new SelectedIndexChangedEventArgs(_SelectedIndex, _SelectedIndex, oldValue, item);
            }
            base.SetItem(index, item);
            if (e is not null)
                SelectedIndexChanged?.Invoke(this, e);
        }

        public void RemoveSelectedItem()
        {
            var ix = SelectedIndex;
            if ((uint)ix < (uint)Count)
                this.RemoveAt(ix);
        }

        public class SelectedIndexChangedEventArgs : EventArgs
        {
            public int OldIndex { get; }
            public int NewIndex { get; }
            public T? OldItem { get; }
            public T? NewItem { get; }
            public SelectedIndexChangedEventArgs(int oldIndex, int newIndex, T? oldItem, T? newItem)
            {
                this.OldIndex = oldIndex;
                this.NewIndex = newIndex;
                this.OldItem = oldItem;
                this.NewItem = newItem;
            }
        }
    }
}
