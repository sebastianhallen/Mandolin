namespace Mandolin.Slicers.Scorer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class NUnitResultXmlScorer
        : IPartitionScorer
    {
        private readonly INUnitResultsRepository resultsRepository;
        
        public NUnitResultXmlScorer(INUnitResultsRepository resultsRepository)
        {
            this.resultsRepository = resultsRepository;
        }

        public long Score(string testMethod)
        {
            var results = this.resultsRepository.All().ToArray();

            var matchingResults = results.Where(result => testMethod.Equals(result.TestName)).ToArray();

            var scoringResults = matchingResults.Any() 
                ? matchingResults 
                : results.Any() ? results : new [] { new NUnitResult(TimeSpan.FromSeconds(0), "place holder test case name") };

            return scoringResults.Sum(result => result.Duration.Ticks) / (scoringResults.Count() * 10000);
        }
    }
}