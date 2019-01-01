using System;
using System.Collections.Generic;
using System.Linq;
using TunnelQuest.Data.Models;

namespace TunnelQuest.AppLogic
{
    public static class ChatLineParser
    {
        private static Node rootNode = new Node();

        // static constructor pulls a list of every item name and builds a big tree of letter paths for every possible name
        static ChatLineParser()
        {
            IEnumerable<string> itemNames;
            using (var context = new TunnelQuestContext())
            {
                itemNames = (from item in context.Items
                             orderby item.ItemName
                             select item.ItemName).ToArray();
            }

            foreach (string name in itemNames)
            {
                Node currentNode = rootNode;
                foreach (char letter in name)
                {
                    if (!currentNode.NextChars.ContainsKey(letter))
                        currentNode.NextChars[letter] = new Node();
                    currentNode = currentNode.NextChars[letter];
                }
                currentNode.ItemName = name;
            }
        }

        public static IEnumerable<string> ParseItemNames(string text)
        {
            var itemNames = new List<string>();

            int searchStartIndex = 0;
            while (searchStartIndex < text.Length)
            {
                Node prevNode = rootNode;
                var nodesTraversed = new Stack<Node>();
                int searchEndIndex = searchStartIndex;

                // build nodesTraversed
                while (searchEndIndex < text.Length)
                {
                    if (prevNode.NextChars.ContainsKey(text[searchEndIndex]))
                    {
                        prevNode = prevNode.NextChars[text[searchEndIndex]];
                        nodesTraversed.Push(prevNode);
                        searchEndIndex++;
                    }
                    else
                        break;
                }
                
                // now traverse backwards through nodesTraversed and find the deepest Node that had an itemName (if any)
                while (nodesTraversed.Count > 0)
                {
                    var lastNode = nodesTraversed.Pop();
                    if (lastNode.ItemName != null)
                    {
                        if (!itemNames.Contains(lastNode.ItemName))
                            itemNames.Add(lastNode.ItemName);
                        break;
                    }
                }

                searchStartIndex += nodesTraversed.Count + 1;
            }

            return itemNames;
        }

        // helper class
        private class Node
        {
            public string ItemName { get; set; } = null;
            public Dictionary<char, Node> NextChars { get; } = new Dictionary<char, Node>();
        }
    }
}
