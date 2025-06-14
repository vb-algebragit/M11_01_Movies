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

        [Theory]
        [InlineData(2, 55)]
        public void GetMovieById_ReturnsOkObjectResult(int id1, int id2)
        {
            // Arrange
            // Act
            var okResult = _controller.GetMovie(id1);
            var notFoundResult = _controller.GetMovie(id2);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
            Assert.IsType<OkObjectResult>(okResult.Result);

            var item = okResult.Result as OkObjectResult;

            Assert.IsType<Movie>(item.Value);

            var movieItem = item.Value as Movie;
            Assert.Equal(id1, movieItem.Id);
            Assert.Equal("Die Hard", movieItem.Title);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var newMovie = new Movie()
            {
                Id = 0,
                Title = "The Godfather II",
                Genre = "Crime, Drama",
                ReleaseYear = "1978"
            };

            // Act
            var createdResponse = _controller.PostMovie(newMovie);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var titleAndIdMissingMovie = new Movie()
            {
                Genre = "Crime-Drama",
                ReleaseYear = "1972"
            };

            // Act
            _controller.ModelState.AddModelError("Id", "Id is a requried filed");
            _controller.ModelState.AddModelError("Title", "Title is a requried filed");

            var badResponse = _controller.PostMovie(titleAndIdMissingMovie);

            // Assert
            Assert.IsType<BadRequestResult>(badResponse);
        }

        [Theory]
        [InlineData(8)]
        public void Remove_GetExistingMovieById_ReturnsOkObjectResult(int id)
        {
            // Arrange
            // Act
            var okResult = _controller.DeleteMovie(id);

            // Assert
            var result = _controller.GetMovies();
            Assert.IsType<OkObjectResult>(result.Result);
            var list = result.Result as OkObjectResult;
            Assert.IsType<List<Movie>>(list.Value);
            var listMovies = list.Value as List<Movie>;

            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(4, listMovies.Count());
        }

        [Theory]
        [InlineData(5000)]
        public void Remove_GetNonExitingMovieById_ReturnsNotFoundResult(int id)
        {
            // Arrange
            // Act
            var notFoundResult = _controller.DeleteMovie(id);

            // Assert
            var result = _controller.GetMovies();
            Assert.IsType<OkObjectResult>(result.Result);
            var list = result.Result as OkObjectResult;
            Assert.IsType<List<Movie>>(list.Value);
            var listMovies = list.Value as List<Movie>;

            Assert.IsType<NotFoundObjectResult>(notFoundResult);
            Assert.Equal(4, listMovies.Count());
        }

    }
}