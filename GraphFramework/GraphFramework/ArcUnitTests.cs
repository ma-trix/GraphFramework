using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class ArcUnitTests
    {
        private Vertex v1;
        private Vertex v2;
        private Graph g;

        [SetUp]
        public void Init()
        {
            v1 = new Vertex();
            v2 = new Vertex();
            g = new Graph();
        }

        [Test]
        public void SetsStartAndEndVerticesOnCreate()
        {
            Arc a = new Arc(null, v1, v2);
            Assert.AreSame(v1, a.Start);
            Assert.AreSame(v2, a.End);
        }

        [Test]
        public void KnowsGraphItBelongsTo()
        {
            Arc a = new Arc(g, v1, v2);
            Assert.AreSame(g, a.Graph);
        }
    }
}
