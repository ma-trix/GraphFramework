namespace GraphFramework
{
    public interface VertexStack
    {
        void Push(Vertex vertex);
        ABVertex Top();
        bool Contains(Vertex vertex);
        void Pop();
    }
}