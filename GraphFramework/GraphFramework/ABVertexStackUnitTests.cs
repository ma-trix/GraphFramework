using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class ABVertexStackUnitTests
    {
        private ABVertexStack _k;
        private ABVertex _v1;
        private ABVertex _v2;

        [SetUp]
        public void SetUp()
        {
            _k = new ABVertexStack();
            _v1 = new ABVertex(VertexType.A);
            _v2 = new ABVertex(VertexType.B);
        }

        [Test]
        public void TopElementIsNullForEmptyVertexStack()
        {
            Assert.That(_k.Top(), Is.Null);
        }

        [Test]
        public void PushingElementSetsItToTop()
        {
            _k.Push(_v1);
            Assert.That(_k.Top().Value, Is.SameAs(_v1));
        }

        [Test]
        public void PushReturnsWrappedPushedElement()
        {
            var pushed = _k.Push(_v1);
            Assert.That(pushed.Value, Is.SameAs(_v1));
        }

        [Test]
        public void PushingAnotherElementMakesCurrentTopItsAncestor()
        {
            var pushedV1 = _k.Push(_v1);
            var pushedV2 = _k.Push(_v2);
            Assert.That(pushedV2.Ancestor, Is.SameAs(pushedV1));
        }

        [Test]
        public void PushingAnotherElementMakesItDescendantOfCurrentTop()
        {
            var pushedV1 = _k.Push(_v1);
            _k.Push(_v2);
            Assert.That(VertexHelper.DoesABVertexExist(_v2.Guid, pushedV1.Descendants), Is.True);
        }

        [Test]
        public void PoppingMovesTopToAncestor()
        {
            _k.Push(_v1);
            _k.Push(_v2);
            _k.Pop();
            Assert.That(_k.Top().Value, Is.SameAs(_v1));
        }

        [Test]
        public void PushingElementAddsItToTheEndOfCurrentStack()
        {
            _k.Push(_v1);
            Assert.That(_k.CurrentStack.Last.Value, Is.EqualTo(_v1));
        }

        [Test]
        public void PoppingElementRemovesItFromTheEndOfCurrentStack()
        {
            _k.Push(_v1);
            _k.Push(_v2);
            Assert.That(_k.CurrentStack.Last.Value, Is.EqualTo(_v2));
            _k.Pop();
            Assert.That(_k.CurrentStack.Last.Value, Is.EqualTo(_v1));
        }
    }
}