namespace SummaryApp.Models
{
    public class WordList
    {
        public ListNode<string> Head;

        public void Add(string word)
        {
            if (Head == null)
            {
                Head = new ListNode<string>(word);
            }
            else
            {
                var last = GetLastNode();
                last.Next = new ListNode<string>(word);
            }
        }

        public bool Remove(ListNode<string> node)
        {
            ListNode<string> curr = Head;
            ListNode<string> prev = null;

            while (curr != null && curr != node)
            {
                prev = curr;
                curr = curr.Next;
            }

            if (curr != null && prev != null && curr == node)
            {
                prev.Next = curr.Next;
                return true;
            }
            else
                return false;
        }

        private ListNode<string> FindFirst(string data)
        {
            var curr = Head;

            while (curr != null && curr.Data.Equals(data))
            {
                curr = curr.Next;
            }

            return curr;
        }


        private ListNode<string> GetLastNode()
        {
            var curr = Head;
            while (curr.Next != null) curr = curr.Next;
            return curr;
        }
    }
}
