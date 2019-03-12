using System;
using System.IO;
using System.Collections.Generic;

class ListSerialization
{
    static void Main(string[] args)
    {
        ListRand listRand = new ListRand();
        ListRand newListRand = new ListRand();
        FillList(listRand, 25000);
        //ShowList(listRand);


        DateTime dateTime1 = DateTime.Now;
        FileStream fileStream = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\file.txt", FileMode.Create);
        listRand.Serialize(fileStream);
        fileStream = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\file.txt", FileMode.Open);
        newListRand.Deserialize(fileStream);

        //ShowList(newListRand);
        DateTime dateTime2 = DateTime.Now;
        Console.WriteLine(dateTime2.Subtract(dateTime1).TotalSeconds);
        Console.ReadKey();
    }

    static void ShowList(ListRand listRand)
    {
        var temp = listRand.head;
        for (int idx = 0; idx < listRand.count; idx++)
        {
            Console.WriteLine(temp.data + " " + temp.rand.data);

            if (idx < listRand.count - 1)
                temp = temp.next;
        }

        Console.WriteLine();
    }

    static void FillList(ListRand listRand, int count)
    {
        for (int idx = 0; idx < count; idx++)
            AddNodeToList(listRand, idx.ToString());
        
        AddRandNodesInList(listRand);
    }

    static void AddNodeToList(ListRand listRand, string data)
    {
        var node = new ListNode();
        node.data = data;

        if (listRand.count == 0)
        {            
            listRand.head = node;
            listRand.tail = node;
        }
        else if (listRand.count == 1)
        {
            listRand.tail = node;
            listRand.head.next = listRand.tail;
            listRand.tail.prev = listRand.head;
        }
        else
        {
            var temp = listRand.tail;
            listRand.tail = node;

            listRand.tail.prev = temp;
            temp.next = listRand.tail;
        }
        listRand.count++;
    }

    static void AddRandNodeToNode(ListRand listRand, List<int> indices, ListNode node)
    {
        var randomIdx = GetRandomInt(indices.Count);
        node.rand = GetNode(listRand, indices[randomIdx]);
        indices.RemoveAt(randomIdx);
    }

    static void AddRandNodesInList(ListRand listRand)
    {
        var indices = new List<int>();
        for (int idx = 0; idx < listRand.count; idx++)
            indices.Add(idx);

        AddRandNodeToNode(listRand, indices, listRand.head);

        var temp = listRand.head.next;
        for (int idx = 1; idx < listRand.count; idx++)
        {
            AddRandNodeToNode(listRand, indices, temp);

            if (idx != listRand.count - 1)
                temp = temp.next;
        }
    }

    static int GetRandomInt(int max)
    {
        var random = new Random();
        return random.Next(max);
    }

    static ListNode GetNode(ListRand listRand, int nodeIdx)
    {
        ListNode resultNode = null;

        if (nodeIdx < listRand.count / 2)
        {
            resultNode = listRand.head;
            for (int idx = 0; idx < nodeIdx; idx++)
                resultNode = resultNode.next;
        }
        else
        {
            resultNode = listRand.tail;
            for (int idx = listRand.count - 1; idx > nodeIdx; idx--)
                resultNode = resultNode.prev;
        }

        return resultNode;
    }
}
