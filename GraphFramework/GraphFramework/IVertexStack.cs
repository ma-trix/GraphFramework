namespace GraphFramework
{
    public interface IVertexStack
    {
        IStackVertex Push(ABVertex vertex);
        IStackVertex Top();
        bool Contains(ABVertex vertex);
        void Pop();
    }
}