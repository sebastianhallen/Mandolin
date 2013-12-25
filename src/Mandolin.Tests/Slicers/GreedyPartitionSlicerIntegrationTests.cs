namespace Mandolin.Tests.Slicers
{
    using System.IO;
    using System.Linq;
    using Mandolin.Slicers;
    using Mandolin.Slicers.Scorer;
    using NUnit.Framework;

    [TestFixture]
    public class GreedyPartitionSlicerIntegrationTests
    {
        [Test]
        public void Should_be_able_to_slice_by_using_an_existing_results_file()
        {
            var result = File.ReadAllText("ExampleTestResult.xml");
            var repository = new DirectoryScanningNUnitResultsRepository(".\\");
            var scorer = new NUnitResultXmlScorer(repository);
            var slicer = new GreedyPartitionSlicer(scorer);
            var slice = slicer.Slice(repository.All().Select(test => test.TestName), 1, 2);
        }
    }
}