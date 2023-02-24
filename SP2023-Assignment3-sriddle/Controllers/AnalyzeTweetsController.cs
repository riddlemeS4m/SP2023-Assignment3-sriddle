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
    public class AnalyzeTweetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalyzeTweetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AnalyzeTweets
        public async Task<IActionResult> Index()
        {
              return View(await _context.AnalyzeTweet.ToListAsync());
        }

        // GET: AnalyzeTweets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AnalyzeTweet == null)
            {
                return NotFound();
            }

            var analyzeTweet = await _context.AnalyzeTweet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analyzeTweet == null)
            {
                return NotFound();
            }

            return View(analyzeTweet);
        }

        // GET: AnalyzeTweets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnalyzeTweets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tweet,Sentiment")] AnalyzeTweet analyzeTweet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(analyzeTweet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(analyzeTweet);
        }

        // GET: AnalyzeTweets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AnalyzeTweet == null)
            {
                return NotFound();
            }

            var analyzeTweet = await _context.AnalyzeTweet.FindAsync(id);
            if (analyzeTweet == null)
            {
                return NotFound();
            }
            return View(analyzeTweet);
        }

        // POST: AnalyzeTweets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tweet,Sentiment")] AnalyzeTweet analyzeTweet)
        {
            if (id != analyzeTweet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(analyzeTweet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnalyzeTweetExists(analyzeTweet.Id))
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
            return View(analyzeTweet);
        }

        // GET: AnalyzeTweets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AnalyzeTweet == null)
            {
                return NotFound();
            }

            var analyzeTweet = await _context.AnalyzeTweet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (analyzeTweet == null)
            {
                return NotFound();
            }

            return View(analyzeTweet);
        }

        // POST: AnalyzeTweets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AnalyzeTweet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AnalyzeTweet'  is null.");
            }
            var analyzeTweet = await _context.AnalyzeTweet.FindAsync(id);
            if (analyzeTweet != null)
            {
                _context.AnalyzeTweet.Remove(analyzeTweet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnalyzeTweetExists(int id)
        {
          return _context.AnalyzeTweet.Any(e => e.Id == id);
        }
    }
}
