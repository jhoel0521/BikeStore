using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BikeStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace BikeStore.Controllers
{
    [Authorize]
    public class OrderItemsController : Controller
    {
        private readonly BikeStoreContext _context;

        public OrderItemsController(BikeStoreContext context)
        {
            _context = context;
        }

        // GET: OrderItems
        public async Task<IActionResult> Index(string ColunOrd, string ordenamiento, string OrderId, int? page, int pageSize = 10)
        {
            ViewBag.ColunOrd = ColunOrd;
            ViewBag.ordenamiento = ordenamiento;
            ViewBag.OrderId = OrderId;
            var query = _context.OrderItems.AsQueryable();
            if (!string.IsNullOrEmpty(ColunOrd))
            {
                switch (ColunOrd)
                {
                    case "OrderId":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.OrderId) : query.OrderByDescending(c => c.OrderId);
                        break;
                    case "ProductName":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.Product.ProductName) : query.OrderByDescending(c => c.Product.ProductName);
                        break;
                    case "Quantity":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.Quantity) : query.OrderByDescending(c => c.Quantity);
                        break;
                    case "Price":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.Price) : query.OrderByDescending(c => c.Price);
                        break;
                    case "Discount":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.Discount) : query.OrderByDescending(c => c.Discount);
                        break;
                    default:
                        break;
                }
            }
            if (int.TryParse(OrderId, out int orderId))
            {
                query = query.Where(o => o.OrderId == orderId);
            }
            query = query.Include(o => o.Order).Include(o => o.Product);
            var (list, totalItems, totalPages, pageNumber) = await PaginationUtility.PaginateAsync(query, pageSize, page);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = totalPages;
            ViewBag.Action = nameof(Index);
            ViewBag.pageSize = pageSize;
            return View(list);
        }

        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderItemId == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: OrderItems/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            var context = _context.Products.
               Select(c => new { c.ProductId, FullName = $"{c.ProductId}: {c.ProductName} " }).ToList();
            ViewData["ProductId"] = new SelectList(context, "ProductId", "FullName");
            return View();
        }

        // POST: OrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderItemId,OrderId,ProductId,Quantity,Price,Discount")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderItem.OrderId);
            var context = _context.Products.
              Select(c => new { c.ProductId, FullName = $"{c.ProductId}: {c.ProductName} " }).ToList();
            ViewData["ProductId"] = new SelectList(context, "ProductId", "FullName", orderItem.ProductId);
            return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderItem.OrderId);
            var context = _context.Products.
              Select(c => new { c.ProductId, FullName = $"{c.ProductId}: {c.ProductName} " }).ToList();
            ViewData["ProductId"] = new SelectList(context, "ProductId", "FullName", orderItem.ProductId);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderItemId,OrderId,ProductId,Quantity,Price,Discount")] OrderItem orderItem)
        {
            if (id != orderItem.OrderItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.OrderItemId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderItem.OrderId);
            var context = _context.Products.
             Select(c => new { c.ProductId, FullName = $"{c.ProductId}: {c.ProductName} " }).ToList();
            ViewData["ProductId"] = new SelectList(context, "ProductId", "FullName", orderItem.ProductId);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderItemId == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.OrderItemId == id);
        }
    }
}
