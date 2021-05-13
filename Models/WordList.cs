using System;
using System.Collections.Generic;

namespace SummaryApp.Models
{
    public class WordList
    {
        public ListNode<string> Head;

        public WordList() { }
        public WordList(List<string> list)
        {
            list.ForEach(word => Add(word));
        }

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

        public void AddRange(WordList wordList)
        {
            var curr = wordList.Head;
            while (curr != null)
            {
                this.Add(curr.Data);
                curr = curr.Next;
            }
        }

        public bool Remove(string word)
        {
            var result = false;
            var node = FindFirst(word);
            if (node != null)
            {
                result = Remove(node);
            }
            return result;
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

            if (prev == null && curr == node)
            {
                Head = curr.Next;
                return true;
            }
            else if (prev != null && curr == node)
            {
                prev.Next = curr.Next;
                return true;
            }
            else
                return false;
        }

        public WordDataList Select(Func<string, WordData> handler)
        {
            var resultList = new WordDataList();
            var curr = Head;

            while (curr != null)
            {
                var handlerResult = handler(curr.Data);
                resultList.Add(handlerResult);
                curr = curr.Next;
            }
            return resultList;
        }

        public WordList SelectMany(Func<string, WordList> handler)
        {
            var flattenedList = new WordList();

            var curr = Head;
            while (curr != null)
            {
                var handlerResult = handler(curr.Data);
                flattenedList.AddRange(handlerResult);

                curr = curr.Next;
            }
            return flattenedList;
        }

        private ListNode<string> FindFirst(string data)
        {
            var curr = Head;

            while (curr != null && !curr.Data.Equals(data))
            {
                curr = curr.Next;
            }

            return curr;
        }

        public string FirstOrDefault(Func<string, bool> pred)
        {
            string data = null;
            var result = false;

            var curr = Head;
            while (curr != null && !result)
            {
                result = pred(curr.Data);
                if (result)
                {
                    data = curr.Data;
                }
                curr = curr.Next;
            }

            return data;
        }

        private ListNode<string> GetLastNode()
        {
            var curr = Head;
            while (curr.Next != null) curr = curr.Next;
            return curr;
        }

        public int Count()
        {
            var count = 0;
            var curr = Head;

            while (curr != null)
            {
                count++;
                curr = curr.Next;
            }

            return count;
        }

        public int Count(Func<string, bool> pred)
        {
            var curr = Head;
            var count = 0;

            while (curr != null)
            {
                var result = pred(curr.Data);
                if (result)
                {
                    count++;
                }
                curr = curr.Next;
            }

            return count;
        }

        public void PrintWordList()
        {
            var curr = Head;

            while (curr != null)
            {
                Console.WriteLine(curr.Data);
                curr = curr.Next;
            }
        }
    }
}
