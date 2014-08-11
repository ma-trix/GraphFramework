using NUnit.Framework;

namespace GraphFramework.UnitTests.ABVertexStackUnitTests
{
    public abstract class ABVertexStackUnitTests
    {
        private ABVertexStack _k;
        private ABVertex _v1;
        private ABVertex _v2;
        private TwinGraph _tg;

        [SetUp]
        public void Init()
        {
            _tg = new TwinGraph();
            _k = new ABVertexStack();
            _v1 = new ABVertex(VertexType.A, _tg);
            _v2 = new ABVertex(VertexType.B, _tg);
        }

        [TestFixture]
        public class TheConstructor : ABVertexStackUnitTests
        {
            [Test]
            public void TopElementIsNullForEmptyVertexStack()
            {
                Assert.That(_k.Top(), Is.Null);
            }
        }

        [TestFixture]
        public class ThePushMethod : ABVertexStackUnitTests
        {
            [Test]
            public void PushingElementSetsItToTop()
            {
                _k.Push(_v1, null);
                Assert.That(_k.Top().Value, Is.SameAs(_v1));
            }

            [Test]
            public void PushReturnsWrappedPushedElement()
            {
                var pushed = _k.Push(_v1, null);
                Assert.That(pushed.Value, Is.SameAs(_v1));
            }

            [Test]
            public void PushingAnotherElementMakesCurrentTopItsAncestor()
            {
                var pushedV1 = _k.Push(_v1, null);
                var pushedV2 = _k.Push(_v2, null);
                Assert.That(pushedV2.Ancestor, Is.SameAs(pushedV1));
            }

            [Test]
            public void PushingAnotherElementMakesItDescendantOfCurrentTop()
            {
                var pushedV1 = _k.Push(_v1, null);
                _k.Push(_v2, null);
                Assert.That(VertexHelper.DoesABVertexExist(_v2.Guid, pushedV1.Descendants), Is.True);
            }

            [Test]
            public void PushingElementAddsItToTheEndOfCurrentStack()
            {
                _k.Push(_v1, null);
                Assert.That(_k.CurrentStack.Last.Value, Is.EqualTo(_v1));
            }
        }

        [TestFixture]
        public class ThePopMethod : ABVertexStackUnitTests
        {
            [Test]
            public void PoppingMovesTopToAncestor()
            {
                _k.Push(_v1, null);
                _k.Push(_v2, null);
                _k.Pop();
                Assert.That(_k.Top().Value, Is.SameAs(_v1));
            }

            [Test]
            public void PoppingElementRemovesItFromTheEndOfCurrentStack()
            {
                _k.Push(_v1, null);
                _k.Push(_v2, null);
                Assert.That(_k.CurrentStack.Last.Value, Is.EqualTo(_v2));
                _k.Pop();
                Assert.That(_k.CurrentStack.Last.Value, Is.EqualTo(_v1));
            }     
        }
    }
}