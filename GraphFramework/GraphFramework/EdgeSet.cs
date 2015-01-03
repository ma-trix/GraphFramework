using System.Collections.Generic;

namespace GraphFramework
{
    public class EdgeSet : IEdgeSet
    {
        public EdgeSet(LinkedList<Arc> edges, double weight)
        {
            Weight = weight;
            Edges = edges;
        }

        public LinkedList<Arc> Edges { get; set; }

        public double Weight { get; set; }
    }
}