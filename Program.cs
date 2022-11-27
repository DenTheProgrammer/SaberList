using System;
using System.Collections.Generic;
using System.IO;

namespace SaberList
{
    class Program
    {
        const string SAVE_PATH = "./save.txt";
        static void Main(string[] args)
        {
            FileStream fs = new FileStream(SAVE_PATH, FileMode.Create, FileAccess.ReadWrite);
            ListRand list = CreateList();
            list.Serialize(fs);


            fs.Close();
            fs = new FileStream(SAVE_PATH, FileMode.Open, FileAccess.Read);
            ListRand deserealizedList = new ListRand();
            deserealizedList.Deserialize(fs);
            Console.WriteLine($"Ser = des? - {deserealizedList.Equals(list)}");


            //test
            /*List<string> tests = new List<string> { "34 435seff", "42 segfg5", "null" };

            Console.WriteLine("parse int tests:");
            foreach (string str in tests)
            {
                int res;
                if (int.TryParse(str, out res))
                {
                    Console.WriteLine($"{str} - {res}");
                }
                else
                {
                    Console.WriteLine($"{str} - null");
                }
            }*/
            //

            fs.Close();
        }

        private static ListRand CreateList()
        {
            ListRand listRand = new ListRand();
            Random rand = new Random(DateTime.Now.Millisecond);
            int length = rand.Next(0, 10);
            List<ListNode> tmpNodeList = new List<ListNode>();

            ListNode prev = null;
            for (int i = 0; i < length; i++)
            {
                ListNode node = new ListNode();
                tmpNodeList.Add(node);
                if (i == 0)//first node
                {
                    listRand.Head = node;
                }
                if (i == length - 1)//last node
                {
                    node.Next = null;
                    listRand.Tail = node;
                }

                node.Data = @$"data{i}
                    more 
                    data...";
                if (prev != null)//all nodes except first
                {
                    node.Prev = prev;
                    prev.Next = node;
                }
                prev = node;
            }
            listRand.Count = length;

            foreach (var node in tmpNodeList)
            {
                if (rand.NextDouble() > 0.5)//has rand pointer
                {
                    node.Rand = tmpNodeList[rand.Next(0, tmpNodeList.Count)];
                }
                else
                {
                    node.Rand = null;
                }
            }
            
            return listRand;
        }
    }
}
