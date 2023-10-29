using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PC3.DTO;
using PC3.Integrations;


namespace PC3.Controllers.UI
{
    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly JsonplaceholderAPIIntegration _jsonplaceholder;

        public PostController(ILogger<PostController> logger, JsonplaceholderAPIIntegration jsonplaceholder)
        {
            _logger = logger;
            _jsonplaceholder = jsonplaceholder;
        }

        public async Task<IActionResult> Index()
        {

            List<PostDTO> posts =await _jsonplaceholder.GetAll();

            //List<TodoDTO> filtro = todos.Where(todo => todo.userId > 6).ToList();

            //List<TodoDTO> filtro = todos.Where(todo => todo.title.Contains("Tarea")).ToList();

            List<PostDTO> filtro = posts
            .Where(post => post.userId == 1)
            .OrderBy(post => post.title)
            .ThenByDescending(post => post.body)
            .ToList();

            return View(posts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}