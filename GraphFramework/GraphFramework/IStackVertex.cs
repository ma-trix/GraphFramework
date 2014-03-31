using System.Collections.Generic;

namespace GraphFramework
{
    public interface IStackVertex
    {
        ABVertex Value { get; }
        IStackVertex Ancestor { get; }
        LinkedList<IStackVertex> Descendants { get; }
        bool IsExpanded { get; }
        ExpandedArc ExpandedArc { get; }
        void AddDescendant(IStackVertex vertex);
    }
}