namespace Mandolin.Console
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition.Hosting;
	using System.Linq;
	using System.Text;
	using Autofac;
	using Autofac.Integration.Mef;
	using Mandolin.Runners;

	public class Bootstrap
		: IDisposable
	{
		private IContainer container;
		private ILifetimeScope scope;

		public Bootstrap()
		{
			this.Configure();
		}

		public T Resolve<T>()
		{
			return this.scope.Resolve<T>();
		}

		public void Dispose()
		{
			this.scope.Dispose();
			this.container.Dispose();
		}

		private void Configure()
		{
			var builder = new ContainerBuilder();

			var catalog = new DirectoryCatalog(@"plugins");
			builder.RegisterComposablePartCatalog(catalog);

			builder.RegisterType<SlicerFactory>().AsSelf();
			builder.RegisterType<RunListFilterFactory>().AsSelf();

			builder.RegisterType<TestSuiteExtractor>().As<ITestSuiteExtractor>();
			builder.RegisterType<DefaultRunListBuilder>().As<IRunListBuilder>();
			builder.RegisterType<DefaultRunListFilter>()
								.As<IRunListFilter>()
								.Named<IRunListFilter>("mandolin-default");
			
			builder.Register(context => context.Resolve<SlicerFactory>().GetSlicer()).As<ISlicer>();
			builder.Register(context =>
				{
					var defaultFilter = context.ResolveNamed<IRunListFilter>("mandolin-default");
					var availableFilters = context.Resolve<IEnumerable<IRunListFilter>>();

					return new RunListFilterFactory(defaultFilter, availableFilters);
				}).As<RunListFilterFactory>();
			

			this.container = builder.Build();
			this.scope = this.container.BeginLifetimeScope();
		}
	}
}
