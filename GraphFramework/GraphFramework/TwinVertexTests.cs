using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class TwinVertexTests
    {
        [Test]
        public void KnowsItsPrecursor()
        {
            Vertex v = new Vertex();
            TwinVertex tv = new TwinVertex(v, null);
            Assert.AreSame(v, tv.Precursor);
        }

        [Test]
        public void KnowsItsTwinGraph()
        {
            TwinGraph tg = new TwinGraph();
            TwinVertex tv = new TwinVertex(null, tg);
            Assert.AreSame(tg, tv.Graph);
        }
    }
}