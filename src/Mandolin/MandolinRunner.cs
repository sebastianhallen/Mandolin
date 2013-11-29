namespace Mandolin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Core;
    using NUnit.Core.Filters;

    public class MandolinRunner
    {
        private readonly ISlicer slicer;
        private readonly EventListener eventListener;

        public MandolinRunner(ISlicer slicer, EventListener eventListener)
        {
            this.slicer = slicer;
            this.eventListener = eventListener;
        }

        public TestResult Run(int wantedSlice, int totalSlices, params string[] testAssemblies)
        {
            CoreExtensions.Host.InitializeService();
            var runner = new SimpleTestRunner();

            var package = new TestPackage("tests", testAssemblies);

            if (runner.Load(package))
            {
                var allTests = GetFullTestNames(runner.Test);
                var slicedTests = this.slicer.Slice(allTests, wantedSlice, totalSlices);
                var filter = new SimpleNameFilter(slicedTests.ToArray());
                var result = runner.Run(this.eventListener, filter, true, LoggingThreshold.All);

                return result;
            }

            throw new Exception("Unable to load test package");
        }

        private static IEnumerable<string> GetFullTestNames(ITest parent)
        {
            if (parent == null) yield break;
            if (parent.Tests == null)
            {
                yield return parent.TestName.FullName;
                yield break;
            }


            foreach (var childResult in parent.Tests.OfType<ITest>())
            {
                foreach (var testName in GetFullTestNames(childResult))
                {
                    yield return testName;
                }
            }
        }
    }
}