using System.Collections.Generic;

namespace GraphFramework
{
    public class Graph
    {
        public LinkedList<Vertex> vertices = new LinkedList<Vertex>();
        public LinkedList<Arc> arcs = new LinkedList<Arc>();

        public void AddVertex(Vertex vertex)
        {
            vertices.AddLast(vertex);
            vertex.Graph = this;
        }

        public void RemoveVertex(Vertex vertexToRemove)
        {
            if (!vertices.Contains(vertexToRemove))
            {
                throw new NoVertexException();
            }
            vertexToRemove.RemoveInboundArcs();
            vertexToRemove.RemoveOutboundArcs();
            vertexToRemove.Graph = null;
            vertices.Remove(vertexToRemove);
        }

        public void AddArc(Vertex startVertex, Vertex endVertex)
        {
            Arc a = startVertex.AddOutboundArc(endVertex);
            arcs.AddLast(a);
        }

        public bool DoesArcExist(Vertex start, Vertex end, LinkedList<Arc> inList)
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

        public void RemoveArc(Vertex v1, Vertex v2)
        {
            if (DeleteArc(v1, v2, arcs))
            {
                v1.RemoveArc(v2);
            }
        }

        private bool DeleteArc(Vertex start, Vertex end, LinkedList<Arc> fromList)
        {
            var arc = fromList.First;
            while (arc != null)
            {
                var nextArc = arc.Next;
                if (arc.Value.Start == start && arc.Value.End == end)
                {
                    fromList.Remove(arc);
                    return true;
                }
                arc = nextArc;
            }
            return false;
        }

        public void AddEdge(Vertex startVertex, Vertex endVertex)
        {
            AddArc(startVertex, endVertex);
            AddArc(endVertex, startVertex);
        }

        public void RemoveEdge(Vertex v1, Vertex v2)
        {
            RemoveArc(v1, v2);
            RemoveArc(v2, v1);
        }
    }
}
