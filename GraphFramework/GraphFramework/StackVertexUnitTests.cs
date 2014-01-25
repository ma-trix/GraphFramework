using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class StackVertexUnitTests
    {
        private ABVertex _value;
        private ABVertex _ancestor;

        [SetUp]
        public void SetUp()
        {
            _value = new ABVertex(VertexType.A);
            _ancestor = new ABVertex(VertexType.B);
        }

        [Test]
        public void NewStackVertexHasNoDescendants()
        {
            var sv = new StackVertex(null, null);
            Assert.That(sv.Descendants, Is.Not.Empty);
        }

        [Test]
        public void NewStackVertexHasValueAssigned()
        {
            var sv = new StackVertex(_value, null);
            Assert.That(sv.Value, Is.SameAs(_value));
        }
        
        [Test]
        public void NewStackVertexHasAncestorAssigned()
        {
            var sv = new StackVertex(null, _ancestor);
            Assert.That(sv.Ancestor, Is.SameAs(_ancestor));
        }

        [Test]
        public void AddsDescendant()
        {
            var sv = new StackVertex(null, null);
            sv.AddDescendant(_value);
            Assert.That(VertexHelper.DoesABVertexExist(_value.Guid, sv.Descendants));
        }
    }
}