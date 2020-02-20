using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusService.Models;
using Microsoft.AspNetCore.Http;

namespace BusService.Controllers
{
    public class RouteStopsController : Controller
    {
        private readonly BusServiceContext _context;

        public RouteStopsController(BusServiceContext context)
        {
            _context = context;
        }

        // GET: RouteStops
        public async Task<IActionResult> Index(string BusRouteCode)
        {
            if (BusRouteCode != null)
            {
                Response.Cookies.Append("BusRouteCode", BusRouteCode);//set cookie
                HttpContext.Session.SetString("BusRouteCode", BusRouteCode);//set  session
            }
            else if (Request.Query["BusRouteCode"].Any())
            {
                Response.Cookies.Append("BusRouteCode", Request.Query["BusRouteCode"]);//set cookie from query
                HttpContext.Session.SetString("BusRouteCode", Request.Query["BusRouteCode"]);//set  session query
                BusRouteCode = Request.Query["BusRouteCode"];
            }
            else if (Request.Cookies["BusRouteCode"] != null)
            {
                BusRouteCode = Request.Cookies["BusRouteCode"].ToString(); //get value from cookie
            }
            else if (HttpContext.Session.GetString("BusRouteCode") != null)
            {
                BusRouteCode = HttpContext.Session.GetString("BusRouteCode"); //get value from session
            }
            else
            {
                TempData["message"] = "Please choose a route";
                return RedirectToAction("Index", "FYBusRoutes");
            }

            var busRoute = _context.BusRoute.Where(a => a.BusRouteCode == BusRouteCode).FirstOrDefault();
            ViewData["BusRouteCode"] = busRoute.BusRouteCode;
            ViewData["BusRouteName"] = busRoute.RouteName;
            var busServiceContext = _context.RouteStop
                .Include(r => r.BusRouteCodeNavigation)
                .Include(r => r.BusStopNumberNavigation)
                .Where(a => a.BusRouteCode == BusRouteCode)
                .OrderBy(m => m.OffsetMinutes);
            return View(await busServiceContext.ToListAsync());
        }

        // GET: RouteStops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeStop = await _context.RouteStop
                .Include(r => r.BusRouteCodeNavigation)
                .Include(r => r.BusStopNumberNavigation)
                .FirstOrDefaultAsync(m => m.RouteStopId == id);
            if (routeStop == null)
            {
                return NotFound();
            }

            return View(routeStop);
        }

        // GET: RouteStops/Create
        public IActionResult Create()
        {
            
            string busRCode = string.Empty;
            if (Request.Cookies["BusRouteCode"] != null)
            {
                busRCode = Request.Cookies["BusRouteCode"].ToString();
            }
            else if (HttpContext.Session.GetString("BusRouteCode") != null)
            {
                busRCode = HttpContext.Session.GetString("BusRouteCode");
            }
            var busRoute = _context.BusRoute.Where(a => a.BusRouteCode == busRCode).FirstOrDefault();
            ViewData["BusRCode"] = busRoute.BusRouteCode;
            ViewData["BusRouteName"] = busRoute.RouteName;
          

            ViewData["BusRouteCode"] = new SelectList(_context.BusRoute, "BusRouteCode", "BusRouteCode");
            ViewData["BusStopNumber"] = new SelectList(_context.BusStop, "BusStopNumber", "Location");
            return View();
        }

        // POST: RouteStops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RouteStopId,BusRouteCode,BusStopNumber,OffsetMinutes")] RouteStop routeStop)
        {
            if (routeStop.OffsetMinutes < 0) ModelState.AddModelError("", "offset must greater than 0");
            string busRCode = string.Empty;
            if (Request.Cookies["BusRouteCode"] != null)
            {
                busRCode = Request.Cookies["BusRouteCode"].ToString();
            }
            else if (HttpContext.Session.GetString("BusRouteCode") != null)
            {
                busRCode = HttpContext.Session.GetString("BusRouteCode");
            }
            var busRoute = _context.BusRoute.Where(a => a.BusRouteCode == busRCode).FirstOrDefault();
            ViewData["BusRCode"] = busRoute.BusRouteCode;
            ViewData["BusRouteName"] = busRoute.RouteName;
            routeStop.BusRouteCode = busRCode;
            if (routeStop.OffsetMinutes == 0)
            {
                var isZero=_context.RouteStop.Where(a => a.OffsetMinutes == 0 && a.BusRouteCode == routeStop.BusRouteCode);
                if (isZero.Any())
                    ModelState.AddModelError("", "zero is already exist");
            }
            var isDuplicate=_context.RouteStop.Where(a => a.BusRouteCode == routeStop.BusRouteCode && a.BusStopNumber == routeStop.BusStopNumber);
            if (isDuplicate.Any())
            {
                ModelState.AddModelError("", "is duplicate");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(routeStop);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "new route stop added";
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.GetBaseException().Message);
                }
            }
            ViewData["BusRouteCode"] = new SelectList(_context.BusRoute, "BusRouteCode", "BusRouteCode", routeStop.BusRouteCode);
            ViewData["BusStopNumber"] = new SelectList(_context.BusStop, "BusStopNumber", "Location", routeStop.BusStopNumber);
            return View(routeStop);
        }

        // GET: RouteStops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeStop = await _context.RouteStop.FindAsync(id);
            if (routeStop == null)
            {
                return NotFound();
            }
            ViewData["BusRouteCode"] = new SelectList(_context.BusRoute, "BusRouteCode", "BusRouteCode", routeStop.BusRouteCode);
            ViewData["BusStopNumber"] = new SelectList(_context.BusStop, "BusStopNumber", "BusStopNumber", routeStop.BusStopNumber);
            return View(routeStop);
        }

        // POST: RouteStops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RouteStopId,BusRouteCode,BusStopNumber,OffsetMinutes")] RouteStop routeStop)
        {
            if (id != routeStop.RouteStopId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(routeStop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteStopExists(routeStop.RouteStopId))
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
            ViewData["BusRouteCode"] = new SelectList(_context.BusRoute, "BusRouteCode", "BusRouteCode", routeStop.BusRouteCode);
            ViewData["BusStopNumber"] = new SelectList(_context.BusStop, "BusStopNumber", "BusStopNumber", routeStop.BusStopNumber);
            return View(routeStop);
        }

        // GET: RouteStops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var routeStop = await _context.RouteStop
                .Include(r => r.BusRouteCodeNavigation)
                .Include(r => r.BusStopNumberNavigation)
                .FirstOrDefaultAsync(m => m.RouteStopId == id);
            if (routeStop == null)
            {
                return NotFound();
            }

            return View(routeStop);
        }

        // POST: RouteStops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var routeStop = await _context.RouteStop.FindAsync(id);
            _context.RouteStop.Remove(routeStop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouteStopExists(int id)
        {
            return _context.RouteStop.Any(e => e.RouteStopId == id);
        }
    }
}
