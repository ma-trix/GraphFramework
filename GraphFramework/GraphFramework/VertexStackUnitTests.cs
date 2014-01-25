using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class VertexStackUnitTests
    {
        private VertexStack k;
        private ABVertex v1;
        private ABVertex v2;

        [SetUp]
        public void SetUp()
        {
            k = new VertexStack();
            v1 = new ABVertex(VertexType.A);
            v2 = new ABVertex(VertexType.B);
        }

        [Test]
        public void TopElementIsNullForEmptyVertexStack()
        {
            Assert.That(k.Top(), Is.Null);
        }

        [Test]
        public void PushingElementSetsItToTop()
        {
            k.Push(v1);
            Assert.That(k.Top(), Is.SameAs(v1));
        }

        [Test]
        public void PushReturnsWrappedPushedElement()
        {
            var pushed = k.Push(v1);
            Assert.That(pushed.Value, Is.SameAs(v1));
            Assert.That(pushed.Ancestor, Is.Null);
        }

        [Test]
        public void PushingAnotherElementMakesCurrentTopItsAncestor()
        {
            k.Push(v1);
            var pushedV2 = k.Push(v2);
            Assert.That(pushedV2.Ancestor, Is.SameAs(v1));
        }

        [Test]
        public void PushingAnotherElementMakesItDescendantOfCurrentTop()
        {
            var pushedV1 = k.Push(v1);
            k.Push(v2);
            Assert.That(VertexHelper.DoesABVertexExist(v2.Guid, pushedV1.Descendants), Is.True);
        }
    }
}