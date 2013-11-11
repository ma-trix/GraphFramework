using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class Node
    {
    }

    public class GraphGenerator
    {
        public Graph gerenarteRandomGraph()
        {
            Graph g = new Graph();

            return g;
        }
    }
}
