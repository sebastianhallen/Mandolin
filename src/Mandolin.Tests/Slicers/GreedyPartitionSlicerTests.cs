namespace Mandolin.Tests.Slicers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FakeItEasy;
    using Mandolin.Slicers;
    using Mandolin.Slicers.Scorer;
    using NUnit.Framework;

    [TestFixture]
    public class GreedyPartitionSlicerTests
    {
        [UnderTest] private GreedyPartitionSlicer slicer;
        [Fake] private IPartitionScorer scorer;

        [SetUp]
        public void Before()
        {
            Fake.InitializeFixture(this);
        }

        [Test]
        public void Should_use_scorer_to_get_value_to_use_in_partitioning()
        {
            this.slicer.Slice(new [] {"A", "B"}, 1, 2);

            A.CallTo(() => this.scorer.Score("A")).MustHaveHappened();
            A.CallTo(() => this.scorer.Score("B")).MustHaveHappened();
        }

        [Test]
        public void Should_not_use_scorer_when_total_slices_is_one()
        {
            this.slicer.Slice(new[] { "A", "B" }, 1, 1);

            A.CallTo(() => this.scorer.Score("A")).MustNotHaveHappened();
            A.CallTo(() => this.scorer.Score("B")).MustNotHaveHappened();
        }

        [TestCase(new[] { "A", "B", "C" }, new[] { 1, 2, 3 }, 1, 2, new[] {"C"})]
        [TestCase(new[] { "A", "B", "C" }, new[] { 1, 2, 3 }, 2, 2, new[] {"A", "B"})]
        public void Should_be_able_to_partition_sample_greedily_using_score(string[] sample, int[] score, int wantedSlice, int totalSlices, string[] expectedSlice)
        {
            A.CallTo(() => this.scorer.Score(A<string>._)).ReturnsNextFromSequence(score.Select(i => (long)i).ToArray());

            var slice = this.slicer.Slice(sample, wantedSlice, totalSlices);

            Assert.That(slice, Is.EquivalentTo(expectedSlice));
        }
    }
}
