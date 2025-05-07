using FluentAssertions;
using NUnit.Framework;

namespace microservice_1.Tests
{
    [TestFixture]
    public class SampleTests
    {
        [SetUp]
        public void SetUpTests()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void AlwaysTrue()
        {
            Assert.True(true);

            var sample = true;
            sample.Should().BeTrue();
        }
    }
}