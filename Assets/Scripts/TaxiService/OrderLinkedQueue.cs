namespace Svnvav.UberSpace
{
    public class OrderLinkedQueue
    {
        private class Item
        {
            private Order value;
            private Item prev, next;
        }

        private Item _head, _tail;

        public void Enqueue(Order order)
        {
            if (_tail == null && _head == null)
            {
            }
        }
    }
}