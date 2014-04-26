namespace Plugin.Test
{
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using Mandolin.Runners;

	[Export(typeof(IRunListFilter))]
	public class TestRunListFilter
		: IRunListFilter
    {
		public IEnumerable<string> Filter(IEnumerable<string> suite)
		{
			return new [] { "custom run list filter" };
		}
    }
}
