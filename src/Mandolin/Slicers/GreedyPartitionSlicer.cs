namespace Mandolin.Slicers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mandolin.Slicers.Scorer;

    public class GreedyPartitionSlicer
        : ISlicer
    {
        private readonly IPartitionScorer scorer;

        public GreedyPartitionSlicer(IPartitionScorer scorer)
        {
            this.scorer = scorer;
        }

        public IEnumerable<string> Slice(IEnumerable<string> sample, int wantedSlice, int totalSlices)
        {
            if (totalSlices == 1) return sample;
            var scoredSample = from testMethod in sample
                               let scoredEntry = Tuple.Create(testMethod, this.scorer.Score(testMethod))
                               orderby scoredEntry.Item2 descending 
                               select scoredEntry;
            
            var partitionTable = new List<List<Tuple<string, long>>>(
                    Enumerable.Range(0, totalSlices)
                                .Select(_ => new List<Tuple<string, long>>())
                );
            foreach (var entry in scoredSample)
            {
                var minSlice = (from slice in partitionTable
                                orderby slice.Sum(scored => scored.Item2) ascending 
                                select slice).First();
                minSlice.Add(entry);
            }

            return partitionTable[wantedSlice - 1].Select(scored => scored.Item1);
        }

    }
}