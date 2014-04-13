using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public interface IVertex
    {
        int OutDegree { get; }
        int InDegree { get; }
        String Name { get; }
        LinkedList<Arc> OutboundArcs { get; }
        LinkedList<Arc> InboundArcs { get; }
        Graph Graph { get; set; }
        Guid Guid { get; }
        bool IsInMatching { get; set; }
        void AddEdge(IVertex newNeighbour, bool inMatching);
        Arc AddOutboundArc(IVertex endVertex, bool inMatching);
        void RemoveArc(IVertex vertex);
        void RemoveEdge(IVertex toVertex);
        void RemoveInboundArcs();
        void RemoveOutboundArcs();
        void EndVertexRemoved(IVertex endVertex);
        void RemoveInboundArc(IVertex startVertex);
        void AddInboundArc(Arc newArc, bool inMatching);
        void ArcReverted(Arc arc);
    }
}