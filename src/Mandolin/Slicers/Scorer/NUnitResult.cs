namespace Mandolin.Slicers.Scorer
{
    using System;

    public class NUnitResult
        : INUnitResult
    {
        public TimeSpan Duration { get; private set; }
        public string TestName { get; private set; }

        public NUnitResult(TimeSpan duration, string testName)
        {
            this.Duration = duration;
            this.TestName = testName;
        }
    }
}