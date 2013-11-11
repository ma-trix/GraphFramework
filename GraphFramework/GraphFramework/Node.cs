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
            if (Neighbours.Contains(newNeighbour))
                throw new NoMultiedgePermitedException();
            Neighbours.AddLast(newNeighbour);
            if (newNeighbour.Neighbours.Contains(this))
                throw new NoMultiedgePermitedException();
            newNeighbour.AddReciprocally(this);
        }

        public void AddReciprocally(Node node)
        {
            Neighbours.AddLast(node);
        }

        public void AddArc(Node node)
        {
            if (Neighbours.Contains(node))
                throw new NoMultiedgePermitedException();
            Neighbours.AddLast(node);
        }
    }
}