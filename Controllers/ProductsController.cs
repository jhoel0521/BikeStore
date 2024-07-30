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

namespace BikeStore.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly BikeStoreContext _context;

        public ProductsController(BikeStoreContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string ColunOrd, string ordenamiento, string searchString, string StartPrice, string EndPrice, int? page, int pageSize = 10, bool pdf = false, bool allPDF = false)
        {
            ViewBag.ColunOrd = ColunOrd;
            ViewBag.ordenamiento = ordenamiento;
            ViewBag.searchString = searchString;
            ViewBag.StartPrice = StartPrice;
            ViewBag.EndPrice = EndPrice;
            if (pdf && allPDF)
            {
                var customers = await _context.Products.ToListAsync();
                return new ViewAsPdf("PDF", new { Products = customers, AllPDF = allPDF });
            }
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(ColunOrd))
            {
                switch (ColunOrd)
                {
                    case "ProductName":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.ProductName) : query.OrderByDescending(c => c.ProductName);
                        break;
                    case "ModelYear":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.ModelYear) : query.OrderByDescending(c => c.ModelYear);
                        break;
                    case "Price":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.Price) : query.OrderByDescending(c => c.Price);
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(
                    c => c.ProductName.Contains(searchString)
                );
            }
            if (int.TryParse(StartPrice, out int startPrice))
            {
                query = query.Where(o => o.Price >= startPrice);
            }
            if (int.TryParse(EndPrice, out int endPrice))
            {
                query = query.Where(o => o.Price <= endPrice);
            }
            var (list, totalItems, totalPages, pageNumber) = await PaginationUtility.PaginateAsync(query, pageSize, page);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = totalPages;
            ViewBag.Action = nameof(Index);
            ViewBag.pageSize = pageSize;
            var context = _context.Products
                .Select(c => new { c.ProductId, FullName = $"{c.ProductName}" }).ToList();
            ViewData["Products"] = new SelectList(context, "ProductId", "FullName");
            if (pdf)
            {
                return new ViewAsPdf("PDF", new { Products = list, CurrentPage = pageNumber, TotalItems = totalItems, TotalPages = totalPages, PageSize = pageSize, AllPDF = allPDF });

            }
            else
            {
                return View(list);
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ModelYear,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ModelYear,Price")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
        public async Task<IActionResult> TopProductos(int cantTop = 10)
        {
            var topProducts = await _context.OrderItems
                .GroupBy(o => o.ProductId)
                .Select(g => new { ProductId = g.Key, Total = g.Sum(o => o.Quantity) })
                .OrderByDescending(o => o.Total)
                .Take(cantTop)
                .ToListAsync();

            var products = await _context.Products
                .Where(p => topProducts.Select(o => o.ProductId).Contains(p.ProductId))
                .ToListAsync();

            var viewModel = new TopProductsViewModel
            {
                Products = products,
                TopProducts = topProducts,
                CantTop = cantTop
            };

            return new ViewAsPdf(viewModel);
        }

    }
}
