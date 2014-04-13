namespace GraphFramework
{
    public interface IVertexStack
    {
        IStackableVertex Push(ABVertex vertex, Arc arcFromAncestor);
        IStackableVertex Top();
        bool Contains(ABVertex vertex);
        void Pop();
    }
}