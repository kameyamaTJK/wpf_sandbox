using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSandbox
{
    public class RingBuffer<T> : IEnumerable<RingBuffer<T>.IBufferItem> where T : IDisposable
    {
        public interface IBufferItemFactory
        {
            T Create();
        }

        public interface IBufferItem
        {
            T Value { get; set; }

            void Lock();
            void Unlock();
        }

        private class BufferItem : IBufferItem, IDisposable
        {
            public bool IsUsing { get; set; }
            public T Value { get; set; }

            public void Dispose()
            {
                Unlock();
            }

            public void Lock()
            {
                IsUsing = true;
            }

            public void Unlock()
            {
                Value.Dispose();
                IsUsing = false;
            }
        }

        public IBufferItemFactory ItemFactory { get; private set; }

        private List<BufferItem> buffer = new List<BufferItem>();
        private IEnumerator<BufferItem> enumerator;

        public RingBuffer(IBufferItemFactory bufferItemFactory, int defaultCapacity = 0)
        {
            ItemFactory = bufferItemFactory;
            buffer.Capacity = defaultCapacity;
            enumerator = buffer.GetEnumerator();
        }

        private IEnumerator<IBufferItem> GetBufferEnumerator()
        {
            int count = 0;
            while (true)
            {
                if (buffer.Count > ++count)
                {
                    var newItem = new BufferItem() { Value = ItemFactory.Create() };
                    buffer.Add(newItem);
                    count = 0;
                    yield return newItem;

                    enumerator = buffer.GetEnumerator();
                    continue;
                }

                if (!enumerator.MoveNext())
                {
                    enumerator = buffer.GetEnumerator();
                    enumerator.MoveNext();
                }

                if (!enumerator.Current.IsUsing)
                {
                    count = 0;
                    yield return enumerator.Current; 
                }
            }
        }

        public IEnumerator<IBufferItem> GetEnumerator()
        {
            return GetBufferEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetBufferEnumerator();
        }
    }
}
