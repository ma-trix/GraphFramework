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

        public int OutDegree
        {
            get { return Neighbours.Count; }
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