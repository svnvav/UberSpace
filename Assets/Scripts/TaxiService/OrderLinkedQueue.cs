namespace Svnvav.UberSpace
{
    public class OrderLinkedQueue
    {
        private class Item
        {
            internal Order value;
            internal Item prev, next;
        }

        private Item _head, _tail;
        private int _count = 0;

        public int Count => _count;

        public void Enqueue(Order order)
        {
            if (_count == 0)
            {
                _head = new Item()
                {
                    value = order
                };
                _tail = _head;
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
        }

        public Order Dequeue()
        {
            if (_count == 0) return null;

            var dequeued = _head;
            _head = _head.next;
            _count--;
            
            return dequeued.value;
        }
    }
}