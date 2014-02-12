using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDataStructures
{
    public class GList<T>
    {
        private T[] _items;
        private int _listCapacity;
        private int _itemCount;
        private bool _isSorted;
        private IComparer<T> comparer;

        public GList(IComparer<T> listComparer)
            : this(100, listComparer)
        {
        }

        public GList(int initialCapacity, IComparer<T> listComparer)
        {
            _listCapacity = initialCapacity;
            _items = new T[_listCapacity];
            _itemCount = 0;
            _isSorted = true;
            this.comparer = listComparer;
        }

        public void Add(T item)
        {
            if (_itemCount == _listCapacity)
                ResizeArray();
            _items[_itemCount] = item;
            _itemCount++;
            _isSorted = _itemCount <= 1;
        }

        public T ItemAt(int index)
        {
            return _items[index];
        }

        public bool IsSorted()
        {
            return _isSorted;
        }

        public void Sort()
        {
            if (_isSorted)
                return;
            Array.Sort(_items,0,_itemCount, comparer);
            _isSorted = true;
        }

        private void ResizeArray()
        {
            _listCapacity += 50;
            var newItems = new T[_listCapacity];
            for (var i = 0; i < _itemCount; i++)
            {
                newItems[i] = _items[i];
            }
            _items = newItems;
        }

        private int BinarySearch(T searchItem)
        {
            if (!_isSorted)
                Sort();
            var low = 0;
            var high = _itemCount - 1;
            while (low <= high)
            {
                var mid = (high - low) / 2 + low;    
                if (comparer.Compare(_items[mid], searchItem) == 0)
                    return mid;
                if (comparer.Compare(_items[mid], searchItem) > 0)
                    high = mid - 1;
                else
                    low = mid + 1;
            }
            return -1;
        }

        public int IndexOf(T searchItem)
        {
            return BinarySearch(searchItem);
        }

        public bool Contains(T searchItem)
        {
            return (BinarySearch(searchItem) != -1);
        }

        public int Capacity()
        {
            return _listCapacity;
        }

        public int Count()
        {
            return _itemCount;
        }
    }
}
