using System.Collections.Generic;

namespace GraphFramework
{
    public class Node
    {
        public LinkedList<Node> Neighbours;

        public Node()
        {
            Neighbours = new LinkedList<Node>();
        }

        public void AddEdge(Node newNeighbour)
        {
            AddArc(newNeighbour);
            newNeighbour.AddArc(this);
        }
        
        public void AddArc(Node node)
        {
            if (Neighbours.Contains(node))
                throw new NoMultiedgePermitedException();
            Neighbours.AddLast(node);
        }
    }
}