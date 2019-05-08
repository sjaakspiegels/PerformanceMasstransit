namespace Performance.Infrastructure.NHibernate
{
    using FluentNHibernate;

    using global::NHibernate;

    using Performance.Domain;

    using Voogd.Library.DomainBase.NHibernate;

    public class Dummy
    {
        private int aantal;
    }

    public class WorkRepository : Repository<Dummy>,
                                  IWorkRepository
    {
        public void DoWork()
        {
            this.Session.CreateSQLQuery("EXEC Conversie..Delay").ExecuteUpdate();
        }
    }
}
