namespace GraphFramework
{
    public class Connection
    {
        public Arc Arc;
        public IStackVertex Start;
        public IStackVertex End;

        public Connection(Arc arc, IStackVertex start, IStackVertex end)
        {
            Arc = arc;
            Start = start;
            End = end;
        }
    }
}