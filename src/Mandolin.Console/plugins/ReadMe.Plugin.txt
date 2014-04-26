You can override the default behavior of certain parts of Mandolin with custom plugins implementing the interface you want to override. 
Do this by placing your placing an assembly with your custom plugin implementation in this folder and add the appropriate setting to Mandolin.Console.config as described below.

Place your custom implementations of Mandolin.Runners.IRunListFilter in this directory, and add a app settings value to configure an override.

To override the run list filter, implement Mandolin.Runners.IRunListFilter and add an app settings value with the:
key: "run-list-filter"
value: "your custom type"
Example: 
	<appSettings>
		<add key="run-list-filter" value="CustomMandolinPlugins.MyCustomRunListFilter"/>
	</appSettings>