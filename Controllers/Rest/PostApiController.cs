using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc; 
using System.Threading.Tasks;     
using System.Collections.Generic;
using PC3.DTO;
using PC3.Integrations;

namespace PC3.Controllers.Rest{
    [Route("api/posts")]
    [ApiController]
    public class PostApiController : ControllerBase{
        private readonly JsonplaceholderAPIIntegration _jsonplaceholder;
        private readonly ILogger<PostApiController> _logger;

        public PostApiController(JsonplaceholderAPIIntegration jsonplaceholder, ILogger<PostApiController> logger){
            _jsonplaceholder = jsonplaceholder;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Listar(){
            List<PostDTO> posts = await _jsonplaceholder.GetAll();
            return Ok(posts);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Ver(int id){
            var post = await _jsonplaceholder.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] PostDTO post){
            if (post == null)
            {
                return BadRequest();
            }
            return Ok(post);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id){

            return NoContent(); 
        }
    }
}