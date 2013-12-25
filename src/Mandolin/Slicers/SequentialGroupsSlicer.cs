namespace Mandolin.Slicers
{
    using System.Collections.Generic;
    using System.Linq;

    public class SequentialGroupsSlicer
        : ISlicer
    {

        public IEnumerable<string> Slice(IEnumerable<string> sample, int wantedSlice, int totalSlices)
        {
            if (totalSlices == 1) return sample;

            var items = sample.ToArray();
            var itemsInEachSlice = (items.Length / totalSlices) + (items.Length % totalSlices > 0 ? 1 : 0);
            var itemsToSkip = itemsInEachSlice * (wantedSlice - 1);
            return items.Skip(itemsToSkip).Take(itemsInEachSlice);
        }
    }
}