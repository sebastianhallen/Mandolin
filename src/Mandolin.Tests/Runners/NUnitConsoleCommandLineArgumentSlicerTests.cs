namespace Mandolin.Tests.Runners
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using FakeItEasy;
    using Mandolin.Runners;
    using NUnit.ConsoleRunner;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class NUnitConsoleCommandLineArgumentSlicerTests
    {
        [UnderTest] private NUnitConsoleCommandLineArgumentSlicer argumentSlicer;
        [Fake] private ISlicer slicer;
	    [Fake] private IRunListFilter preFilter;
        [Fake] private ITestSuiteExtractor suiteExtractor;
        [Fake] private IRunListBuilder runListBuilder;

        [SetUp]
        public void Before()
        {
            Fake.InitializeFixture(this);
        }

        [Test]
        public void Should_replace_existing_run_argument_with_runlist_version()
        {
            var args = "test.dll /run:1,2,3,4".Split(' ');
            var runlist = A.Fake<IRunListFile>();
            A.CallTo(() => this.slicer.Slice(A<IEnumerable<string>>._, 1, 2)).Returns(new[] {"1", "3"});
            A.CallTo(() => this.runListBuilder.CreateRunList(A<IEnumerable<string>>._)).Returns(runlist);
            A.CallTo(() => runlist.Path).Returns("some-runlist");
            string[] slicedArguments;

            this.argumentSlicer.Slice(args, 1, 2, out slicedArguments);

            Assert.That(slicedArguments, Is.EquivalentTo(new [] { "/runlist:some-runlist", "test.dll"}));
        }

        [Test]
        public void Should_use_matching_tests_from_suite_extractor_when_filtering()
        {
            var matchingTests = A.Fake<IEnumerable<string>>();
            A.CallTo(() => this.suiteExtractor.FindMatchingTests(A<ConsoleOptions>._, A<string[]>._)).Returns(matchingTests);

            string[] _;
            this.argumentSlicer.Slice(new [] {"some.dll"}, 10, 20, out _);

            A.CallTo(() => this.preFilter.Filter(matchingTests)).MustHaveHappened();
        }

        [TestCase("/xml:C:\\somexml=output:with-a-rouge-colon=", "/xml:C:\\somexml=output:with-a-rouge-colon=")]
        [TestCase("/xml=C:\\somexml=output:with-a-rouge-colon=", "/xml:C:\\somexml=output:with-a-rouge-colon=")]
        [TestCase("/xml:somexml", "/xml:somexml")]
        [TestCase("/xml=somexml", "/xml:somexml")]
        public void Should_be_able_to_re_assemble_argument_with_mixed_equals_and_colon_chars(string arg, string expectedArg)
        {
            var args = ("test.dll " + arg).Split(' ');
            string[] processedArgs;

            A.CallTo(() => this.slicer.Slice(A<IEnumerable<string>>._, 1, 1)).Returns(new[] { "1", "3" });

            this.argumentSlicer.Slice(args, 1, 1, out processedArgs);

            Assert.That(processedArgs, Is.EquivalentTo(new[] { expectedArg, "/runlist:", "test.dll" }));
        }

        [Test]
        public void Should_set_run_argument_to_a_non_matching_place_holder_test_when_no_matching_tests_can_be_found()
        {
            var args = "test.dll /xml:somexml=output:with-a-rouge-colon=".Split(' ');
            string[] processedArgs;

            this.argumentSlicer.Slice(args, 1, 1, out processedArgs);

            Assert.That(processedArgs, Is.EquivalentTo(new[] { "/xml:somexml=output:with-a-rouge-colon=", "/run:NoMatchingTestsInSlice", "test.dll" }));
        }

        [Test]
        public void Should_use_run_list_builder_to_construct_run_list()
        {
            var args = new [] { "test.dll" };
            string[] _;
            A.CallTo(() => this.slicer.Slice(A<IEnumerable<string>>._, 1, 1)).Returns(new[] { "test1", "test2" });

            this.argumentSlicer.Slice(args, 1, 1, out _);

            A.CallTo(() => this.runListBuilder.CreateRunList(A<IEnumerable<string>>
                .That.IsSameSequenceAs(new [] { "test1", "test2"} ))).MustHaveHappened();
        }

		[Test]
		public void Should_not_explode_when_given_two_assemblies_on_the_same_drive()
		{
			var assemblies = new[]
				{
					"Mandolin.Console.exe",
					"Mandolin.dll"
				}.Select(asm => Path.Combine(AssemblyDirectory, asm)).ToArray();
			string[] slicedArgs;

			this.argumentSlicer.Slice(assemblies, 1, 1, out slicedArgs);

			Assert.That(slicedArgs, Is.EquivalentTo(assemblies.Concat(new [] { "/run:NoMatchingTestsInSlice" })));
		}

	    [Test]
	    public void Should_pre_filter_run_list()
	    {
		    var assemblies = new[] {"foo.dll"};
			string[] _;
		    
			this.argumentSlicer.Slice(assemblies, 1, 1, out _);

			A.CallTo(() => this.preFilter.Filter(A<IEnumerable<string>>._)).MustHaveHappened();
	    }

	    [Test]
	    public void Should_use_result_from_runlist_filter_when_slicing()
		{
			var filtered = A.Fake<IEnumerable<string>>();
			var assemblies = new[] { "foo.dll" };
		    A.CallTo(() => this.preFilter.Filter(A<IEnumerable<string>>._)).Returns(filtered);

			string[] _;
			this.argumentSlicer.Slice(assemblies, 1, 1, out _);

		    A.CallTo(() => this.slicer.Slice(filtered, 1, 1)).MustHaveHappened();
	    }

	    public static string AssemblyDirectory
		{
			get
			{
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}
    }
}
