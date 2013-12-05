using System.Collections.Generic;
using System.Xml.Linq;

namespace GraphFramework
{
    public class Vertex
    {
        private LinkedList<Vertex> _outbound;
        private LinkedList<Vertex> _inbound;
        private LinkedList<Arc> _outboundArcs;
        private LinkedList<Arc> _inboundArcs;

        public Vertex()
        {
            _outbound = new LinkedList<Vertex>();
            _inbound = new LinkedList<Vertex>();
            _outboundArcs = new LinkedList<Arc>();
            _inboundArcs = new LinkedList<Arc>();
        }

        public int OutDegree
        {
            get { return _outboundArcs.Count; }
        }

        public LinkedList<Vertex> Outbound
        {
            get { return _outbound; }
        }

        public LinkedList<Vertex> Inbound
        {
            get { return _inbound; }
        }

        public int InDegree
        {
            get { return _inbound.Count; }
        }

        public LinkedList<Arc> OutboundArcs
        {
            get { return _outboundArcs; }
        }

        public LinkedList<Arc> InboundArcs
        {
            get { return _inboundArcs; }
        }

        public void AddEdge(Vertex newNeighbour)
        {
            AddArc(newNeighbour);
            newNeighbour.AddArc(this);
        }
        
        public void AddArc(Vertex vertex)
        {
            if (DoesArcExist(this, vertex, _outboundArcs))
            {
                throw new NoMultiedgePermitedException();
            }
            _outbound.AddLast(vertex);
            var newArc = new Arc(null, this, vertex);
            _outboundArcs.AddLast(newArc);
            vertex.AddInboundArc(newArc);
        }

        private bool DoesArcExist(Vertex start, Vertex end, LinkedList<Arc> inList)
        {
            var arc = inList.First;
            while (arc != null)
            {
                var nextArc = arc.Next;
                if (arc.Value.Start == start && arc.Value.End == end)
                {
                    return true;
                }
                arc = nextArc;
            }
            return false;
        }

        private void AddInboundArc(Arc newArc)
        {
            _inboundArcs.AddLast(newArc);
            _inbound.AddLast(newArc.Start);
        }

        public void RemoveArc(Vertex vertex)
        {
            if (!DoesArcExist(this, vertex, _outboundArcs))
                throw new NoArcException();
            vertex.RemoveInboundArc(this);
            _outbound.Remove(vertex);
            DeleteArc(this, vertex, _outboundArcs);
        }

        private void DeleteArc(Vertex start, Vertex end, LinkedList<Arc> fromList)
        {
            var arc = fromList.First;
            while (arc != null)
            {
                var nextArc = arc.Next;
                if (arc.Value.Start == start && arc.Value.End == end)
                {
                    fromList.Remove(arc);
                }
                arc = nextArc;
            }
        }

        private void RemoveInboundArc(Vertex fromVertex)
        {
            if (!_inbound.Contains(fromVertex))
                throw new NoArcException();
            _inbound.Remove(fromVertex);
            DeleteArc(fromVertex, this, _inboundArcs);
        }

        public void RemoveEdge(Vertex toVertex)
        {
            RemoveArc(toVertex);
            toVertex.RemoveArc(this);
        }

        public void RemoveInboundArcs()
        {
            foreach (var vertex in _inbound)
            {
                vertex.EndVertexRemoved(this);
            }
            _inbound = new LinkedList<Vertex>();
            _inboundArcs = new LinkedList<Arc>();
        }

        private void EndVertexRemoved(Vertex endVertex)
        {
            if (!DoesArcExist(this, endVertex, _outboundArcs))
                throw new NoArcException();
            _outbound.Remove(endVertex);
            DeleteArc(this, endVertex, _outboundArcs);
        }

        public void RemoveOutboundArcs()
        {
            foreach (var neighbour in _outboundArcs)
            {
                neighbour.End.RemoveInboundArc(this);
            }
        }
    }
}