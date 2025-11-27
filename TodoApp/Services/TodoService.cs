using TodoApp.Models;

namespace TodoApp.Services;

public class TodoService
{
    private static readonly List<TodoItem> _todos = new()
    {
        new TodoItem { Id = 1, Title = "Learn .NET 10", Description = "Explore new features in .NET 10", IsCompleted = false, CreatedAt = DateTime.Now.AddDays(-2) },
        new TodoItem { Id = 2, Title = "Build TODO App", Description = "Create a sample MVC TODO application", IsCompleted = true, CreatedAt = DateTime.Now.AddDays(-1) },
        new TodoItem { Id = 3, Title = "Write Documentation", Description = "Document the TODO application features", IsCompleted = false, CreatedAt = DateTime.Now }
    };

    private static int _nextId = 4;

    public List<TodoItem> GetAll()
    {
        return _todos.OrderByDescending(t => t.CreatedAt).ToList();
    }

    public TodoItem? GetById(int id)
    {
        return _todos.FirstOrDefault(t => t.Id == id);
    }

    public TodoItem Create(TodoItem todo)
    {
        todo.Id = _nextId++;
        todo.CreatedAt = DateTime.Now;
        _todos.Add(todo);
        return todo;
    }

    public bool Update(TodoItem todo)
    {
        var existingTodo = _todos.FirstOrDefault(t => t.Id == todo.Id);
        if (existingTodo == null)
        {
            return false;
        }

        existingTodo.Title = todo.Title;
        existingTodo.Description = todo.Description;
        existingTodo.IsCompleted = todo.IsCompleted;
        return true;
    }

    public bool Delete(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return false;
        }

        _todos.Remove(todo);
        return true;
    }

    public bool ToggleComplete(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return false;
        }

        todo.IsCompleted = !todo.IsCompleted;
        return true;
    }
}
