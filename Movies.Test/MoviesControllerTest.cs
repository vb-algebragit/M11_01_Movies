using Microsoft.AspNetCore.Mvc;
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
        public void GetAllMovies_ReturnSuccessIfCorrectCount()
        {
            // Arrange
            // Act
            var result = _controller.GetMovies();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var list = result.Result as OkObjectResult;

            Assert.IsType<List<Movie>>(list.Value);

            var listMovies = list.Value as List<Movie>;

            Assert.Equal(4, listMovies.Count);
        }

        [Fact]
        public void GetAllMovies_ReturnSuccessIfWrongCount()
        {
            // Arrange
            // Act
            var result = _controller.GetMovies();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var list = result.Result as OkObjectResult;

            Assert.IsType<List<Movie>>(list.Value);

            var listMovies = list.Value as List<Movie>;

            Assert.NotEqual(5, listMovies.Count);
        }
    }
}