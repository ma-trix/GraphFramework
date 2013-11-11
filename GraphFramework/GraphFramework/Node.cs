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

        public void AddNeighbour(Node newNeighbour)
        {
            Neighbours.AddLast(newNeighbour);
            newNeighbour.AddReciprocally(this);
        }

        public void AddReciprocally(Node node)
        {
            Neighbours.AddLast(node);
        }
    }
}