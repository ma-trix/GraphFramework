using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class ABVertexStackUnitTests
    {
        private ABVertexStack k;
        private ABVertex v1;
        private ABVertex v2;

        [SetUp]
        public void SetUp()
        {
            k = new ABVertexStack();
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
            Assert.That(k.Top().Value, Is.SameAs(v1));
        }

        [Test]
        public void PushReturnsWrappedPushedElement()
        {
            var pushed = k.Push(v1);
            Assert.That(pushed.Value, Is.SameAs(v1));
        }

        [Test]
        public void PushingAnotherElementMakesCurrentTopItsAncestor()
        {
            var pushedV1 = k.Push(v1);
            var pushedV2 = k.Push(v2);
            Assert.That(pushedV2.Ancestor, Is.SameAs(pushedV1));
        }

        [Test]
        public void PushingAnotherElementMakesItDescendantOfCurrentTop()
        {
            var pushedV1 = k.Push(v1);
            k.Push(v2);
            Assert.That(VertexHelper.DoesABVertexExist(v2.Guid, pushedV1.Descendants), Is.True);
        }

        [Test]
        public void PoppingMovesTopToAncestor()
        {
            k.Push(v1);
            k.Push(v2);
            k.Pop();
            Assert.That(k.Top().Value, Is.SameAs(v1));
        }

        [Test]
        public void PushingElementAddsItToTheEndOfCurrentStack()
        {
            k.Push(v1);
            Assert.That(k.CurrentStack.Last.Value, Is.EqualTo(v1));
        }

        [Test]
        public void PoppingElementRemovesItFromTheEndOfCurrentStack()
        {
            k.Push(v1);
            k.Push(v2);
            Assert.That(k.CurrentStack.Last.Value, Is.EqualTo(v2));
            k.Pop();
            Assert.That(k.CurrentStack.Last.Value, Is.EqualTo(v1));
        }
    }
}