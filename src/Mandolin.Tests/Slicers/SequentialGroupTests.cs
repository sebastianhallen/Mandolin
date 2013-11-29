namespace Mandolin.Tests.Slicers
{
    using System.Collections.Generic;
    using Mandolin.Slicers;
    using NUnit.Framework;

    [TestFixture]
    public class SequentialGroupTests
    {
        private SequentialGroupsSlicer slicer;

        [SetUp]
        public void Before()
        {
            this.slicer = new SequentialGroupsSlicer();
        }

        [TestCase(new[] { "A", "B", "C" }, 1, 3, new[] { "A" }, Description = "single first")]
        [TestCase(new[] { "A", "B", "C" }, 2, 3, new[] { "B" }, Description = "single middle"),]
        [TestCase(new[] { "A", "B", "C" }, 3, 3, new[] { "C" }, Description = "single last"),]
        [TestCase(new[] { "A", "B", "C" }, 1, 2, new[] { "A", "B" }, Description = "first with two")]
        [TestCase(new[] { "A", "B", "C" }, 1, 2, new[] { "A", "B" }, Description = "second with two")]
        [TestCase(new[] { "A", "B", "C", "D", "E" }, 1, 3, new[] { "A", "B" }, Description = "first of many")]
        [TestCase(new[] { "A", "B", "C", "D", "E" }, 2, 3, new[] { "C", "D" }, Description = "middle of many")]
        [TestCase(new[] { "A", "B", "C", "D", "E" }, 3, 3, new[] { "E" }, Description = "last of many")]
        [TestCase(new[] { "A", "B", "C" }, 4, 4, new string[0], Description = "empty")]
        public void Should_be_able_to_slice_sequential_groups(IEnumerable<string> sample, int wantedSlice, int totalSlices, IEnumerable<string> expected)
        {
            var slice = this.slicer.Slice(sample, wantedSlice, totalSlices);

            Assert.That(slice, Is.EquivalentTo(expected));
        }
    }
}