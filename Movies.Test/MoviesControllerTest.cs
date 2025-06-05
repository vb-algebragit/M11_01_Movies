using Movies.API.Controllers;
using Movies.Data.Models;
using Movies.Data.Repositories;
using Xunit.Sdk;

namespace Movies.Test
{
    public class MoviesControllerTest
    {
        private readonly AlgebramssqlhostH0124Context _context;
        private readonly MovieRepository _repository;
        private readonly MoviesController _controller;

        public MoviesControllerTest()
        {
            _context = new AlgebramssqlhostH0124Context();
            _repository = new MovieRepository(_context);
            _controller = new MoviesController(_repository);
        }

        [Fact]
        public void Test1()
        {

        }
    }
}