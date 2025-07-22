using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Clients
{
    public interface ITodoClient
    {
        Task<IEnumerable<TodoItem>> GetTodosAsync();
        Task<TodoItem> GetTodoAsync(int id);
        Task<TodoItem> CreateTodoAsync(TodoItem todoItem);
        Task<TodoItem> UpdateTodoAsync(int id, TodoItem todoItem);
        Task DeleteTodoAsync(int id);
    }
}