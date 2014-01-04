﻿using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public class Vertex
    {
        public Vertex()
        {
            OutboundArcs = new LinkedList<Arc>();
            InboundArcs = new LinkedList<Arc>();
            Guid = Guid.NewGuid();
        }

        public int OutDegree
        {
            get { return OutboundArcs.Count; }
        }

        public int InDegree
        {
            get { return InboundArcs.Count; }
        }

        public LinkedList<Arc> OutboundArcs { get; private set; }

        public LinkedList<Arc> InboundArcs { get; private set; }

        public Graph Graph { get; set; }

        public Guid Guid { get; private set; }

        public void AddEdge(Vertex newNeighbour)
        {
            AddOutboundArc(newNeighbour);
            newNeighbour.AddOutboundArc(this);
        }
        
        public Arc AddOutboundArc(Vertex endVertex)
        {
            if (ArcHelper.DoesArcExist(this, endVertex, OutboundArcs))
            {
                throw new NoMultiedgePermitedException();
            }
            var newArc = new Arc(Graph, this, endVertex);
            OutboundArcs.AddLast(newArc);
            endVertex.AddInboundArc(newArc);
            return newArc;
        }

        private void AddInboundArc(Arc newArc)
        {
            InboundArcs.AddLast(newArc);
        }

        public void RemoveArc(Vertex vertex)
        {
            if (!ArcHelper.DoesArcExist(this, vertex, OutboundArcs))
                throw new NoArcException();
            vertex.RemoveInboundArc(this);
            ArcHelper.DeleteArc(this, vertex, OutboundArcs);
        }

        private void RemoveInboundArc(Vertex startVertex)
        {
            if (!ArcHelper.DoesArcExist(startVertex, this, InboundArcs))
                throw new NoArcException();
            ArcHelper.DeleteArc(startVertex, this, InboundArcs);
        }

        public void RemoveEdge(Vertex toVertex)
        {
            RemoveArc(toVertex);
            toVertex.RemoveArc(this);
        }

        public void RemoveInboundArcs()
        {
            foreach (var neighbour in InboundArcs)
            {
                neighbour.Start.EndVertexRemoved(this);
            }
            InboundArcs = new LinkedList<Arc>();
        }

        private void EndVertexRemoved(Vertex endVertex)
        {
            if (!ArcHelper.DoesArcExist(this, endVertex, OutboundArcs))
                throw new NoArcException();
            ArcHelper.DeleteArc(this, endVertex, OutboundArcs);
        }

        public void RemoveOutboundArcs()
        {
            foreach (var neighbour in OutboundArcs)
            {
                neighbour.End.RemoveInboundArc(this);
            }
        }
    }
}