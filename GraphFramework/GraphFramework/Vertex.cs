using System.Collections.Generic;

namespace GraphFramework
{
    public class Vertex
    {
        public LinkedList<Vertex> Outbound;
        public LinkedList<Vertex> Inbound;

        public Vertex()
        {
            Outbound = new LinkedList<Vertex>();
            Inbound = new LinkedList<Vertex>();
        }

        public int OutDegree
        {
            get { return Outbound.Count; }
        }

        public void AddEdge(Vertex newNeighbour)
        {
            AddArc(newNeighbour);
            newNeighbour.AddArc(this);
        }
        
        public void AddArc(Vertex vertex)
        {
            if (Outbound.Contains(vertex))
                throw new NoMultiedgePermitedException();
            Outbound.AddLast(vertex);
            vertex.AddInboundArc(this);
        }

        private void AddInboundArc(Vertex fromVertex)
        {
            Inbound.AddLast(fromVertex);
        }

        public void RemoveArc(Vertex vertex)
        {
            if (!Outbound.Contains(vertex))
                throw new NoArcException();
            vertex.RemoveInboundArc(this);
            Outbound.Remove(vertex);
        }

        private void RemoveInboundArc(Vertex fromVertex)
        {
            if (!Inbound.Contains(fromVertex))
                throw new NoArcException();
            Inbound.Remove(fromVertex);
        }

        public void RemoveEdge(Vertex toVertex)
        {
            RemoveArc(toVertex);
            toVertex.RemoveArc(this);
        }

        public void RemoveInboundArcs()
        {
            foreach (var vertex in Inbound)
            {
                vertex.EndVertexRemoved(this);
            }
            Inbound = new LinkedList<Vertex>();
        }

        private void EndVertexRemoved(Vertex endVertex)
        {
            if (!Outbound.Contains(endVertex))
                throw new NoArcException();
            Outbound.Remove(endVertex);
        }

        public void RemoveOutboundArcs()
        {
            foreach (var neighbour in Outbound)
            {
                neighbour.RemoveInboundArc(this);
            }
        }
    }
}