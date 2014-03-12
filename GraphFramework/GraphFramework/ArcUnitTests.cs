using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class ArcUnitTests
    {
        private Vertex _v1;
        private Vertex _v2;
        private Graph _g;

        [SetUp]
        public void Init()
        {
            _v1 = new Vertex();
            _v2 = new Vertex();
            _g = new Graph();
        }

        [Test]
        public void SetsStartAndEndVerticesOnCreate()
        {
            Arc a = new Arc(null, _v1, _v2);
            Assert.AreSame(_v1, a.Start);
            Assert.AreSame(_v2, a.End);
        }

        [Test]
        public void KnowsGraphItBelongsTo()
        {
            Arc a = new Arc(_g, _v1, _v2);
            Assert.AreSame(_g, a.Graph);
        }

        [Test]
        public void KnowsItIsInMatching()
        {
            Arc a = new Arc(_g, _v1, _v2);
            a.AddToMatching();
            Assert.AreEqual(true, a.IsInMatching);
        }

        [Test]
        public void CanCreateArcInMatching()
        {
            Arc a = new Arc(_g, _v1, _v2, true);
            Assert.That(a.IsInMatching, Is.True);
        }
    }
}
