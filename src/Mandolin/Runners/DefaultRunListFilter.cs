namespace Mandolin.Runners
{
	using System.Collections.Generic;

	public class DefaultRunListFilter
		: IRunListFilter
	{
		public IEnumerable<string> Filter(IEnumerable<string> suite)
		{
			return suite;
		}
	}
}