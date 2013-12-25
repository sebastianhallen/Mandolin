namespace Mandolin.Tests.Slicers.Scorer
{
    using Mandolin.Slicers.Scorer;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class NUnitResultXmlScorerTests
    {
        private ResultsBuilder builder;

        [SetUp]
        public void Before()
        {
            this.builder = new ResultsBuilder();
        }
    
        [Test]
        public void Should_return_mean_value_of_total_milliseconds_from_matching_durations_as_score()
        {
            var results = this.builder
                              .Add("A", 3)
                              .Add("A", 8)
                              .Add("A", 1)
                              .Build();
            var scorer = new NUnitResultXmlScorer(results);

            var score = scorer.Score("A");

            Assert.That(score, Is.EqualTo(1000 * (3 + 8 + 1) / 3));
        }

        [Test]
        public void Should_return_average_of_all_total_milliseconds_when_no_matching_tests_exists()
        {
            var results = this.builder
                              .Add("A", 3)
                              .Add("B", 8)
                              .Add("C", 1)
                              .Build();
            var scorer = new NUnitResultXmlScorer(results);

            var score = scorer.Score("D");

            Assert.That(score, Is.EqualTo(1000 * (3 + 8 + 1) / 3));
        }

        [Test]
        public void Should_return_0_when_no_results_are_completly_empty()
        {
            var results = this.builder.Build();
            var scorer = new NUnitResultXmlScorer(results);

            var score = scorer.Score("A");

            Assert.That(score, Is.EqualTo(0));
        }

        private class ResultsBuilder
        {
            private List<INUnitResult> results;

            public ResultsBuilder()
            {
                this.results = new List<INUnitResult>();
            }

            public ResultsBuilder Add(string name, int score)
            {
                this.results.Add(new NUnitResult(TimeSpan.FromSeconds(score), name));
                return this;
            }

            public INUnitResultsRepository Build()
            {
                var resultsToReturn = this.results.ToArray();

                this.results.Clear();

                return new PassThroughNUnitResultsRepository(resultsToReturn);
            }
        }

        public class PassThroughNUnitResultsRepository
            : INUnitResultsRepository
        {
            private readonly IEnumerable<INUnitResult> results;

            public PassThroughNUnitResultsRepository(IEnumerable<INUnitResult> results)
            {
                this.results = results;
            }

            public IEnumerable<INUnitResult> All()
            {
                return this.results;
            }
        }
    }
}
