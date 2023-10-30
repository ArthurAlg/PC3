using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PC3.DTO;
using PC3.Integrations;

namespace PC3.Controllers.UI
{
    public class PostController : Controller
    {
        private readonly JsonplaceholderAPIIntegration _jsonplaceholder;

        public PostController(JsonplaceholderAPIIntegration jsonplaceholder)
        {
            _jsonplaceholder = jsonplaceholder;
        }

        public async Task<IActionResult> Listar()
        {
            List<PostDTO> posts = await _jsonplaceholder.GetAll();
            return View(posts);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(PostDTO post)
        {
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(post);

                // Preparar la solicitud HTTP
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://jsonplaceholder.typicode.com/posts", content);

                if (response.IsSuccessStatusCode)
                {
                    // La creación fue exitosa, procesa la respuesta de la API
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var newPost = JsonConvert.DeserializeObject<PostDTO>(responseContent);

                    // Puedes redirigir a la vista "Listar" u otra acción después de la creación
                    return RedirectToAction("Listar");
                }
                else
                {
                    // La creación falló, maneja el error de acuerdo a tus necesidades
                    ModelState.AddModelError(string.Empty, "Error en la creación del recurso en la API.");
                }
            }

            return View(post);
        }

        public async Task<IActionResult> Editar(int id)
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Editar(PostDTO post)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Listar");
            }

            return View(post);
        }

        public async Task<IActionResult> Ver(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _jsonplaceholder.GetPostById(id.Value);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        public async Task<IActionResult> Eliminar(int id)
        {

            return RedirectToAction("Listar");
        }
    }
}