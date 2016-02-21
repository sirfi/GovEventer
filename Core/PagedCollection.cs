using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GovEventer.Core
{
    public interface IPagedCollection<T> : IReadOnlyCollection<T>
    {
        string Title { get; set; }
        IReadOnlyCollection<T> Collection { get; set; }
        int PageItemCount { get; set; }
        int PageNumber { get; set; }
        int PageCount { get; set; }
        int ItemCount { get; set; }
    }

    public class PagedCollection<T> : IPagedCollection<T>
    {
        public string Title { get; set; }

        public IReadOnlyCollection<T> Collection { get; set; }

        public int PageItemCount { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public int ItemCount { get; set; }

        public PagedCollection(IQueryable<T> queryable, int pageItemCount, int pageNumber, string title)
        {
            PageItemCount = pageItemCount;
            PageNumber = pageNumber;
            ItemCount = queryable.Count();
            Collection = queryable.Skip((pageNumber - 1) * pageItemCount).Take(pageItemCount).ToList();
            PageCount = Convert.ToInt32(Math.Ceiling(ItemCount / (1.0 * pageItemCount)));
            Title = title;
        }

        public PagedCollection()
        {
            Collection = new List<T>();
            PageItemCount = 0;
            PageNumber = 0;
            PageCount = 0;
            ItemCount = 0;
            Title = "";
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return Collection.Count; }
        }
    }
}