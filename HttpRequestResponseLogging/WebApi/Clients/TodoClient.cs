using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Clients
{
    public class TodoClient : ITodoClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        public TodoClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _baseUrl = string.Empty; // Base URL is set via HttpClient.BaseAddress
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            // Ensure the API key is set in the default headers
            if (!_httpClient.DefaultRequestHeaders.Contains("X-API-Key"))
            {
                throw new InvalidOperationException("API Key header is required");
            }
        }

        public async Task<IEnumerable<TodoItem>> GetTodosAsync()
        {
            var response = await _httpClient.GetAsync("todos");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<TodoItem>>(_jsonOptions);
        }

        public async Task<TodoItem> GetTodoAsync(int id)
        {
            var response = await _httpClient.GetAsync($"todos/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TodoItem>(_jsonOptions);
        }

        public async Task<TodoItem> CreateTodoAsync(TodoItem todoItem)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(todoItem, _jsonOptions),
                Encoding.UTF8,
                "application/json");
                
            var response = await _httpClient.PostAsync("todos", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TodoItem>(_jsonOptions);
        }

        public async Task<TodoItem> UpdateTodoAsync(int id, TodoItem todoItem)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(todoItem, _jsonOptions),
                Encoding.UTF8,
                "application/json");
                
            var response = await _httpClient.PutAsync($"todos/{id}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TodoItem>(_jsonOptions);
        }

        public async Task DeleteTodoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"todos/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}