using Microsoft.AspNetCore.Mvc;
using Movies.Client.Models;

namespace Movies.Client.Controllers
{
    public class MoviesController : Controller
    {
        private readonly HttpClient _httpClient;

        public MoviesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7268/");
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var movies = await _httpClient.GetFromJsonAsync<List<Movie>>("api/Movies");

            return View(movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _httpClient.GetFromJsonAsync<Movie>($"api/Movies/{id}");

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Movies", movie);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _httpClient.GetFromJsonAsync<Movie>($"api/Movies/{id}");

            return View(movie);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Movies/{id}", movie);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }
    }
}
