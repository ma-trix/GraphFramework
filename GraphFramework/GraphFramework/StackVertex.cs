using System.Collections.Generic;

namespace GraphFramework
{
    public class StackVertex
    {
        public StackVertex(ABVertex value, ABVertex ancestor)
        {
            Value = value;
            Ancestor = ancestor;
            Descendants = new LinkedList<ABVertex>();
        }

        public ABVertex Value { get; private set; }
        public ABVertex Ancestor { get; private set; }
        public LinkedList<ABVertex> Descendants { get; private set; }

        public void AddDescendant(ABVertex vertex)
        {
            Descendants.AddLast(vertex);
        }
    }
}