using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApiDTO.Controllers
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItemDTO>> GetTodoItems();
        Task<TodoItemDTO> GetTodoItem(long id);
        Task<TodoItemDTO> UpdateTodoItem(TodoItemDTO todoItemDTO);
        Task<TodoItemDTO> CreateTodoItem(TodoItemDTO todoItemDTO);
        Task DeleteTodoItem(long id);

    }
}
