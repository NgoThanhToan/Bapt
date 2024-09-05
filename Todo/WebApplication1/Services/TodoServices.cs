using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models.Services

{
    public class TodoServices : ControllerBase, ITodoService
    {
        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
      new TodoItemDTO
      {
          Id = todoItem.Id,
          Name = todoItem.Name,
          IsComplete = todoItem.IsComplete
      };
        private readonly TodoContext _context;
        public TodoServices(TodoContext context)
        { _context = context; }
        public async Task<IEnumerable<TodoItemDTO>> GetTodoItems()
        {
            return await _context.TodoItems
            .Select(x => ItemToDTO(x))
            .ToListAsync();
        }

        public async Task<TodoItemDTO> GetTodoById(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return null;
            }

            return ItemToDTO(todoItem);
        }

        public async Task<TodoItemDTO> CreateTodoById(TodoItemDTO CreateTodo)
        {
            var todoItem = new TodoItem
            {  Id = CreateTodo.Id,
                IsComplete = CreateTodo.IsComplete,
                Name = CreateTodo.Name
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return ItemToDTO(todoItem);
        }

        public async Task<bool> UpdateTodoById(long id, todoapi UpdateTodo)
        {
            
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return false;
            }

            todoItem.Name = UpdateTodo.Name;
            todoItem.IsComplete = UpdateTodo.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return false;
            }

            return false;
        }

        public async Task<bool> DeleteTodoById(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return false;
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return true ;
        }
        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
