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
                _SelectedIndex = value;
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            }
            get => _SelectedIndex;
        }

        [MaybeNull]
        public T SelectedItem => SelectedIndex < 0 ? default : this[SelectedIndex];

        public event EventHandler? SelectedIndexChanged;

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
            base.SetItem(index, item);
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
