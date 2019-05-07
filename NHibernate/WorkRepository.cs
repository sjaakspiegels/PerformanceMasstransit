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

        //protected ISession Session
        //{
        //    get
        //    {
        //        return UnitOfWork.Current.Session;
        //    }
        //}

        //protected ISession Session => UnitOfWork.Current.Session;

        public void DoWork(int delay)
        {
            this.Session.CreateSQLQuery("EXEC Conversie..Delay").ExecuteUpdate();
        }
    }
}
