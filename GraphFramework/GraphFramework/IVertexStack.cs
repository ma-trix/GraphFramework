namespace GraphFramework
{
    public interface IVertexStack
    {
        StackVertex Push(ABVertex vertex);
        ABVertex Top();
        bool Contains(ABVertex vertex);
        void Pop();
    }
}