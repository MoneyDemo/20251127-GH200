using TodoApp.Models;

namespace TodoApp.Services;

public interface ITodoService
{
    List<TodoItem> GetAll();
    TodoItem? GetById(int id);
    TodoItem Create(TodoItem todo);
    bool Update(TodoItem todo);
    bool Delete(int id);
    bool ToggleComplete(int id);
}
