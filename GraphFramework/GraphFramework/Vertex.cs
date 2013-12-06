using System.Collections.Generic;

namespace GraphFramework
{
    public class Vertex
    {
        private LinkedList<Arc> _outboundArcs;
        private LinkedList<Arc> _inboundArcs;
        private readonly ArcHelper _arcHelper;

        public Vertex()
        {
            _outboundArcs = new LinkedList<Arc>();
            _inboundArcs = new LinkedList<Arc>();
            _arcHelper = new ArcHelper();
        }

        public int OutDegree
        {
            get { return _outboundArcs.Count; }
        }

        public int InDegree
        {
            get { return _inboundArcs.Count; }
        }

        public LinkedList<Arc> OutboundArcs
        {
            get { return _outboundArcs; }
        }

        public LinkedList<Arc> InboundArcs
        {
            get { return _inboundArcs; }
        }

        public Graph Graph { get; set; }
        
        public void AddEdge(Vertex newNeighbour)
        {
            AddOutboundArc(newNeighbour);
            newNeighbour.AddOutboundArc(this);
        }
        
        public Arc AddOutboundArc(Vertex endVertex)
        {
            if (ArcHelper.DoesArcExist(this, endVertex, _outboundArcs))
            {
                throw new NoMultiedgePermitedException();
            }
            var newArc = new Arc(Graph, this, endVertex);
            _outboundArcs.AddLast(newArc);
            endVertex.AddInboundArc(newArc);
            return newArc;
        }

        private void AddInboundArc(Arc newArc)
        {
            _inboundArcs.AddLast(newArc);
        }

        public void RemoveArc(Vertex vertex)
        {
            if (!ArcHelper.DoesArcExist(this, vertex, _outboundArcs))
                throw new NoArcException();
            vertex.RemoveInboundArc(this);
            ArcHelper.DeleteArc(this, vertex, _outboundArcs);
        }

        private void RemoveInboundArc(Vertex startVertex)
        {
            if (!ArcHelper.DoesArcExist(startVertex, this, _inboundArcs))
                throw new NoArcException();
            ArcHelper.DeleteArc(startVertex, this, _inboundArcs);
        }

        public void RemoveEdge(Vertex toVertex)
        {
            RemoveArc(toVertex);
            toVertex.RemoveArc(this);
        }

        public void RemoveInboundArcs()
        {
            foreach (var neighbour in _inboundArcs)
            {
                neighbour.Start.EndVertexRemoved(this);
            }
            _inboundArcs = new LinkedList<Arc>();
        }

        private void EndVertexRemoved(Vertex endVertex)
        {
            if (!ArcHelper.DoesArcExist(this, endVertex, _outboundArcs))
                throw new NoArcException();
            ArcHelper.DeleteArc(this, endVertex, _outboundArcs);
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