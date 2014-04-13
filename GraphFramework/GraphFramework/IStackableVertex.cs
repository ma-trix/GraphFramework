using System.Collections.Generic;

namespace GraphFramework
{
    public interface IStackableVertex
    {
        ABVertex Value { get; }
        IStackableVertex Ancestor { get; }
        Arc ArcFromAncestor { get; }
        LinkedList<IStackableVertex> Descendants { get; }
        bool IsExpanded { get; }
        ExpandedArc ExpandedArc { get; }
        void AddDescendant(IStackableVertex vertex);
    }
}