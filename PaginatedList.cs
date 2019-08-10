using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity
{
    // dykstra3 - Add paging to Student's Index
    public class PaginatedList<T> : List<T>
    {
        // auto-implemented properties to get values of Page Index and Total Pages
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }


        // constructor
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        // add properties to control previous page and next page
        // disable or unable the previous page
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        // add properties to control previous page and next page
        // disable or enable the next page
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        /* The async method is added to get page size and page number, then apply Skip and Take statment
         * to the IQueryble.
         * ToListAsync at the end helps return only requested page. 
         */
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}