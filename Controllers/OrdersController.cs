using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BikeStore.Models;
using Microsoft.AspNetCore.Authorization;
using Rotativa.AspNetCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BikeStore.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly BikeStoreContext _context;

        public OrdersController(BikeStoreContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string ColunOrd, string ordenamiento, string CustomerId, string StartDate, string EndDate, int? page)
        {
            var context = _context.Customers
                .Select(c => new { c.CustomerId, FullName = $"{c.CustomerId}: {c.FirstName} {c.LastName}" }).ToList();
            ViewData["Customer"] = new SelectList(context, "CustomerId", "FullName");
            ViewData["CustomerId"] = CustomerId;
            ViewData["ColunOrd"] = ColunOrd;
            ViewData["ordenamiento"] = ordenamiento;
            ViewData["StartDate"] = StartDate;
            ViewData["EndDate"] = EndDate;

            var query = _context.Orders.AsQueryable();

            if (int.TryParse(CustomerId, out int customerId) && customerId != 0)
            {
                query = query.Where(o => o.CustomerId == customerId);
            }
            if (DateOnly.TryParse(StartDate, out DateOnly startDate))
            {
                query = query.Where(o => o.OrderDate >= startDate);
            }
            if (DateOnly.TryParse(EndDate, out DateOnly endDate))
            {
                query = query.Where(o => o.OrderDate <= endDate);
            }

            if (!string.IsNullOrEmpty(ColunOrd))
            {
                switch (ColunOrd)
                {
                    case "OrderDate":
                        query = ordenamiento == "↑" ? query.OrderBy(o => o.OrderDate) : query.OrderByDescending(o => o.OrderDate);
                        break;
                    case "Customer":
                        query = ordenamiento == "↑" ? query.OrderBy(o => o.Customer.FirstName + " " + o.Customer.LastName) : query.OrderByDescending(o => o.Customer.FirstName + " " + o.Customer.LastName);
                        break;
                    case "OrderId":
                        query = ordenamiento == "↑" ? query.OrderBy(o => o.OrderId) : query.OrderByDescending(o => o.OrderId);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(o => o.OrderId);
                ViewData["ColunOrd"] = "OrderId";
                ViewData["ordenamiento"] = "↓";
            }

            var (list, totalItems, totalPages, pageNumber) = await PaginationUtility.PaginateAsync(query, 10, page);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.Action = nameof(Index);
            return View(list);
        }


        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                 .Include(o => o.Customer)
                 .Include(o => o.OrderItems)
                 .ThenInclude(oi => oi.Product)
                 .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var context = _context.Customers.
                Select(c => new { c.CustomerId, FullName = $"{c.CustomerId}: {c.FirstName} {c.LastName}" }).ToList();
            ViewData["CustomerId"] = new SelectList(context, "CustomerId", "FullName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var context = _context.Customers.
               Select(c => new { c.CustomerId, FullName = $"{c.CustomerId}: {c.FirstName} {c.LastName}" }).ToList();
            ViewData["CustomerId"] = new SelectList(context, "CustomerId", "FullName", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var context = _context.Customers.
               Select(c => new { c.CustomerId, FullName = $"{c.CustomerId}: {c.FirstName} {c.LastName}" }).ToList();
            ViewData["CustomerId"] = new SelectList(context, "CustomerId", "FullName", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var context = _context.Customers.
               Select(c => new { c.CustomerId, FullName = $"{c.CustomerId}: {c.FirstName} {c.LastName}" }).ToList();
            ViewData["CustomerId"] = new SelectList(context, "CustomerId", "FullName", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
        //PDF
        public async Task<IActionResult> PDF(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return new ViewAsPdf(order);
        }
    }
}
