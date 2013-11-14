using System.Collections.Generic;

namespace GraphFramework
{
    public class Vertex
    {
        public LinkedList<Vertex> Neighbours;

        public Vertex()
        {
            Neighbours = new LinkedList<Vertex>();
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
        }

        public void RemoveArc(Vertex vertex)
        {
            if (!Neighbours.Contains(vertex))
                throw new NoArcException();
            Neighbours.Remove(vertex);
        }

        public void RemoveEdge(Vertex toVertex)
        {
            RemoveArc(toVertex);
            toVertex.RemoveArc(this);
        }
    }
}