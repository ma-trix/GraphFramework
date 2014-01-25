using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class ABVertexTests
    {
        [Test]
        public void NewTypedVertexIsNotPushed()
        {
            var abv = new ABVertex(VertexType.A);
            Assert.That(abv.IsPushed, Is.False);
        }

        [Test]
        public void PushingABVertexMarksItAsPushed()
        {
            var abv = new ABVertex(VertexType.A);
            abv.Pushed();
            Assert.That(abv.IsPushed, Is.True);
        }

        [Test]
        public void NewVertexHasEmptyE()
        {
            var abv = new ABVertex(VertexType.A);
            Assert.That(abv.E, Is.Empty);
        }

        [Test]
        public void AddsToE()
        {
            var abv = new ABVertex(VertexType.A);
            var v1 = new ABVertex(VertexType.B);
            var arc = new Arc(null, v1, abv);
            abv.AddToE(arc);
            Assert.That(ArcHelper.DoesArcExist(v1, abv, abv.E));
        }

        [Test]
        public void NewVertexHasEmptyR()
        {
            var abv = new ABVertex(VertexType.A);
            Assert.That(abv.R, Is.Empty);
        }

        [Test]
        public void AddsToR()
        {
            var abv = new ABVertex(VertexType.A);
            var v1 = new ABVertex(VertexType.B);
            var arc = new Arc(null, v1, abv);
            abv.AddToR(arc);
            Assert.That(ArcHelper.DoesArcExist(v1, abv, abv.R));
        }
    }
}