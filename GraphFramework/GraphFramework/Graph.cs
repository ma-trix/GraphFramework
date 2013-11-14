using System.Collections.Generic;

namespace GraphFramework
{
    public class Graph
    {
        public LinkedList<Vertex> vertices = new LinkedList<Vertex>();

        public void AddVertex(Vertex vertex)
        {
            vertices.AddLast(vertex);
        }

        public void RemoveVertex(Vertex vertexToRemove)
        {
            if (!vertices.Contains(vertexToRemove))
            {
                throw new NoVertexException();
            }
            vertices.Remove(vertexToRemove);
        }
    }
}
