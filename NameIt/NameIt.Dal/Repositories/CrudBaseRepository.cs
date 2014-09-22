
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NameIt.Dal.Contracts;
using NameIt.Dal.Extensions;

namespace NameIt.Dal.Repositories
{
    public abstract partial class CrudBaseRepository<T> where T : class
    {
        protected IDbRepositoryContext<DbContext> _context;
        protected DbContext _db { get { return _context.Context; } }


        protected CrudBaseRepository(IDbRepositoryContext<DbContext> context, CrudBaseRepositoryConfiguration config)
        {
            _context = context;
            _context.Context.Configuration.AutoDetectChangesEnabled = true;
            _ojbectSet = this.GetEntityObjectSet<T>();
            _context.SharedBetweenRepositoriesCount += 1;

            AutoSave = config.AutoSave;
            DisposeContext = config.DisposeContext;
        }


        private CrudBaseRepository(IDbRepositoryContext<DbContext> context)
            : this(context, new CrudBaseRepositoryConfiguration { AutoSave = true, DisposeContext = true })
        {
        }


        protected SqlConnection GetConnection()
        {
            var scsb = new SqlConnectionStringBuilder(
               _db.Database.Connection.ConnectionString) {Pooling = true, AsynchronousProcessing = true};
            var conn = new SqlConnection(scsb.ConnectionString);
            conn.Open();
            return conn;
        }

        public virtual string LegacyConnectionString
        {
            get
            {
                var scsb = new SqlConnectionStringBuilder(
                     _db.Database.Connection.ConnectionString);
                return scsb.ConnectionString;
            }
        }

        #region Private methods
        protected void Add(T entity)
        {
            this.ObjectSet.Add(entity);
        }
        private DbSet<TEntity> GetEntityObjectSet<TEntity>() where TEntity : class
        {
            DbSet<TEntity> obj = _db.Set<TEntity>();

            return obj;
        }
        protected static Type GetMetaDataClass(T entity)
        {
            var dnAttribute = typeof(T).GetCustomAttributes(
                typeof(MetadataTypeAttribute), true).FirstOrDefault() as MetadataTypeAttribute;
            if (dnAttribute != null)
            {
                return dnAttribute.MetadataClassType;
            }

            return null;
        }
        /// <summary>
        /// Adds the error in the last error list.
        /// </summary>
        /// <param name="errorMessage">The error message to add in the list of errors.</param>
        protected void AddErrorMessage(string errorMessage)
        {
            this._listOfErrors.Add(errorMessage);
        }
        #endregion Private methods
    }

    public abstract partial class CrudBaseRepository<T> : ICrudBaseRepository<T> where T : class
    {
        protected List<string> _listOfErrors = new List<string>();

        /// <summary>
        /// The object set of the T entity.
        /// </summary>
        protected DbSet<T> _ojbectSet;

        protected DbSet<T> ObjectSet
        {
            get
            {
                return _ojbectSet;
            }
        }

        /// <summary>
        /// Gets an IQuerable strongly typed.
        /// </summary>
        /// <returns>IQueryable.</returns>
        public virtual IQueryable<T> GetQuery()
        {
            var entityName = _db.GetEntitySetName(typeof(T));
            return ((IObjectContextAdapter)_db).ObjectContext.CreateQuery<T>(entityName);
        }

        /// <summary>
        /// Creates and adds a new object in the datastore if the object is not found,
        /// or if it is found it is updated.
        /// </summary>
        /// <param name="entity">The POCO entity.</param>
        public virtual void CreateOrUpdate(T entity)
        {
            _db.CreateOrUpdate(entity, AutoSave);

        }

        /// <summary>
        /// Deletes the specified POCO entity.
        /// </summary>
        /// <param name="entity">The POCO entity.</param>
        public virtual void Delete(T entity)
        {
            // Define an ObjectStateEntry and EntityKey for the current object.
            //EntityKey key;
            object originalItem;
            var entitySet = _db.GetEntitySetName(typeof(T));
            EntityKey key = ((IObjectContextAdapter)_db).ObjectContext.CreateEntityKey(entitySet, entity);
            try
            {
                // Get the original item based on the entity key from the context
                // or from the database.
                if (((IObjectContextAdapter)_db).ObjectContext.TryGetObjectByKey(key, out originalItem))
                {
                    this.ObjectSet.Remove(entity);
                }
                else
                {
                    throw new Exception("The entity does not exist");
                }

                if (AutoSave)
                    _context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return ObjectSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Deletes the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public void Delete(Expression<Func<T, bool>> predicate)
        {
            Delete(ObjectSet.FirstOrDefault(predicate));
        }

        /// <summary>
        /// Gets the original entity.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual T GetOriginalEntity(Expression<Func<T, bool>> predicate)
        {
            var t = Get(predicate);

            object originalItem;
            var entitySet = _db.GetEntitySetName(typeof(T));
            EntityKey key = ((IObjectContextAdapter)_db).ObjectContext.CreateEntityKey(entitySet, t);

            if (((IObjectContextAdapter)_context.Context).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                return (T)originalItem;
            }
            else
            {
                throw new Exception("The entity does not exist");
            }
        }

        /// <summary>
        /// Finds the specified predicate.
        /// (Sorts the results in asc mode.)
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="orderby">The orderby.</param>
        /// <returns></returns>
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, Func<T, object> orderby)
        {
            return ObjectSet.Where(predicate).OrderBy(orderby).ToList();
        }

        /// <summary>
        /// Returns a collection of POCO objects filtered and sorted.
        /// </summary>
        /// <param name="predicate">The predicate expression to filter results.</param>
        /// <param name="orderByProperty">The field name to preform the sort operation on.</param>
        /// <param name="desc">if set to <c>true</c> the results is sorted descending, else ascending.</param>
        /// <returns>Collection of POCO entities.</returns>
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, string orderByProperty, bool desc)
        {
            return ObjectSet.Where(predicate).OrderBy(orderByProperty, desc).ToList();
        }

        /// <summary>
        /// Returns a collection of POCO objects filtered and sorted.
        /// Supports paging.
        /// </summary>
        /// <param name="predicate">The predicate expression to filter results.</param>
        /// <param name="orderByProperty">The field name to preform the sort operation on.</param>
        /// <param name="desc">if set to <c>true</c> the results is sorted descending, else ascending.</param>
        /// <param name="pageNumber">The page number, 1 based.</param>
        /// <param name="pageSize">The count of results per page.</param>
        /// <param name="totalRecords">The total number of records returnd by the filtered query.</param>
        /// <returns>Collection of POCO entities.</returns>
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate,
            string orderByProperty, bool desc, int pageNumber, int pageSize,
            out int totalRecords)
        {
            var q = ObjectSet.Where(predicate);
            totalRecords = q.Count();

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // in case the pageNumber is greater than totalPages
            // then we should use the last page to get the data
            if (pageNumber > totalPages) { pageNumber = totalPages; }

            if (totalRecords == 0)  //a case with no data
            {
                return q.OrderBy(orderByProperty, desc);
            }

            return q.OrderBy(orderByProperty, desc).Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Returns a collection of POCO objects filtered and sorted.
        /// Supports paging.
        /// </summary>
        /// <param name="predicate">The predicate expression to filter results.</param>
        /// <param name="orderby">The orderby expression to sort results.</param>
        /// <param name="desc">if set to <c>true</c> returns entities in descending order.</param>
        /// <param name="pageNumber">The page number, 1 based.</param>
        /// <param name="pageSize">The count of results per page.</param>
        /// <param name="totalRecords">The total number of records returnd by the filtered query.</param>
        /// <returns>
        /// Collection of POCO entities.
        /// </returns>
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, Func<T, object> orderby, bool desc, int pageNumber, int pageSize, out int totalRecords)
        {
            var q = ObjectSet.Where(predicate);
            totalRecords = q.Count();

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // in case the pageNumber is greater than totalPages
            // then we should use the last page to get the data
            if (pageNumber > totalPages) { pageNumber = totalPages; }

            if (!desc)
            {
                return q.DefaultIfEmpty<T>().OrderBy(orderby).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return q.DefaultIfEmpty<T>().OrderByDescending(orderby).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
        }

        /// <summary>
        /// Determines whether a POCO entity exists in the data store based on the filter expressions.
        /// </summary>
        /// <param name="predicate">The predicate expression to filter results.</param>
        /// <returns>
        ///   <c>true</c> if a POCO entity exists in the data store based in the specified predicate; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Contains(Expression<Func<T, bool>> predicate)
        {
            return GetQuery().Any(predicate);
        }

        //TODO: Review method IsObjectValid of CrudBaseRepository
        // http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.metadatatypeattribute.metadataclasstype.aspx
        // http://stackoverflow.com/questions/2656189/how-do-i-read-an-attribute-on-a-class-at-runtime
        // http://msdn.microsoft.com/en-us/library/kyaxdd3x.aspx
        // http://msdn.microsoft.com/en-us/library/system.reflection.memberinfo.getcustomattributes.aspx
        // http://stackoverflow.com/questions/2011031/attribute-querying-not-casting-to-base-type-of-attribute-what-gives
        /// <summary>
        /// Determines whether object is valid.
        /// </summary>
        /// <param name="entity">The T entity.</param>
        /// <returns>
        ///   <c>true</c> if object is valid; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsObjectValid(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            // here we check the entity properties
            Type entityTypeClass = entity.GetType();
            Type metadataTypeClass = GetMetaDataClass(entity);
            if (metadataTypeClass == null)
            {
                this.AddErrorMessage(entityTypeClass.ToString() + " does not have a metadata attribure");
                return false;
            }

            PropertyInfo[] properties = metadataTypeClass.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var valProps = from PropertyInfo property in properties
                           where property.GetCustomAttributes(typeof(ValidationAttribute), true).Length > 0
                           select new
                           {
                               Property = property,
                               ValidationAttributes = property.GetCustomAttributes(typeof(ValidationAttribute), true)
                           };

            foreach (var item in valProps)
            {
                foreach (ValidationAttribute attribute in item.ValidationAttributes)
                {
                    if ((!attribute.IsValid(entityTypeClass.GetProperty(item.Property.Name).GetValue(entity, null))))
                    {
                        this.AddErrorMessage(item.Property.Name + ": " + attribute.ErrorMessage);
                    }

                    //// Validate mandatory integer properties with 0 as value but exclude ids
                    //if ((attribute is RequiredAttribute && item.Property.PropertyType == typeof(int)))
                    //{
                    //    int propValue = (int)entityTypeClass.GetProperty(item.Property.Name).GetValue(entity, null);
                    //    if (propValue == 0 && !item.Property.Name.ToLower().Equals("id"))
                    //    {
                    //        this.AddErrorMessage(item.Property.Name + ": " + attribute.ErrorMessage);
                    //    }
                    //}
                }

                // Datatime validation
                if (item.Property.PropertyType == typeof(DateTime))
                {
                    DateTime propValue = (DateTime)entityTypeClass.GetProperty(item.Property.Name).GetValue(entity, null);
                    if (propValue == DateTime.MinValue)
                    {
                        this.AddErrorMessage(item.Property.Name + ": " + string.Format("date is not valid"));
                    }
                }
            }

            return this._listOfErrors.Count == 0;
        }


        #region Async
        public async Task<int> AddAsync(T t)
        {
            _db.Set<T>().Add(t);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(T t)
        {
            _db.Entry(t).State = EntityState.Deleted;
            return await _context.CommitAsync();
        }

        public async Task<int> UpdateAsync(T t)
        {
            _db.Entry(t).State = EntityState.Modified;
            return await _context.CommitAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _db.Set<T>().CountAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _db.Set<T>().SingleOrDefaultAsync(match);
        }

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _db.Set<T>().Where(match).ToListAsync();
        }
        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            _context.SharedBetweenRepositoriesCount -= 1;
            if (_context.SharedBetweenRepositoriesCount == 0)
            {
                if (_db.IsDirty()) _db.SaveChanges();
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        #endregion IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DisposeContext && _context != null)
                {
                    _context.Dispose();
                    _context = null;
                }


            }
        }

    }

    public abstract partial class CrudBaseRepository<T> : IRepositoryWithContext where T : class
    {
        private bool _disposeContext = true;
        private bool _autoSave = true;
        public bool AutoSave
        {
            get { return _autoSave; }
            set { _autoSave = value; }
        }
        public bool DisposeContext
        {
            get { return _disposeContext; }
            set { _disposeContext = value; }
        }
    }
}
