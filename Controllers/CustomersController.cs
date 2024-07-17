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
    public class CustomersController : Controller
    {
        private readonly BikeStoreContext _context;

        public CustomersController(BikeStoreContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string searchString, string ColunOrd, string ordenamiento)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(
                    c =>
                        (c.FirstName + " " + c.LastName + " " + c.Email + " " + c.Phone + " " + c.Street + " " + c.City + " " + c.State + " " + c.ZipCode).Contains(searchString)
                );
            }

            if (!string.IsNullOrEmpty(ColunOrd))
            {
                switch (ColunOrd)
                {
                    case "FirstName":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.FirstName) : query.OrderByDescending(c => c.FirstName);
                        break;
                    case "LastName":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.LastName) : query.OrderByDescending(c => c.LastName);
                        break;
                    case "Email":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.Email) : query.OrderByDescending(c => c.Email);
                        break;
                    case "Phone":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.Phone) : query.OrderByDescending(c => c.Phone);
                        break;
                    case "Street":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.Street) : query.OrderByDescending(c => c.Street);
                        break;
                    case "City":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.City) : query.OrderByDescending(c => c.City);
                        break;
                    case "State":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.State) : query.OrderByDescending(c => c.State);
                        break;
                    case "ZipCode":
                        query = ordenamiento == "↑" ? query.OrderBy(c => c.ZipCode) : query.OrderByDescending(c => c.ZipCode);
                        break;
                    default:
                        break;
                }
            }

            var list = await query.ToListAsync();
            ViewBag.SearchString = searchString;
            ViewBag.ColunOrd = ColunOrd;
            ViewBag.ordenamiento = ordenamiento;
            return View(list);
        }



        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Phone,Email,Street,City,State,ZipCode")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,Phone,Email,Street,City,State,ZipCode")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
