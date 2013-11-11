using System.Collections.Generic;

namespace GraphFramework
{
    public class Graph
    {
        public LinkedList<Node> nodes = new LinkedList<Node>();

        public void AddNode(Node node)
        {
            nodes.AddLast(node);
        }
    }
}
