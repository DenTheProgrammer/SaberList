using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SaberList
{
    class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; // произвольный элемент внутри списка
        public string Data;

        public override bool Equals(object obj)
        {
            ListNode node = (ListNode)obj;
            if (!Data.Equals(node.Data)) return false;
            if (Rand != null)
            {
                if (!Rand.Data.Equals(node.Rand.Data)) return false;
            }
            return true;
        }
    }


    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public override bool Equals(object obj)
        {
            ListRand list = (ListRand)obj;
            if (Count != list.Count) return false;
            ListNode thisCur = Head;
            ListNode otherCur = list.Head;
            for (int i = 0; i < list.Count; i++)
            {
                if (!thisCur.Equals(otherCur)) return false;
                thisCur = thisCur.Next;
                otherCur = otherCur.Next;
            }
            return true;
        }

        public void Serialize(FileStream s)
        {

            using (StreamWriter sw = new StreamWriter(s))
            {
                sw.WriteLine(Count);

                ListNode curNode = Head;
                Dictionary<ListNode, int> nodeIndDict = new Dictionary<ListNode, int>();
                for (int i = 0; i < Count; i++)//nodes indexing
                {
                    nodeIndDict.Add(curNode, i);
                    curNode = curNode.Next;
                }

                curNode = Head;
                for (int i = 0; i < Count; i++)//serializing
                {
                    string randIndex = curNode.Rand == null ? "null" : nodeIndDict[curNode.Rand].ToString();
                    string nodeFullInfo = $"{randIndex}\n{curNode.Data.Length}\n{curNode.Data}\n";

                    sw.Write(nodeFullInfo);
                    curNode = curNode.Next;
                }
            };
        }

        public void Deserialize(FileStream s)
        {
            if (!s.CanRead)
            {
                Console.WriteLine("Can't read file");
                return;
            }
            using (StreamReader sr = new StreamReader(s))
            {
                int nodeCount = int.Parse(sr.ReadLine());
                Count = nodeCount;
                List<int?> randIndexes = new List<int?>();
                Dictionary<int, ListNode> nodeIndDict = new Dictionary<int, ListNode>();
                ListNode prev = null;
                for (int i = 0; i < nodeCount; i++)
                {
                    //parse rand indexes
                    ListNode node = new ListNode();
                    nodeIndDict.Add(i, node);
                    int randNodeIndex;
                    if (int.TryParse(sr.ReadLine(), out randNodeIndex))
                    {
                        randIndexes.Add(randNodeIndex);   
                    }
                    else
                    {
                        randIndexes.Add(null);
                    }
                    //parse data
                    int dataSize = int.Parse(sr.ReadLine());
                    StringBuilder sb = new StringBuilder();
                    while(sb.Length < dataSize)
                    {
                        string str = sr.ReadLine();
                        if (str.Length + sb.Length == dataSize)
                        {
                            sb.Append(str);
                        }
                        else
                        {
                            sb.AppendLine(str);
                        }
                    }
                    node.Data = sb.ToString();
                    //link prev-next
                    if (i == 0)//first
                    {
                        Head = node;
                    }
                    if (i == nodeCount - 1)//last
                    {
                        node.Next = null;
                        Tail = node;
                    }
                    if (prev != null)//not first
                    {
                        prev.Next = node;
                        node.Prev = prev;
                    }

                    prev = node;
                }
                //link rands
                ListNode curNode = Head;
                for (int i = 0; i < Count; i++)
                {
                    if (randIndexes[i] == null)
                    {
                        curNode.Rand = null;
                    }
                    else
                    {
                        int index = (int)randIndexes[i];
                        curNode.Rand = nodeIndDict[index];
                    }

                    curNode = curNode.Next;
                }

            };
        }
    }
}
