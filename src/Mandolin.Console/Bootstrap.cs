namespace Mandolin.Console
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Autofac;
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
			var containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterType<SlicerFactory>().AsSelf();
			containerBuilder.RegisterType<TestSuiteExtractor>().As<ITestSuiteExtractor>();
			containerBuilder.RegisterType<DefaultRunListBuilder>().As<IRunListBuilder>();
			containerBuilder.RegisterType<DefaultRunListFilter>().As<IRunListFilter>();
			containerBuilder.Register(context => context.Resolve<SlicerFactory>().GetSlicer()).As<ISlicer>();

			this.container = containerBuilder.Build();
			this.scope = this.container.BeginLifetimeScope();
		}
	}
}
