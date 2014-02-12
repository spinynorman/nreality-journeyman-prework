using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDataStructures
{
    public class GQueue<T>
    {
        private T[] _items;
        private int _queueCapacity;
        private int _itemCount;
        private int _currentIndex;

        public GQueue() : this(100)
        {
        }

        public GQueue(int initialSize)
        {
            _itemCount = 0;
            _currentIndex = 0;
            _queueCapacity = initialSize;
            _items = new T[initialSize];
        }

        public void Enqueue(T item)
        {
            if (_itemCount >= _queueCapacity)
                ResizeArray();
            var insertIndex = (_currentIndex + _itemCount) % _queueCapacity;
            _items[insertIndex] = item;
            _itemCount++;
        }

        private void ResizeArray()
        {
            _queueCapacity += 50;
            var newItems = new T[_queueCapacity];
            for (var i = 0; i < _itemCount; i++)
            {
                newItems[i] = _items[_currentIndex];
                _currentIndex++;
                if (_currentIndex >= _items.Count())
                    _currentIndex = 0;
            }
            _items = newItems;
            _currentIndex = 0;
        }

        public T Dequeue()
        {
            var retVal = _items[_currentIndex];
            _itemCount--;
            _currentIndex++;
            if (_currentIndex >= _queueCapacity)
                _currentIndex = 0;
            return retVal;
        }

        public int Capacity()
        {
            return _queueCapacity;
        }

        public int Count()
        {
            return _itemCount;
        }
    }
}
