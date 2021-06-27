using System;
using System.Collections.Generic;

namespace SummaryApp.Models
{
    public class WordDataList
    {
        public ListNode<WordData> Head;

        public WordDataList() { }
        public WordDataList(List<WordData> list)
        {
            list.ForEach(word => Add(word));
        }

        public void Add(WordData word)
        {
            if (Head == null)
            {
                Head = new ListNode<WordData>(word);
            }
            else
            {
                var last = GetLastNode();
                last.Next = new ListNode<WordData>(word);
            }
        }

        public void AddInOrderOfFrequency(WordData word)
        {
            if (Head == null)
            {
                Head = new ListNode<WordData>(word);
            }
            else
            {

                // Find next node by frequency
                ListNode<WordData> curr = Head;
                ListNode<WordData> prev = null;
                while (curr != null && curr.Data.Frequency > word.Frequency)
                {
                    prev = curr;
                    curr = curr.Next;
                }

                var newNode = new ListNode<WordData>(word);
                if (prev == null && curr.Data.Frequency <= word.Frequency)
                {
                    newNode.Next = curr;
                    Head = newNode;
                }
                else if (prev != null)
                {
                    prev.Next = newNode;
                    newNode.Next = curr;
                }
            }
        }


        public void AddRange(WordDataList wordList)
        {
            var curr = wordList.Head;
            while (curr != null)
            {
                this.Add(curr.Data);
            }
        }

        public bool Remove(WordData wordData)
        {
            var result = false;
            var node = FindFirst(wordData.Word);
            if (node != null)
            {
                result = Remove(node);
            }
            return result;
        }

        public bool Remove(ListNode<WordData> node)
        {
            ListNode<WordData> curr = Head;
            ListNode<WordData> prev = null;

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

        private ListNode<WordData> FindFirst(string data)
        {
            var curr = Head;

            while (curr != null && !curr.Data.Word.Equals(data))
            {
                curr = curr.Next;
            }

            return curr;
        }

        public WordData FirstOrDefault(Func<WordData, bool> pred)
        {
            WordData data = null;
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


        public void Sort()
        {
            var orderedList = new WordDataList();

            var curr = Head;
            while (curr != null)
            {
                orderedList.AddInOrderOfFrequency(curr.Data);
                curr = curr.Next;
            }

            Head = orderedList.Head;
        }


        private ListNode<WordData> GetLastNode()
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

        public int Max(Func<WordData, int> mapper)
        {
            var max = 0;
            var curr = Head;

            while (curr != null)
            {
                var result = mapper(curr.Data);
                if (result > max)
                {
                    max = result;
                }
                curr = curr.Next;
            }

            return max;
        }

        public void PrintWordDataList()
        {
            var curr = Head;

            while (curr != null)
            {
                var data = curr.Data;
                Console.WriteLine($"Word: {data.Word} | Frequency: {data.Frequency}");
                curr = curr.Next;
            }
        }
    }
}
