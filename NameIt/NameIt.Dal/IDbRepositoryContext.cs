using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NameIt.Dal
{
    public interface IDbRepositoryContext<out TContext> : IDisposable where TContext : DbContext
    {
        /// <summary>
        /// Gets the object context.
        /// </summary>
        /// <value>
        /// The object context.
        /// </value>
        DbContext Context { get; }

        /// <summary>
        /// Commits changes.
        /// </summary>
        /// <returns>The number of affected rows</returns>
        int Commit();

        /// <summary>
        /// Commits changes asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<int> CommitAsync();

        /// <summary>
        /// Commits the changes according with the specified <c>SaveOptions</c>.
        /// </summary>
        /// <param name="saveOptions">The save options.</param>
        /// <returns>The number of affected rows</returns>
        //int Commit(SaveOptions saveOptions);

        /// <summary>
        /// Test the operation of the underluing provider.
        /// </summary>
        bool TestConnection();

        /// <summary>
        /// Get the number of repositorie instances that share the context
        /// </summary>
        int SharedBetweenRepositoriesCount { get; set; }
    }
}
