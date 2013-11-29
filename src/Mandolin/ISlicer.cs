namespace Mandolin
{
    using System.Collections.Generic;

    public interface ISlicer
    {
        IEnumerable<string> Slice(IEnumerable<string> sample, int wantedSlice, int totalSlices);
    }
}
