namespace GraphFramework
{
    public interface IVertexStack
    {
        StackVertex Push(ABVertex vertex);
        StackVertex Top();
        bool Contains(ABVertex vertex);
        void Pop();
    }
}