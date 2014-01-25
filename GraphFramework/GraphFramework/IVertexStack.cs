namespace GraphFramework
{
    public interface IVertexStack
    {
        StackVertex Push(ABVertex vertex);
        ABVertex Top();
        bool Contains(Vertex vertex);
        void Pop();
    }
}