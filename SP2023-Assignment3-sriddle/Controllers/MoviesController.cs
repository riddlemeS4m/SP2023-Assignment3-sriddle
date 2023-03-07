using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SP2023_Assignment3_sriddle.Data;
using SP2023_Assignment3_sriddle.Models;
using Tweetinvi;
using VaderSharp2;

namespace SP2023_Assignment3_sriddle.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
              return View(await _context.Movie.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            MovieCastVM castsVM = new MovieCastVM();
            castsVM.Movie = movie;
            //castsVM.Casts = _context.Cast.Where(c => c.MovieId == id).ToList();
            castsVM.Casts = _context.Cast.Where(c => c.MovieId == id).Include(c => c.Actor).ToList();

            var userClient = new TwitterClient("AAx9UfdCemph0Pg0t8Moq5c6L", "LbhoERpFGjBESYSNjTHuRvE0R80cGxZBx5lJWanM5lFpO2Hs63", "1455230009153503238-WTxQgoYUAQ3D9PTSsUu8stHkmJvuVe", "2ZVnM9tWbCSNAhyJcyC4WPIgiIbUWZ77MTLSx2Qb8TkW3");
            var searchResponse = await userClient.SearchV2.SearchTweetsAsync(movie.Title);
            var tweets = searchResponse.Tweets;
            var analyzer = new SentimentIntensityAnalyzer();

            double tweetTotal = 0;
            int tweetCount = 0;
            List<AnalyzeTweet> analyzeTweets = new List<AnalyzeTweet>();

            for (int i = 0; i < tweets.Length; i++)
            {
                var results = analyzer.PolarityScores(tweets[i].Text);
                tweetTotal += results.Compound;

                if (results.Compound != 0)
                {
                    tweetCount += 1;
                }

                analyzeTweets.Add(new AnalyzeTweet
                {
                    Tweet = tweets[i].Text,
                    Sentiment = Math.Round(results.Compound, 3).ToString()
                });
            }

            TweetsVM tweetsVM = new TweetsVM();
            tweetsVM.Name = movie.Title;
            tweetsVM.Average = Math.Round(tweetTotal / tweetCount, 3);
            tweetsVM.Tweets = analyzeTweets;

            castsVM.TweetsVM = tweetsVM;


            return View(castsVM);
        }

        public async Task<IActionResult> Analyze(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            var userClient = new TwitterClient("AAx9UfdCemph0Pg0t8Moq5c6L", "LbhoERpFGjBESYSNjTHuRvE0R80cGxZBx5lJWanM5lFpO2Hs63", "1455230009153503238-WTxQgoYUAQ3D9PTSsUu8stHkmJvuVe", "2ZVnM9tWbCSNAhyJcyC4WPIgiIbUWZ77MTLSx2Qb8TkW3");
            var searchResponse = await userClient.SearchV2.SearchTweetsAsync(movie.Title);
            var tweets = searchResponse.Tweets;
            var analyzer = new SentimentIntensityAnalyzer();

            double tweetTotal = 0;
            int tweetCount = 0;
            List<AnalyzeTweet> analyzeTweets = new List<AnalyzeTweet>();

            for (int i = 0; i < tweets.Length; i++)
            {
                var results = analyzer.PolarityScores(tweets[i].Text);
                tweetTotal += results.Compound;

                if(results.Compound != 0)
                {
                    tweetCount += 1;
                }

                analyzeTweets.Add(new AnalyzeTweet
                {
                    Tweet = tweets[i].Text,
                    Sentiment = Math.Round(results.Compound,3).ToString()
                });
            }

            TweetsVM tweetsVM = new TweetsVM();
            tweetsVM.Name = movie.Title;
            tweetsVM.Average = Math.Round(tweetTotal / tweetCount,3);
            tweetsVM.Tweets = analyzeTweets;

            return View(tweetsVM);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,IMDBLink,Genre,ReleaseYear")] Movie movie, IFormFile Poster)
        {
            if (ModelState.IsValid)
            {
                if (Poster != null && Poster.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Poster.CopyToAsync(memoryStream);
                        movie.Poster = memoryStream.ToArray();
                    }
                }
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        public async Task<IActionResult> GetMoviePoster(int id)
        {
            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            var imageData = movie.Poster;

            return File(imageData, "image/jpg");
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IMDBLink,Genre,ReleaseYear")] Movie movie, IFormFile Poster)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (Poster != null && Poster.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Poster.CopyToAsync(memoryStream);
                    movie.Poster = memoryStream.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return _context.Movie.Any(e => e.Id == id);
        }
    }
}
