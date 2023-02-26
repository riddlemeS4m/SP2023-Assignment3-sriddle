using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SP2023_Assignment3_sriddle.Data;
using SP2023_Assignment3_sriddle.Models;

namespace SP2023_Assignment3_sriddle.Controllers
{
    public class CastsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CastsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Casts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cast.Include(c => c.Actor).Include(c => c.Movie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Casts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cast == null)
            {
                return NotFound();
            }

            var cast = await _context.Cast
                .Include(c => c.Actor)
                .Include(c => c.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cast == null)
            {
                return NotFound();
            }

            return View(cast);
        }

        // GET: Casts/Create
        public IActionResult Create()
        {            
            CastVM castVM = new CastVM();
            //castVM.Cast = new Cast();
            castVM.ActorNames = new SelectList(_context.Actor, "Id", "Name");
            castVM.MovieNames = new SelectList(_context.Movie, "Id", "Title");
            return View(castVM);
        }

        // POST: Casts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieId,ActorId,CharacterName")] Cast cast)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(cast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CastVM castVM = new CastVM();
            castVM.Cast = cast;
            castVM.ActorNames = new SelectList(_context.Actor, "Id", "Name", cast.ActorId);
            castVM.MovieNames = new SelectList(_context.Movie, "Id", "Title", cast.MovieId);
            return View(castVM);
        }

        // GET: Casts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cast == null)
            {
                return NotFound();
            }

            var cast = await _context.Cast.FindAsync(id);
            if (cast == null)
            {
                return NotFound();
            }

            CastVM castVM = new CastVM();
            castVM.Cast = cast;
            castVM.ActorNames = new SelectList(_context.Actor, "Id", "Name");
            castVM.MovieNames = new SelectList(_context.Movie, "Id", "Title");
            return View(castVM);
        }

        // POST: Casts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,ActorId,CharacterName")] Cast cast)
        {
            if (id != cast.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CastExists(cast.Id))
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

            CastVM castVM = new CastVM();
            castVM.Cast = cast;
            castVM.ActorNames = new SelectList(_context.Actor, "Id", "Name", cast.ActorId);
            castVM.MovieNames = new SelectList(_context.Movie, "Id", "Title", cast.MovieId);
            return View(castVM);
        }

        // GET: Casts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cast == null)
            {
                return NotFound();
            }

            var cast = await _context.Cast
                .Include(c => c.Actor)
                .Include(c => c.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cast == null)
            {
                return NotFound();
            }

            CastVM castVM = new CastVM();
            castVM.Cast = cast;
            castVM.ActorNames = new SelectList(_context.Actor, "Id", "Name", cast.ActorId);
            castVM.MovieNames = new SelectList(_context.Movie, "Id", "Title", cast.MovieId);
            return View(castVM);
        }

        // POST: Casts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cast == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cast'  is null.");
            }
            var cast = await _context.Cast.FindAsync(id);
            if (cast != null)
            {
                _context.Cast.Remove(cast);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CastExists(int id)
        {
          return _context.Cast.Any(e => e.Id == id);
        }
    }
}
