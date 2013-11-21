using System.Collections.Generic;

namespace GraphFramework
{
    public class Vertex
    {
        private LinkedList<Vertex> _outbound;
        private LinkedList<Vertex> _inbound;

        public Vertex()
        {
            _outbound = new LinkedList<Vertex>();
            _inbound = new LinkedList<Vertex>();
        }

        public int OutDegree
        {
            get { return _outbound.Count; }
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

        public void AddEdge(Vertex newNeighbour)
        {
            AddArc(newNeighbour);
            newNeighbour.AddArc(this);
        }
        
        public void AddArc(Vertex vertex)
        {
            if (_outbound.Contains(vertex))
                throw new NoMultiedgePermitedException();
            _outbound.AddLast(vertex);
            vertex.AddInboundArc(this);
        }

        private void AddInboundArc(Vertex fromVertex)
        {
            _inbound.AddLast(fromVertex);
        }

        public void RemoveArc(Vertex vertex)
        {
            if (!_outbound.Contains(vertex))
                throw new NoArcException();
            vertex.RemoveInboundArc(this);
            _outbound.Remove(vertex);
        }

        private void RemoveInboundArc(Vertex fromVertex)
        {
            if (!_inbound.Contains(fromVertex))
                throw new NoArcException();
            _inbound.Remove(fromVertex);
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
        }

        private void EndVertexRemoved(Vertex endVertex)
        {
            if (!_outbound.Contains(endVertex))
                throw new NoArcException();
            _outbound.Remove(endVertex);
        }

        public void RemoveOutboundArcs()
        {
            foreach (var neighbour in _outbound)
            {
                neighbour.RemoveInboundArc(this);
            }
        }
    }
}