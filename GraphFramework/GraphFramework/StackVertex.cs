using System.Collections.Generic;

namespace GraphFramework
{
    public class StackVertex
    {
        public bool isExpanded;
        public ExpandedArc expandedArc;

        public StackVertex(ABVertex value, StackVertex ancestor)
        {
            Value = value;
            Ancestor = ancestor;
            Descendants = new LinkedList<ABVertex>();
        }

        public ABVertex Value { get; private set; }
        public StackVertex Ancestor { get; private set; }
        public LinkedList<ABVertex> Descendants { get; private set; }

        public void AddDescendant(ABVertex vertex)
        {
            Descendants.AddLast(vertex);
        }
    }

    public class ExpandedArc
    {
        public StackVertex Start;
        public StackVertex End;
    }
}