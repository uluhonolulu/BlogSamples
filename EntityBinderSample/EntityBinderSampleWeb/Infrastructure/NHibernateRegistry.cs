using StructureMap;
using NHibernate;
using StructureMap.Configuration.DSL;
using NHibernate.Cfg;

namespace EntityBinderSampleWeb.Infrastructure {
	public class NHibernateRegistry : Registry {
		public NHibernateRegistry() {
			var configuration = new Configuration().Configure();
			For<Configuration>().Singleton().Use(() => configuration);
			For<ISessionFactory>().Singleton().Use(ctx => ctx.GetInstance<Configuration>().BuildSessionFactory());
			For<ISession>().HybridHttpOrThreadLocalScoped().Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());
		}


	}
}