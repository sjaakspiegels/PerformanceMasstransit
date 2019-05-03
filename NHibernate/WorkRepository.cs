namespace Performance.Infrastructure.NHibernate
{
    using global::NHibernate;

    using Performance.Domain;

    using Voogd.Library.DomainBase.NHibernate;

    public class WorkRepository :IWorkRepository
    {
        protected ISession Session => UnitOfWork.Current.Session;

        public void DoWork(int delay)
        {
            this.Session.CreateSQLQuery("SELECT 1").ExecuteUpdate();
        }
    }
}
