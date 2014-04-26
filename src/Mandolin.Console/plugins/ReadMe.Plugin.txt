You can override the default behavior of certain parts of Mandolin with custom plugins implementing the interface you want to override and adding a Mef Export attribute. 
Do this by placing your placing an assembly with your custom plugin implementation in this folder and add the appropriate setting to Mandolin.Console.config as described below.

Place your custom implementations of Mandolin.Runners.IRunListFilter in this directory, and add a app settings value to configure an override.

To override the run list filter, implement Mandolin.Runners.IRunListFilter and add an app settings value with the:
key: "run-list-filter"
value: "your custom type"
Configuration example: 
	<appSettings>
		<add key="run-list-filter" value="CustomMandolinPlugins.MyCustomRunListFilter"/>
	</appSettings>

Custom IRunListFilter example:
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
