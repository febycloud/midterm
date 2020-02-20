using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusService.Models;

namespace BusService.Controllers
{
    public class FYBusRoutesController : Controller
    {
        private readonly BusServiceContext _context;

        public FYBusRoutesController(BusServiceContext context)
        {
            _context = context;
        }

        // GET: FYBusRoutes
        public async Task<IActionResult> Index()
        {
            return View(await _context.BusRoute.OrderBy(a => Convert.ToInt32(a.BusRouteCode)).ToListAsync());// sort by buscode,cuz string sorting is different int sorting
        }

        // GET: FYBusRoutes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var busRoute = await _context.BusRoute
                .FirstOrDefaultAsync(m => m.BusRouteCode == id);
            if (busRoute == null)
            {
                return NotFound();
            }

            return View(busRoute);
        }

        // GET: FYBusRoutes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FYBusRoutes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BusRouteCode,RouteName")] BusRoute busRoute)
        {
            if (ModelState.IsValid)
            {
                _context.Add(busRoute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(busRoute);
        }

        // GET: FYBusRoutes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var busRoute = await _context.BusRoute.FindAsync(id);
            if (busRoute == null)
            {
                return NotFound();
            }
            return View(busRoute);
        }

        // POST: FYBusRoutes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BusRouteCode,RouteName")] BusRoute busRoute)
        {
            if (id != busRoute.BusRouteCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(busRoute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusRouteExists(busRoute.BusRouteCode))
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
            return View(busRoute);
        }

        // GET: FYBusRoutes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var busRoute = await _context.BusRoute
                .FirstOrDefaultAsync(m => m.BusRouteCode == id);
            if (busRoute == null)
            {
                return NotFound();
            }

            return View(busRoute);
        }

        // POST: FYBusRoutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var busRoute = await _context.BusRoute.FindAsync(id);
            _context.BusRoute.Remove(busRoute);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusRouteExists(string id)
        {
            return _context.BusRoute.Any(e => e.BusRouteCode == id);
        }
    }
}
