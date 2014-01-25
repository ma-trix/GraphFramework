using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class VertexStackUnitTests
    {
        [Test]
        public void TopElementIsNullForEmptyVertexStack()
        {
            var k = new VertexStack();
            Assert.That(k.Top(), Is.Null);
        }

        [Test]
        public void PushingElementSetsItToTop()
        {
            var k = new VertexStack();
            var v = new ABVertex(VertexType.A);
            k.Push(v);
            Assert.That(k.Top(), Is.SameAs(v));
        }

        [Test]
        public void PushReturnsWrappedPushedElement()
        {
            var k = new VertexStack();
            var v = new ABVertex(VertexType.A);
            var pushed = k.Push(v);
            Assert.That(pushed.Value, Is.SameAs(v));
            Assert.That(pushed.Ancestor, Is.Null);
        }

        [Test]
        public void PushingAnotherElementMakesItAncestorToCurrentTop()
        {
            var k = new VertexStack();
            var v = new ABVertex(VertexType.A);
            k.Push(v);
            var v2 = new ABVertex(VertexType.B);
            var pushed = k.Push(v2);
            Assert.That(pushed.Ancestor, Is.SameAs(v));
        }
    }
}