using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;
using webapi.Models.DTO;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly KinoContext _context;

        public MoviesController(KinoContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            return await _context.Movies.Select(x => ConvertToMovieDTO(x)).ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return ConvertToMovieDTO(movie);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieDTO movieDTO)
        {
            if (id != movieDTO.Id)
            {
                return BadRequest();
            }

            Movie movie = ConvertToMovie(movieDTO);

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(MovieDTO movieDTO)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'KinoContext.Movies'  is null.");
            }

            Movie movie = ConvertToMovie(movieDTO);

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movieDTO.Id }, movieDTO);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static MovieDTO ConvertToMovieDTO(Movie movie)
        {
            return new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                Budget = movie.Budget,
                Overview = movie.Overview,
                Popularity = movie.Popularity,
                ReleaseDate = movie.ReleaseDate,
                Revenue = movie.Revenue,
                Runtime = movie.Runtime,
                MovieStatus = movie.MovieStatus,
                VoteAverage = movie.VoteAverage,
                VoteCount = movie.VoteCount,
                AgeRating = movie.AgeRating,
                MediaSource = movie.MediaSource
            };
        }

        private static Movie ConvertToMovie(MovieDTO movieDTO)
        {
            return new Movie
            {
                Id = movieDTO.Id,
                Title = movieDTO.Title,
                Budget = movieDTO.Budget,
                Overview = movieDTO.Overview,
                Popularity = movieDTO.Popularity,
                ReleaseDate = movieDTO.ReleaseDate,
                Revenue = movieDTO.Revenue,
                Runtime = movieDTO.Runtime,
                MovieStatus = movieDTO.MovieStatus,
                VoteAverage = movieDTO.VoteAverage,
                VoteCount = movieDTO.VoteCount,
                AgeRating = movieDTO.AgeRating,
                MediaSource = movieDTO.MediaSource
            };
        }
    }
}
