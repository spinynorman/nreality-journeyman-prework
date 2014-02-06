using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDataStructures
{
    public class GQueue<T>
    {
        private T[] m_items;
        private int m_queueCapacity;
        private int m_itemCount;
        private int m_currentIndex;

        public GQueue() : this(100)
        {
        }

        public GQueue(int initialSize)
        {
            m_itemCount = 0;
            m_currentIndex = 0;
            m_queueCapacity = initialSize;
            m_items = new T[initialSize];
        }

        public void Enqueue(T item)
        {
            if (m_itemCount >= m_queueCapacity)
                ResizeArray();
            var insertIndex = (m_currentIndex + m_itemCount) % m_queueCapacity;
            m_items[insertIndex] = item;
            m_itemCount++;
        }

        private void ResizeArray()
        {
            m_queueCapacity += 50;
            var newItems = new T[m_queueCapacity];
            for (var i = 0; i < m_itemCount; i++)
            {
                newItems[i] = m_items[m_currentIndex];
                m_currentIndex++;
                if (m_currentIndex >= m_items.Count())
                    m_currentIndex = 0;
            }
            m_items = newItems;
            m_currentIndex = 0;
        }

        public T Dequeue()
        {
            var retVal = m_items[m_currentIndex];
            m_itemCount--;
            m_currentIndex++;
            if (m_currentIndex >= m_queueCapacity)
                m_currentIndex = 0;
            return retVal;
        }

        public int Capacity()
        {
            return m_queueCapacity;
        }

        public int Count()
        {
            return m_itemCount;
        }
    }
}
