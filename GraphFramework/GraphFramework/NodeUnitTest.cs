using NUnit.Framework;

namespace GraphFramework
{
    public class NodeUnitTest
    {
        [Test]
        public void NewNodeHasNoNeighbours()
        {
            Node node = new Node();
            Assert.AreEqual(0, node.Neighbours.Count);
        }
    }
}