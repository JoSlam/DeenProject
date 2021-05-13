namespace SummaryApp.Models
{
    public class ListNode<T>
    {
        public T Data;
        public ListNode<T> Next;

        public ListNode(T data)
        {
            this.Data = data;
        }
    }
}
