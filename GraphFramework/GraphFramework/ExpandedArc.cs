namespace GraphFramework
{
    public class ExpandedArc
    {
        public IStackableVertex Start;
        public IStackableVertex End;

        public ExpandedArc(){}
        public ExpandedArc(Arc a)
        {
            Start = (ABVertex) a.Start;
            End = (ABVertex) a.End;
        }
    }
}