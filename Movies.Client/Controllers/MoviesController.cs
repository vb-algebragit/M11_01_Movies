using Microsoft.AspNetCore.Mvc;
using Movies.Client.Models;
using Newtonsoft.Json;

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

            ViewBag.Filter = "";
            ViewBag.OrderBy = "asc";
            ViewBag.PerPage = 0;

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

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _httpClient.GetFromJsonAsync<Movie>($"api/Movies/{id}");

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Movies/{id}");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string filter, string orderBy = "asc", int perPage = 0)
        {
            var query = $"api/Movies/search?filter={filter}&orderBy={orderBy}&perPage={perPage}";
            var movies = await _httpClient.GetFromJsonAsync<List<Movie>>(query);

            ViewBag.Filter = filter;
            ViewBag.OrderBy = orderBy;
            ViewBag.PerPage = perPage;

            return View(nameof(Index), movies);
        }

        public async Task<IActionResult> Export()
        {
            var response = await _httpClient.GetAsync("api/Movies");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to fetch movies.");
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var movies = JsonConvert.DeserializeObject<List<Movie>>(jsonString);

            var formattedJson = JsonConvert.SerializeObject(movies, Formatting.Indented);

            var exportPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "exports");
            Directory.CreateDirectory(exportPath);

            var fileName = $"movies_export_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var fullPath = Path.Combine(exportPath, fileName);

            await System.IO.File.WriteAllTextAsync(fullPath, formattedJson);

            TempData["ExportSuccess"] = $"Exported to /exports/{fileName}";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ExportFile()
        {
            var response = await _httpClient.GetAsync("api/Movies");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to fetch movies.");
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var movies = JsonConvert.DeserializeObject<List<Movie>>(jsonString);

            var formattedJson = JsonConvert.SerializeObject(movies, Formatting.Indented);
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(formattedJson);

            var fileName = $"movies_export_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            return File(fileBytes, "application/json", fileName);
        }


    }
}
