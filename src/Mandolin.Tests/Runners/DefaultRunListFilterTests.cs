namespace Mandolin.Tests.Runners
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Mandolin.Runners;
	using NUnit.Framework;

	[TestFixture]
	public class DefaultRunListFilterTests
	{
		[Test]
		public void Should_not_do_any_filtering()
		{
			var filter = new DefaultRunListFilter();
			var unfiltered = new[] {"foo", "bar"};

			var filtered = filter.Filter(unfiltered);

			Assert.That(filtered, Is.SameAs(unfiltered));
		}
	}
}
