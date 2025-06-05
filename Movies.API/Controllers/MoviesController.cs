using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Data.Interfaces;
using Movies.Data.Models;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        //private readonly AlgebramssqlhostH0124Context _context;
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // GET: api/Movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            try
            {
                return Ok(_movieRepository.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public ActionResult<Movie> GetMovie(int id)
        {
            try
            {
                var result = _movieRepository.GetMovieById(id);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult PostMovie([FromBody]Movie movie)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                
                var createdMovie = _movieRepository.InsertMovie(movie);

                return CreatedAtAction(nameof(GetMovie), new { id = createdMovie.Id }, createdMovie);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new movie");
            }
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult PutMovie(int id, Movie movie)
        {
            try
            {
                if (id != movie.Id)
                {
                    return BadRequest("Id missmatch!");
                }

                var updatedMovie = _movieRepository.GetMovieById(id);
                if (updatedMovie == null)
                {
                    return NotFound($"Movie with Id = {id} not found");
                }

                return Ok(_movieRepository.UpdateMovie(movie));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating movie");
            }
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public ActionResult DeleteMovie(int id)
        {
            try
            {
                var movieToDelete = _movieRepository.GetMovieById(id);

                if (movieToDelete == null)
                {
                    return NotFound($"Movie with Id = {id} not found");
                }

                return Ok(_movieRepository.DeleteMovie(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting movie");
            }
        }

        // api/movies/search?filter=umri&orderBy=desc&perPage=10
        [HttpGet("search")]
        public ActionResult SearchByQueryString([FromQuery]string filter, [FromQuery]string orderBy = "asc", [FromQuery]int perPage = 0)
        {
            try
            {
                var movies = _movieRepository.QueryStringFilter(filter, orderBy, perPage);

                return Ok(movies);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        //// GET: api/Movies
        //[HttpGet]
        //private async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        //{
        //    return await _context.Movies.ToListAsync();
        //}

        //// GET: api/Movies/5
        //[HttpGet("{id}")]
        //private async Task<ActionResult<Movie>> GetMovie(int id)
        //{
        //    var movie = await _context.Movies.FindAsync(id);

        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }

        //    return movie;
        //}

        //// PUT: api/Movies/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //private async Task<IActionResult> PutMovie(int id, Movie movie)
        //{
        //    if (id != movie.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(movie).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MovieExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Movies
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //private async Task<ActionResult<Movie>> PostMovie(Movie movie)
        //{
        //    _context.Movies.Add(movie);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        //}

        //// DELETE: api/Movies/5
        //[HttpDelete("{id}")]
        //private async Task<IActionResult> DeleteMovie(int id)
        //{
        //    var movie = await _context.Movies.FindAsync(id);
        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Movies.Remove(movie);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool MovieExists(int id)
        //{
        //    return _context.Movies.Any(e => e.Id == id);
        //}
    }
}
