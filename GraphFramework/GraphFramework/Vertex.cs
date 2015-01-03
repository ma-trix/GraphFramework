using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public class Vertex : IVertex
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Vertex()
        {
            OutboundArcs = new LinkedList<Arc>();
            InboundArcs = new LinkedList<Arc>();
            Guid = Guid.NewGuid();
            _name = Guid.ToString();
        }

        public Vertex(string name) : this()
        {
            _name = name;
        }

        public int OutDegree
        {
            get { return OutboundArcs.Count; }
        }

        public int InDegree
        {
            get { return InboundArcs.Count; }
        }

        public virtual String Name { get { return _name; } }
        protected String _name;

        public LinkedList<Arc> OutboundArcs { get; private set; }

        public LinkedList<Arc> InboundArcs { get; private set; }

        public Graph Graph { get; set; }

        public Guid Guid { get; private set; }

        public bool IsInMatching { get; set; }

        public void AddEdge(IVertex newNeighbour, bool inMatching)
        {
            AddOutboundArc(newNeighbour, inMatching);
            newNeighbour.AddOutboundArc(this, inMatching);
        }
        
        public Arc AddOutboundArc(IVertex endVertex, bool inMatching)
        {
            if (ArcHelper.DoesArcExist(this, endVertex, OutboundArcs))
            {
                throw new NoMultiedgePermitedException();
            }
            var newArc = new Arc(Graph, this, endVertex, inMatching);
            OutboundArcs.AddLast(newArc);
            if (inMatching)
            {
                AddToMatching();
            }
            Log.Info("Added outbound arc " + newArc + " to vertex " + Name);
            endVertex.AddInboundArc(newArc, inMatching);
            return newArc;
        }

        public void AddInboundArc(Arc newArc, bool inMatching)
        {
            InboundArcs.AddLast(newArc);
            if (inMatching)
            {
                AddToMatching();
            }
            Log.Info("Added inbound arc " + newArc + " to vertex " + Name);
        }

        private void AddToMatching()
        {
            IsInMatching = true;
            Log.Info("Added vertex " + Name + " to matching");
        }

        public void RemoveArc(IVertex vertex)
        {
            Log.Info("Removing outbound arc from vertex " + Name);
            if (!ArcHelper.DoesArcExist(this, vertex, OutboundArcs))
                throw new NoArcException();
            vertex.RemoveInboundArc(this);
            ArcHelper.DeleteArc(this, vertex, OutboundArcs);
        }

        public void RemoveInboundArc(IVertex startVertex)
        {
            Log.Info("Removing inbound arc from vertex " + Name);
            if (!ArcHelper.DoesArcExist(startVertex, this, InboundArcs))
                throw new NoArcException();
            ArcHelper.DeleteArc(startVertex, this, InboundArcs);
        }

        public void RemoveEdge(IVertex toVertex)
        {
            RemoveArc(toVertex);
            toVertex.RemoveArc(this);
        }

        public void RemoveInboundArcs()
        {
            Log.Info("Removing inbound arcs from vertex " + Name);
            foreach (var neighbour in InboundArcs)
            {
                neighbour.Start.EndVertexRemoved(this);
            }
            InboundArcs = new LinkedList<Arc>();
        }

        public void EndVertexRemoved(IVertex endVertex)
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
    
        public virtual void ArcReverted(Arc arc)
        {
            if (!IsInMatching)
            {
                AddToMatching();
            }
            if (arc.Start == this)
            {
                InboundArcs.Remove(arc);
                OutboundArcs.AddLast(arc);
            }
            if (arc.End == this)
            {
                OutboundArcs.Remove(arc);
                InboundArcs.AddLast(arc);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public double DoubleWeight { get; set; }
    }
}