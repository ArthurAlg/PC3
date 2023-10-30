using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using PC3.DTO;
using PC3.Integrations;

namespace PC3.Controllers.UI
{
    public class PostController : Controller
    {
        private readonly JsonplaceholderAPIIntegration _jsonplaceholder;
        private readonly HttpClient httpClient;
        public PostController(JsonplaceholderAPIIntegration jsonplaceholder){
            _jsonplaceholder = jsonplaceholder;
            httpClient = new HttpClient();
        }
        public async Task<IActionResult> Listar(){
            List<PostDTO> posts = await _jsonplaceholder.GetAll();
            return View(posts);
        }
        public IActionResult Crear(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(PostDTO post){
            if (ModelState.IsValid){
                
                    return RedirectToAction("Listar");
                }
                else{
                    ModelState.AddModelError(string.Empty, "Error en la creación del recurso en la API.");
                }
            return View(post);
        }
        public async Task<IActionResult> Editar(int id){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Editar(PostDTO post){
            if (ModelState.IsValid){
                return RedirectToAction("Listar");
            }
            return View(post);
        }
        public async Task<IActionResult> Ver(int? id){
            if (id == null){
                return NotFound();
            }
            var post = await _jsonplaceholder.GetPostById(id.Value);
            if (post == null){
                return NotFound();
            }
            return View(post);
        }
        public async Task<IActionResult> Eliminar(int id){
            return RedirectToAction("Listar");
        }
    }
}