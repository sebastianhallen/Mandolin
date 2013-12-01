﻿namespace Mandolin.Tests.Runners
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

        [SetUp]
        public void Before()
        {
            Fake.InitializeFixture(this);
        }

        [Test]
        public void Should_slice_run_argument()
        {
            var run = new[] {"1", "2", "3"};
            var args = string.Format(@"test.dll /run:{0}", string.Join(",", run)).Split(' ');

            this.argumentSlicer.Slice(args, 1, 2);

            A.CallTo(() => this.slicer.Slice(
                A<IEnumerable<string>>.That.Matches(arg => arg.SequenceEqual(run)), 1, 2)).MustHaveHappened();
        }

        [Test]
        public void Should_replace_existing_run_argument_with_sliced_version()
        {
            var args = "test.dll /run:1,2,3,4".Split(' ');
            A.CallTo(() => this.slicer.Slice(A<IEnumerable<string>>._, 1, 2)).Returns(new[] {"1", "3"});

            var slicedArguments = this.argumentSlicer.Slice(args, 1, 2);

            Assert.That(slicedArguments, Is.EquivalentTo(new [] { "test.dll", "--run:1,3"}));
        }
    }
}
