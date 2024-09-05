using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Models.Services
{
 interface ITodoService
    {
        Task<IEnumerable<TodoItemDTO>> GetTodoItems();

        Task<TodoItemDTO> GetTodoById(long id);
        Task<TodoItemDTO> CreateTodoById(TodoItemDTO CreateTodo);
        Task<bool> UpdateTodoById(long id, todoapi todoDTO);
        Task<bool> DeleteTodoById(long id);

    }
}
