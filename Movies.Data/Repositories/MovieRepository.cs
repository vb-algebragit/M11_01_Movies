using Movies.Data.Interfaces;
using Movies.Data.Models;

namespace Movies.Data.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AlgebramssqlhostH0124Context _context;

        public MovieRepository(AlgebramssqlhostH0124Context context)
        {
            _context = context;
        }

        public IEnumerable<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public Movie GetMovieById(int id)
        {
            return _context.Movies.FirstOrDefault(m => m.Id == id);
        }

        public Movie InsertMovie(Movie movie)
        {
            var newMovie = _context.Movies.Add(movie);
            _context.SaveChanges();

            return newMovie.Entity;
        }

        public Movie UpdateMovie(Movie movie)
        {
            var result = _context.Movies.FirstOrDefault(m => m.Id == movie.Id);

            if (result != null)
            {
                result.Title = movie.Title;
                result.Genre = movie.Genre;
                result.ReleaseYear = movie.ReleaseYear;

                _context.SaveChanges();

                return result;
            }

            return null;
        }

        public Movie DeleteMovie(int id)
        {
            var result = _context.Movies.FirstOrDefault(m => m.Id == id);

            if (result != null)
            {
                _context.Movies.Remove(result);
                _context.SaveChanges();

                return result;
            }

            return null;
        }

        public IEnumerable<Movie> QueryStringFilter(string filter, string orderBy, int perPage)
        {
            var movies = _context.Movies.ToList();

            if (!string.IsNullOrEmpty(filter))
            {
                movies = movies.Where(m => m.Title.Contains(filter, StringComparison.CurrentCultureIgnoreCase) 
                                        || m.Genre.Contains(filter, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "asc":
                        movies = movies.OrderBy(m => m.Id).ToList();
                        break;
                    case "desc":
                        movies = movies.OrderByDescending(m => m.Id).ToList();
                        break;
                }
            }

            if (perPage > 0)
            {
                movies = movies.Take(perPage).ToList();
            }

            return movies;
        }

    }
}
