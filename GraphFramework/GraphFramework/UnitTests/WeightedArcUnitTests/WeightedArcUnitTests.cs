using NUnit.Framework;

namespace GraphFramework.UnitTests.WeightedArcUnitTests
{
    public abstract class WeightedArcUnitTests
    {
        [TestFixture]
        public class TheConstructor4 : WeightedArcUnitTests
        {
            private WeightedArc _wa;

            [SetUp]
            public void Init()
            {
            }

            [Test]
            public void SetsWeightOnCreate()
            {
                const double weight = 1.0;
                _wa = new WeightedArc(null, null, null, weight);
                Assert.That(_wa.Weight, Is.EqualTo(weight));
            }
        }

        [TestFixture]
        public class TheConstructor5 : WeightedArcUnitTests
        {
            private WeightedArc _wa;

            [SetUp]
            public void Init()
            {
            }

            [Test]
            public void SetsWeightOnCreate()
            {
                const double weight = 1.0;
                _wa = new WeightedArc(null, null, null, true, weight);
                Assert.That(_wa.Weight, Is.EqualTo(weight));
            }
        }
    }
}