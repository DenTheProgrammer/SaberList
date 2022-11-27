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
            ListRand deserializedList = new ListRand();
            deserializedList.Deserialize(fs);
            Console.WriteLine($"Created list == deserialized list? - {deserializedList.Equals(list)}");
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

                node.Data = @$"data{i} | {rand.NextDouble()} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer vel lorem augue. 
Praesent molestie venenatis augue, id feugiat felis. Maecenas lobortis tortor at turpis dapibus, at ullamcorper mauris rutrum. Nulla mollis volutpat semper. Sed tristique arcu nec nunc lacinia, quis congue urna sagittis. Nam mattis fringilla ipsum at rutrum. Nunc accumsan ligula turpis, faucibus vehicula erat accumsan id.

Suspendisse eget urna malesuada justo convallis convallis. Quisque tincidunt ac purus vel feugiat. Curabitur dolor massa, sollicitudin in molestie sed, dapibus ut nulla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Praesent sed ornare dui. Vestibulum in lectus ornare lorem accumsan tristique quis vel ligula. Sed at augue tortor. Praesent orci massa, dapibus a elementum nec, hendrerit ac enim. Sed sagittis scelerisque sollicitudin. Sed sed libero fermentum, condimentum dui et, ullamcorper purus.";
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
