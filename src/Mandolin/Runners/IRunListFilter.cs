namespace Mandolin.Runners
{
	using System.Collections.Generic;

	public interface IRunListFilter
	{
		IEnumerable<string> Filter(IEnumerable<string> suite);
	}
}
