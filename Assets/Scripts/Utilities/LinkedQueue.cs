
using System;
using System.Collections;
using System.Collections.Generic;

namespace Svnvav.UberSpace
{
    public class LinkedQueue<T> : IEnumerable<T> where T : class
    {
        private class Item
        {
            internal T value;
            internal Item prev, next;
        }

        private Item _head, _tail;
        private int _count = 0;

        public int Count => _count;

        public Action<T> OnEnqueue, OnDequeue;

        public void Clear()
        {
            _head = _tail = null;
            _count = 0;
        }
        
        public T Peek()
        {
            return _head?.value;
        }
        
        public void Enqueue(T order)
        {
            if (_count == 0)
            {
                _head = _tail = new Item()
                {
                    value = order
                };
            }
            else
            {
                var newTail = new Item()
                {
                    value = order,
                    prev = _tail
                };
                _tail.next = newTail;
                _tail = newTail;
            }

            _count++;
            
            OnEnqueue?.Invoke(order);
        }

        public T Dequeue()
        {
            if (_count == 0) return null;

            var dequeued = _head;
            _head = _head.next;

            _count--;
            if (_count != 0)
            {
                _head.prev.next = null;
                _head.prev = null;
            }
            
            OnDequeue?.Invoke(dequeued.value);
            return dequeued.value;
        }

        public T RemoveAndGetLast(Func<T, bool> predicate)
        {
            var iterator = _tail;
            while (iterator != null)
            {
                if (predicate(iterator.value))
                {
                    Remove(iterator);
                    return iterator.value;
                }
                iterator = iterator.prev;
            }

            return null;
        }

        public void RemoveAll(Func<T, bool> predicate)
        {
            var iterator = _tail;
            while (iterator != null)
            {
                if (predicate(iterator.value))
                {
                    Remove(iterator);
                }
                iterator = iterator.prev;
            }
        }

        private void Remove(Item item)
        {
            var prev = item.prev;
            if (prev != null)
            {
                prev.next = item.next;
            }

            var next = item.next;
            if (next != null)
            {
                next.prev = item.prev;
            }

            if (item == _head)
            {
                _head = item.next;
            }

            if (item == _tail)
            {
                _tail = item.prev;
            }
            
            _count--;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ItemsEnumerator(_head);
        }
        
        private class ItemsEnumerator : IEnumerator<T>
        {
            private Item _head, _current;

            public ItemsEnumerator(Item head)
            {
                _head = head;
            }

            public bool MoveNext()
            {
                _current = _current == null ? _head : _current.next;

                return _current != null;
            }

            public void Reset()
            {
                _current = null;
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _current = _head = null;
                GC.SuppressFinalize(this);
            }

            public T Current => _current.value;
        }
    }
}