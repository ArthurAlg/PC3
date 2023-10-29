using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using PC3.DTO;

namespace PC3.Integrations
{
    public class JsonplaceholderAPIIntegration
    {
        private readonly ILogger<JsonplaceholderAPIIntegration> _logger;
        private const string API_URL="https://jsonplaceholder.typicode.com/posts";
        private readonly HttpClient httpClient;

        public JsonplaceholderAPIIntegration(ILogger<JsonplaceholderAPIIntegration> logger){
            _logger = logger;
            httpClient = new HttpClient();
        }

        public async Task<List<PostDTO>> GetAll(){
            string requestUrl = $"{API_URL}";
            List<PostDTO> listado = new List<PostDTO>();
            try{
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    listado =  await response.Content.ReadFromJsonAsync<List<PostDTO>>() ?? new List<PostDTO>();
                }
            }catch(Exception ex){
                _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
            }
            return listado;
        }

        public async Task<PostDTO> CreatePost(PostDTO post)
        {
            string requestUrl = $"{API_URL}";
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(requestUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var createdPostContent = await response.Content.ReadAsStringAsync();
                    var createdPost = JsonSerializer.Deserialize<PostDTO>(createdPostContent);
                    return createdPost;
                }
                else
                {
                    _logger.LogError($"Error al crear un post. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al llamar a la API: {ex.Message}");
            }

            return null;
        }

        public async Task<PostDTO> GetPostById(int id)
        {
            var postList = await GetAll();
            return postList.FirstOrDefault(post => post.id == id);
        }
    }
}