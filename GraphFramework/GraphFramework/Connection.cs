namespace GraphFramework
{
    public class Connection
    {
        public Arc Arc;
        public IStackableVertex Start;
        public IStackableVertex End;

        public Connection(Arc arc, IStackableVertex start, IStackableVertex end)
        {
            Arc = arc;
            Start = start;
            End = end;
        }
    }
}