namespace GraphFramework
{
    public class TwinVertex
    {
        public TwinVertex(Vertex precursor, TwinGraph tg)
        {
            Precursor = precursor;
            Graph = tg;
        }

        public Vertex Precursor { get; set; }

        public TwinGraph Graph { get; set; }
    }
}
