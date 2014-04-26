namespace Plugin.Test
{
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using Mandolin.Runners;

	[Export(typeof(IRunListFilter))]
	public class ExampleRunListFilter
		: IRunListFilter
	{
		/// <summary>
		/// an example filter that replaces the whole suite with a single fake test
		/// </summary>
		/// <param name="suite"></param>
		/// <returns></returns>
		public IEnumerable<string> Filter(IEnumerable<string> suite)
		{
			return new [] { "custom run list filter" };
		}
	}
}
