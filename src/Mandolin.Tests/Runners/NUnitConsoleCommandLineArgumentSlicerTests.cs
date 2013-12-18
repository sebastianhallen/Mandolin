namespace Mandolin.Tests.Runners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using FakeItEasy;
    using Mandolin.Runners;
    using NUnit.Framework;

    [TestFixture]
    public class NUnitConsoleCommandLineArgumentSlicerTests
    {
        [UnderTest] private NUnitConsoleCommandLineArgumentSlicer argumentSlicer;
        [Fake] private ISlicer slicer;
        [Fake] private ITestSuiteExtractor suiteExtractor;

        [SetUp]
        public void Before()
        {
            Fake.InitializeFixture(this);
        }

        [Test]
        public void Should_replace_existing_run_argument_with_sliced_version()
        {
            var args = "test.dll /run:1,2,3,4".Split(' ');
            A.CallTo(() => this.slicer.Slice(A<IEnumerable<string>>._, 1, 2)).Returns(new[] {"1", "3"});

            var slicedArguments = this.argumentSlicer.Slice(args, 1, 2);

            Assert.That(slicedArguments, Is.EquivalentTo(new [] { "/run:1,3", "test.dll"}));
        }

        [TestCase("/xml:C:\\somexml=output:with-a-rouge-colon=", "/xml:C:\\somexml=output:with-a-rouge-colon=")]
        [TestCase("/xml=C:\\somexml=output:with-a-rouge-colon=", "/xml:C:\\somexml=output:with-a-rouge-colon=")]
        [TestCase("/xml:somexml", "/xml:somexml")]
        [TestCase("/xml=somexml", "/xml:somexml")]
        public void Should_be_able_to_re_assemble_argument_with_mixed_equals_and_colon_chars(string arg, string expectedArg)
        {
            var args = ("test.dll " + arg).Split(' ');
            A.CallTo(() => this.slicer.Slice(A<IEnumerable<string>>._, 1, 1)).Returns(new[] { "1", "3" });

            var processedArgs = this.argumentSlicer.Slice(args, 1, 1);

            Assert.That(processedArgs, Is.EquivalentTo(new[] { expectedArg, "/run:1,3", "test.dll" }));
        }

        [Test]
        public void Should_set_run_argument_to_a_non_matching_place_holder_test_when_no_matching_tests_can_be_found()
        {
            var args = "test.dll /xml:somexml=output:with-a-rouge-colon=".Split(' ');

            var processedArgs = this.argumentSlicer.Slice(args, 1, 1);

            Assert.That(processedArgs, Is.EquivalentTo(new[] { "/xml:somexml=output:with-a-rouge-colon=", "/run:NoMatchingTestsInSlice", "test.dll" }));
        }
    }
}
