using System.Collections.Generic;

namespace GraphFramework
{
    public class Vertex
    {
        public LinkedList<Vertex> Neighbours;
        public LinkedList<Vertex> Inbound;

        public Vertex()
        {
            Neighbours = new LinkedList<Vertex>();
            Inbound = new LinkedList<Vertex>();
        }

        public int OutDegree
        {
            get { return Neighbours.Count; }
        }

        public void AddEdge(Vertex newNeighbour)
        {
            AddArc(newNeighbour);
            newNeighbour.AddArc(this);
        }
        
        public void AddArc(Vertex vertex)
        {
            if (Neighbours.Contains(vertex))
                throw new NoMultiedgePermitedException();
            Neighbours.AddLast(vertex);
            vertex.AddInboundArc(this);
        }

        private void AddInboundArc(Vertex fromVertex)
        {
            Inbound.AddLast(fromVertex);
        }

        public void RemoveArc(Vertex vertex)
        {
            if (!Neighbours.Contains(vertex))
                throw new NoArcException();
            vertex.RemoveInboundArc(this);
            Neighbours.Remove(vertex);
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
    }
}