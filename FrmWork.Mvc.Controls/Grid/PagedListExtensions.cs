using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FrmWork.Objects.Interfaces.General;
using Microsoft.EntityFrameworkCore;

namespace FrmWork.Mvc.Controls.Grid
{
    public static class PagedListExtensions
    {
        /// <summary>
        /// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{T}"/>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> superset, int pageNumber, int pageSize)
        {
            return new PagedList<T>(superset, pageNumber, pageSize);
        }

        /// <summary>
        /// Async creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IEnumerable{T}"/>, it will be treated as such.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{T}"/>
        public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> superset) => await superset.ToListAsync();

        /// <summary>
        /// Async creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{T}"/>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize)
        {
            var subset = new List<T>();
            var totalCount = 0;

            if ((superset != null) && (superset.Any()))
            {
                totalCount = superset.Count();

                subset.AddRange(
                    (pageNumber == 1)
                        ? await superset.Skip<T>(0).Take<T>(pageSize).ToListAsync<T>().ConfigureAwait(false)
                        : await superset.Skip<T>(((pageNumber - 1) * pageSize)).Take<T>(pageSize).ToListAsync<T>().ConfigureAwait(false)
                );
            }

            return new StaticPagedList<T>(subset, pageNumber, pageSize, totalCount);
        }

        /// <summary>
        /// Async creates a subset of this collection of objects that can be individually accessed by index (defaulting to the first page) and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="T">The type of object the collection should contain.</typeparam>
        /// <param name="superset"></param>
        /// <param name="pageNumber">The one-based index of the subset of objects to be contained by this instance, defaulting to the first page.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{T}"/>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int? pageNumber, int pageSize)
        {
            return await ToPagedListAsync(superset.AsQueryable(), pageNumber ?? 1, pageSize);
        }

        public static IPagedList<TEntity> GetPaged<TEntity>(this IQueryable<TEntity> query
            , IHasFilter<TEntity> filterPanel, IPagedList pagerOptions, string sort) where TEntity : class, IHasId<long>
        {
            if (filterPanel != null)
            {
                query = filterPanel.GetWhereQuery(query);
            }
            var result = query.AsNoTracking().OrderByDescending(e => e.Id).ToPagedList(pagerOptions.PageNumber, pagerOptions.PageSize);
            //pagerOptions.Map(result);
            return result;
        }

        public static IPagedList<TEntity> GetPaged<TEntity>(this IQueryable<TEntity> query
            , IHasFilter<TEntity> filterPanel, IPagedList pagerOptions, string sort, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IHasId<long>
        {
            query = includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            var result = query.GetPaged(filterPanel, pagerOptions, sort);
            //pagerOptions.Map(result);
            return result;
        }
    }
}