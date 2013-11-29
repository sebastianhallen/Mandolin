namespace Mandolin.Slicers
{
    using System.Collections.Generic;
    using System.Linq;

    public class AlternatingSlicer
        : ISlicer
    {
        public IEnumerable<string> Slice(IEnumerable<string> sample, int wantedSlice, int totalSlices)
        {
            return sample
                .Select((entry, i) => new
                    {
                        Entry = entry,
                        SliceGroup = (i%totalSlices) + 1
                    })
                .Where(slice => slice.SliceGroup == wantedSlice)
                .Select(slice => slice.Entry);
        }
    }
}