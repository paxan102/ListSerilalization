using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

class ListRand
{
    public ListNode head;
    public ListNode tail;
    public int count;

    public void Serialize(FileStream s)
    {
        Console.WriteLine("Serialization start");

        ListNode[] nodes = new ListNode[count];
        int[] indecesOfRandNodes = new int[count];
        var currentNode = head;
        string serializeData = "";

        for (int nodeIdx = 0; nodeIdx < count; nodeIdx++)
        {
            serializeData += currentNode.data.ToString() + Environment.NewLine;

            ListNode forwardNode = currentNode.rand;
            ListNode backNode = currentNode.rand;
            int forward = 0;
            int back = 0;

            if (currentNode.rand == null)
            {
                indecesOfRandNodes[nodeIdx] = -1;
            }
            else
            {
                while (forwardNode != tail && backNode != head)
                {
                    forwardNode = forwardNode.next;
                    forward++;
                    backNode = backNode.prev;
                    back++;
                }

                if (forwardNode == tail)
                    indecesOfRandNodes[nodeIdx] = count - 1 - back;
                if (backNode == head)
                    indecesOfRandNodes[nodeIdx] = forward;
            }
            currentNode = currentNode.next;
        }

        string serializeString = count.ToString() + Environment.NewLine;

        foreach (var idx in indecesOfRandNodes)
            serializeString += idx.ToString() + " ";

        serializeString += Environment.NewLine + serializeData;

        byte[] serializeBytes = Encoding.Default.GetBytes(serializeString);
        s.Write(serializeBytes, 0, serializeBytes.Length);
        s.Flush();
        s.Close();

        Console.WriteLine("Serialization finish");
        Console.WriteLine();
    }
    public void Deserialize(FileStream s)
    {
        Console.WriteLine("Deserialization start");

        string textFromFile;
        byte[] bytesFromFile = new byte[s.Length];
        s.Read(bytesFromFile, 0, bytesFromFile.Length);
        textFromFile = Encoding.Default.GetString(bytesFromFile);

        string[] stringsFromFile = textFromFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        int countOfNodes = Int32.Parse(stringsFromFile[0]);

        ListNode[] nodes = new ListNode[countOfNodes];
        int[] indecesOfRandNodes = new int[countOfNodes];

        string[] stringsOfRandNodes = stringsFromFile[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        for(int idx = 0; idx < stringsOfRandNodes.Length; idx++)
            indecesOfRandNodes[idx] = Int32.Parse(stringsOfRandNodes[idx]);

        count = countOfNodes;
        for(int idx = 2; idx < stringsFromFile.Length - 1; idx++)
        {
            var node = new ListNode();
            nodes[idx - 2] = node;
            node.data = stringsFromFile[idx];

            if (idx == 2)
            {
                head = node;
                tail = node;
            }
            else if (idx == 3)
            {
                tail = node;
                head.next = tail;
                tail.prev = head;
            }
            else
            {
                var temp = tail;
                tail = node;

                tail.prev = temp;
                temp.next = tail;
            }
        }

        var tempNode = head;
        for (int idx = 0; idx < indecesOfRandNodes.Length; idx++)
        {
            if (indecesOfRandNodes[idx] == -1)
                return;
            tempNode.rand = nodes[indecesOfRandNodes[idx]];
            tempNode = tempNode.next;
        }

        s.Close();

        Console.WriteLine("Deserialization finish");
        Console.WriteLine();
    }
}
