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
    }
}