using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Tests.Services;

public class TodoServiceTests
{
    private TodoService CreateService()
    {
        return new TodoService();
    }

    #region GetAll Tests

    [Fact]
    public void GetAll_ReturnsAllTodos()
    {
        // Arrange
        var service = CreateService();

        // Act
        var todos = service.GetAll();

        // Assert
        Assert.NotNull(todos);
        Assert.True(todos.Count >= 3); // At least the initial seeded items
    }

    [Fact]
    public void GetAll_ReturnsTodosOrderedByCreatedAtDescending()
    {
        // Arrange
        var service = CreateService();

        // Act
        var todos = service.GetAll();

        // Assert
        for (int i = 0; i < todos.Count - 1; i++)
        {
            Assert.True(todos[i].CreatedAt >= todos[i + 1].CreatedAt);
        }
    }

    #endregion

    #region GetById Tests

    [Fact]
    public void GetById_WithValidId_ReturnsTodoItem()
    {
        // Arrange
        var service = CreateService();

        // Act
        var todo = service.GetById(1);

        // Assert
        Assert.NotNull(todo);
        Assert.Equal(1, todo.Id);
    }

    [Fact]
    public void GetById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var service = CreateService();

        // Act
        var todo = service.GetById(9999);

        // Assert
        Assert.Null(todo);
    }

    #endregion

    #region Create Tests

    [Fact]
    public void Create_AddsNewTodoItem()
    {
        // Arrange
        var service = CreateService();
        var newTodo = new TodoItem
        {
            Title = "Test Todo",
            Description = "Test Description",
            IsCompleted = false
        };
        var initialCount = service.GetAll().Count;

        // Act
        var createdTodo = service.Create(newTodo);

        // Assert
        Assert.NotNull(createdTodo);
        Assert.True(createdTodo.Id > 0);
        Assert.Equal("Test Todo", createdTodo.Title);
        Assert.Equal(service.GetAll().Count, initialCount + 1);
    }

    [Fact]
    public void Create_AssignsNewId()
    {
        // Arrange
        var service = CreateService();
        var newTodo = new TodoItem { Title = "New Todo" };

        // Act
        var createdTodo = service.Create(newTodo);

        // Assert
        Assert.True(createdTodo.Id > 0);
    }

    [Fact]
    public void Create_SetsCreatedAtToNow()
    {
        // Arrange
        var service = CreateService();
        var newTodo = new TodoItem { Title = "New Todo" };
        var beforeCreation = DateTime.Now;

        // Act
        var createdTodo = service.Create(newTodo);
        var afterCreation = DateTime.Now;

        // Assert
        Assert.True(createdTodo.CreatedAt >= beforeCreation.AddSeconds(-1));
        Assert.True(createdTodo.CreatedAt <= afterCreation.AddSeconds(1));
    }

    #endregion

    #region Update Tests

    [Fact]
    public void Update_WithValidId_UpdatesTodoAndReturnsTrue()
    {
        // Arrange
        var service = CreateService();
        var existingTodo = service.GetById(1)!;
        var updatedTodo = new TodoItem
        {
            Id = 1,
            Title = "Updated Title",
            Description = "Updated Description",
            IsCompleted = true
        };

        // Act
        var result = service.Update(updatedTodo);
        var retrievedTodo = service.GetById(1);

        // Assert
        Assert.True(result);
        Assert.NotNull(retrievedTodo);
        Assert.Equal("Updated Title", retrievedTodo.Title);
        Assert.Equal("Updated Description", retrievedTodo.Description);
        Assert.True(retrievedTodo.IsCompleted);
    }

    [Fact]
    public void Update_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var service = CreateService();
        var invalidTodo = new TodoItem
        {
            Id = 9999,
            Title = "Non-existent"
        };

        // Act
        var result = service.Update(invalidTodo);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public void Delete_WithValidId_RemovesTodoAndReturnsTrue()
    {
        // Arrange
        var service = CreateService();
        var newTodo = service.Create(new TodoItem { Title = "To Be Deleted" });
        var initialCount = service.GetAll().Count;

        // Act
        var result = service.Delete(newTodo.Id);
        var deletedTodo = service.GetById(newTodo.Id);

        // Assert
        Assert.True(result);
        Assert.Null(deletedTodo);
        Assert.Equal(initialCount - 1, service.GetAll().Count);
    }

    [Fact]
    public void Delete_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = service.Delete(9999);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region ToggleComplete Tests

    [Fact]
    public void ToggleComplete_WithValidId_TogglesCompletionStatus()
    {
        // Arrange
        var service = CreateService();
        var todo = service.GetById(1)!;
        var originalStatus = todo.IsCompleted;

        // Act
        var result = service.ToggleComplete(1);
        var updatedTodo = service.GetById(1)!;

        // Assert
        Assert.True(result);
        Assert.NotEqual(originalStatus, updatedTodo.IsCompleted);
    }

    [Fact]
    public void ToggleComplete_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = service.ToggleComplete(9999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ToggleComplete_TogglesFromFalseToTrue()
    {
        // Arrange
        var service = CreateService();
        var newTodo = service.Create(new TodoItem { Title = "Toggle Test", IsCompleted = false });
        // The newly created todo should have IsCompleted = false by default after being added

        // Act
        service.ToggleComplete(newTodo.Id);
        var toggledTodo = service.GetById(newTodo.Id)!;

        // Assert
        Assert.True(toggledTodo.IsCompleted);
    }

    [Fact]
    public void ToggleComplete_TogglesFromTrueToFalse()
    {
        // Arrange
        var service = CreateService();
        // ID 2 is seeded with IsCompleted = true
        var todo = service.GetById(2)!;
        Assert.True(todo.IsCompleted);

        // Act
        service.ToggleComplete(2);
        var toggledTodo = service.GetById(2)!;

        // Assert
        Assert.False(toggledTodo.IsCompleted);
    }

    #endregion
}
