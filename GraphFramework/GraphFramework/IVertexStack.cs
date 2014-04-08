namespace GraphFramework
{
    public interface IVertexStack
    {
        IStackableVertex Push(ABVertex vertex);
        IStackableVertex Top();
        bool Contains(ABVertex vertex);
        void Pop();
    }
}