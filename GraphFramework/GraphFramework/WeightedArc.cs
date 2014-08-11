namespace GraphFramework
{
    public class WeightedArc : Arc
    {
        public WeightedArc(Graph graph, IVertex start, IVertex end, double weight) : base(graph, start, end)
        {
            _weight = weight;
        }

        public WeightedArc(Graph graph, IVertex start, IVertex end, bool inMatching, double weight) : base(graph, start, end, inMatching)
        {
            _weight = weight;
        }

        private double _weight;

        public double Weight
        {
            get { return _weight; }
        }
    }
}