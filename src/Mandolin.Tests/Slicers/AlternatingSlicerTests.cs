namespace Mandolin.Tests.Slicers
{
    using Mandolin.Slicers;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class AlternatingSlicerTests
    {
        private AlternatingSlicer slicer;

        [SetUp]
        public void Before()
        {
            this.slicer = new AlternatingSlicer();
        }

        [TestCase(new[] { "A", "B", "C" }, 1, 3, new[] { "A" })]
        [TestCase(new[] { "A", "B", "C" }, 2, 3, new[] { "B" })]
        [TestCase(new[] { "A", "B", "C" }, 3, 3, new[] { "C" })]
        [TestCase(new[] { "A", "B", "C" }, 1, 2, new[] { "A", "C" })]
        [TestCase(new[] { "A", "B", "C" }, 4, 4, new string[0])]
        public void Should_be_able_to_slice_alternating(IEnumerable<string> sample, int wantedSlice, int totalSlices, IEnumerable<string> expected)
        {
            var slice = this.slicer.Slice(sample, wantedSlice, totalSlices);

            Assert.That(slice, Is.EquivalentTo(expected));
        }
    }
}
