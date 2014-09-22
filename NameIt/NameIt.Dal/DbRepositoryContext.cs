using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace NameIt.Dal
{
    public abstract class DbRepositoryContext : IDbRepositoryContext<DbContext> 
    {
        #region Constructor
        protected DbRepositoryContext(string connectionString)
        {
            Context = new DbContext(connectionString);
        }

        #endregion

        #region Behaviour

        public virtual DbContext Context { get; private set; }


        public virtual bool TestConnection()
        {

            int oldTimeOut = Context.Database.CommandTimeout ?? 1;

            try
            {
                Context.Database.CommandTimeout = 1;
                Context.Database.Connection.Open();
                Context.Database.Connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                Context.Database.CommandTimeout = oldTimeOut;
            }
        }
        public virtual string LegacyConnectionString
        {
            get
            {
                var entityConnection = Context.Database.Connection as EntityConnection;
                if (null != entityConnection)
                {
                    return entityConnection.StoreConnection.ConnectionString;
                }
                return string.Empty;
            }
        }
        public virtual int SharedBetweenRepositoriesCount
        {
            get;
            set;
        }
        #endregion
        public virtual int Commit()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        public virtual Task<int> CommitAsync()
        {
            try
            {
                return Context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        #region IDisposable
        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Context != null
                    && Context.Database != null
                    && Context.Database.Connection != null)
                {
                    if (!Context.Database.Connection.State.HasFlag(ConnectionState.Closed)) { Context.Database.Connection.Close(); }
                    Context.Dispose();
                    Context = null;
                }
            }
        }
        #endregion

    }
}
