namespace Mandolin.Slicers.Scorer
{
    using System;

    public interface INUnitResult
    {
        TimeSpan Duration { get; }
        string TestName { get; }
    }
}