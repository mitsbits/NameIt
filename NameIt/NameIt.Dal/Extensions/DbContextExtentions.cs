using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace NameIt.Dal.Extensions
{
    public static class DbContextExtentions
    {

        public static string GetEntitySetName(this DbContext db, Type type)
        {
            IObjectContextAdapter adapter = db as IObjectContextAdapter;
            var container =
                adapter
                .ObjectContext
                .MetadataWorkspace.GetEntityContainer((adapter).ObjectContext.DefaultContainerName, DataSpace.CSpace);
            return (from meta in container.BaseEntitySets
                    where meta.ElementType.Name == type.Name
                    select meta.Name).First();
        }

        public static int CreateOrUpdate<T>(this DbContext db, T entity, bool autoSave = true) where T : class
        {
            IObjectContextAdapter adapter = db as IObjectContextAdapter;
            // Define an ObjectStateEntry and EntityKey for the current object.
            //EntityKey key;
            object originalItem;
            var entitySet = db.GetEntitySetName(typeof(T));
            //var entitySet = ObjectSet.EntitySet.Name
            EntityKey key = (adapter).ObjectContext.CreateEntityKey(entitySet, entity);

            // check if object is valid before save.
            //if (this.IsObjectValid(entity))
            //{
            // Get the original item based on the entity key from the context
            // or from the database.
            if ((adapter).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                // Call the ApplyCurrentValues method to apply changes.
                db.Entry(originalItem).CurrentValues.SetValues(entity);
            }
            else
            {
                db.Entry(entity).State = EntityState.Added;
            }

            //else
            //{
            //    throw new InvalidOperationException(_listOfErrors.EnumerableToDelimited(","));
            //}

            // commit the changes
            if (autoSave)
                return db.SaveChanges();

            return default(int);

        }

        public static Task<int> CreateOrUpdateAsync<T>(this DbContext db, T entity, bool autoSave = true) where T : class
        {
            IObjectContextAdapter adapter = db as IObjectContextAdapter;
            // Define an ObjectStateEntry and EntityKey for the current object.
            //EntityKey key;
            object originalItem;
            var entitySet = db.GetEntitySetName(typeof(T));
            //var entitySet = ObjectSet.EntitySet.Name
            EntityKey key = (adapter).ObjectContext.CreateEntityKey(entitySet, entity);

            // check if object is valid before save.
            //if (this.IsObjectValid(entity))
            //{
            // Get the original item based on the entity key from the context
            // or from the database.
            if ((adapter).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                // Call the ApplyCurrentValues method to apply changes.
                db.Entry(originalItem).CurrentValues.SetValues(entity);
            }
            else
            {
                db.Entry(entity).State = EntityState.Added;
            }

            //else
            //{
            //    throw new InvalidOperationException(_listOfErrors.EnumerableToDelimited(","));
            //}

            // commit the changes
            if (autoSave)
                return db.SaveChangesAsync();

            return Task.FromResult(default(int));

        }

        /// <summary>
        ///     Detect whether the context is dirty (i.e., there are changes in entities in memory that have
        ///     not yet been saved to the database).
        /// </summary>
        /// <param name="context">The database context to check.</param>
        /// <param name="db">Db Context</param>
        /// <returns>True if dirty (unsaved changes); false otherwise.</returns>
        public static bool IsDirty(this DbContext db)
        {
            if (db == null) throw new ArgumentNullException("db");

            // Query the change tracker entries for any adds, modifications, or deletes.
            try
            {
                IEnumerable<DbEntityEntry> res = from e in db.ChangeTracker.Entries()
                                                 where e.State.HasFlag(EntityState.Added) ||
                                                       e.State.HasFlag(EntityState.Modified) ||
                                                       e.State.HasFlag(EntityState.Deleted)
                                                 select e;

                return res.Any();
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
