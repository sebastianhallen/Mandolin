namespace Mandolin.Console
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using Mandolin.Runners;

	internal class RunListFilterFactory
	{
		private const string SettingsKey = "run-list-filter";
		private readonly IRunListFilter defaultFilter;
		private readonly IEnumerable<IRunListFilter> availableFilters;

		public RunListFilterFactory(IRunListFilter defaultFilter, IEnumerable<IRunListFilter> availableFilters)
		{
			this.defaultFilter = defaultFilter;
			this.availableFilters = availableFilters;
		}

		public IRunListFilter GetFilter()
		{
			var wantedRunListFilterSetting = ConfigurationManager.AppSettings.Get(SettingsKey);
			if (wantedRunListFilterSetting == null)
			{
				return this.defaultFilter;
			}

			var matchingFilterOverride = (from filter in this.availableFilters
			                             where wantedRunListFilterSetting.Equals(filter.GetType().ToString())
			                             select filter).ToArray();

			if (!matchingFilterOverride.Any())
			{
				var message = "No run list filters matching '{0}' could be found in Mandolin or in the plugins folder";
				throw new Exception(string.Format(message, wantedRunListFilterSetting));
			}

			if (matchingFilterOverride.Count() > 1)
			{
				var messsage = "Multiple run list filters matching '{0}' found, unable to determine which one to use";
				throw new Exception(string.Format(messsage, wantedRunListFilterSetting));
			}

			return matchingFilterOverride.Single();
		}
	}
}