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

        public async Task<IActionResult> Listar()
        {

            List<PostDTO> posts =await _jsonplaceholder.GetAll();

            List<PostDTO> filtro = posts
            .Where(post => post.userId == 1)
            .OrderBy(post => post.title)
            .ThenByDescending(post => post.body)
            .ToList();

            return View(posts);
        }

        [HttpPost]
        public IActionResult Crear()
        {
            return View();
        }

        public async Task<IActionResult> Ver(int? id)
        {
            if (id == null){
                return NotFound();
            }

            var post = await _jsonplaceholder.GetPostById(id.Value);

            if (post == null){
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("userId,title,body")] PostDTO post)
        {
            if (ModelState.IsValid)
            {
                var createdPost = await _jsonplaceholder.CreatePost(post);

                if (createdPost != null)
                {
                    // Puedes redirigir a la página de detalles del nuevo post o a donde sea necesario.
                    return RedirectToAction(nameof(Listar));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear el post.");
                }
            }

            return View(post);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}