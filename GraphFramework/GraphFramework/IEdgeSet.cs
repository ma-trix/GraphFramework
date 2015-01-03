using System.Collections.Generic;

namespace GraphFramework
{
    public interface IEdgeSet
    {
        LinkedList<Arc> Edges { get; set; }
        double Weight { get; set; }
    }
}