namespace Performance.Infrastructure.NHibernate
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using FluentNHibernate.Cfg;

    using global::NHibernate;
    using global::NHibernate.Cfg;

    using Performance.Domain;

    using Voogd.Library.DomainBase.NHibernate;

    /// <summary>
    /// NHibernate installer
    /// </summary>
    public class NHibernateInstaller : IWindsorInstaller
        {
            /// <summary>
            /// NHibernate installeren
            /// </summary>
            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Install(new UnitOfWorkInstaller());
                container.Register(Component.For<ISessionFactory>().UsingFactoryMethod(CreateSessionFactory).LifeStyle
                    .Singleton);
                container.Register(Component.For<UnitOfWorkInterceptor>().LifeStyle.Transient);
                container.Register(Component.For<IWorkRepository>().ImplementedBy<WorkRepository>()
                    .LifeStyle.Transient);
            }

            /// <summary>
            /// Maak de session factory
            /// </summary>
            private static ISessionFactory CreateSessionFactory()
            {
                return
                    Fluently.Configure(new Configuration().Configure())
                        .BuildSessionFactory();
            }
        }
    }