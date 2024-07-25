using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace BikeStore.Controllers
{
    public class PaginationUtility
    {
        public static async Task<(List<T> list, int totalItems, int totalPages, int pageNumber)> PaginateAsync<T>(
            IQueryable<T> query,
            int pageSize,
            int? page)
        {
            int pageNumber = page ?? 1;
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if(pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }
            var list = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (list, totalItems, totalPages, pageNumber);
        }
    }
}
