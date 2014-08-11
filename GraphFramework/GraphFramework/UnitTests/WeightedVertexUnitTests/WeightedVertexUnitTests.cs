using NUnit.Framework;

namespace GraphFramework.UnitTests.WeightedVertexUnitTests
{
    public abstract class WeightedVertexUnitTests
    {
        [TestFixture]
        public class TheConstructor : WeightedVertexUnitTests
        {
            private WeightedVertex _wv;
            private const double DualWeight = 1.0;

            [Test]
            public void SetsDualWeightOnCreate()
            {
                _wv = new WeightedVertex(DualWeight);
                Assert.That(_wv.DualWeight, Is.EqualTo(DualWeight));
            }
        }
    }

    public class WeightedVertex : Vertex
    {
        public WeightedVertex(double dualWeight)
        {
            DualWeight = dualWeight;
        }

        public double DualWeight { get; private set; }
    }
}