using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using TodoApp.Controllers;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Tests.Controllers;

public class TodoControllerTests
{
    private readonly Mock<ITodoService> _mockTodoService;
    private readonly TodoController _controller;

    public TodoControllerTests()
    {
        _mockTodoService = new Mock<ITodoService>();
        _controller = new TodoController(_mockTodoService.Object);

        // Setup HttpContext for antiforgery token
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        _controller.TempData = tempData;
    }

    #region Index Tests

    [Fact]
    public void Index_ReturnsViewWithTodos()
    {
        // Arrange
        var todos = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Test 1" },
            new TodoItem { Id = 2, Title = "Test 2" }
        };
        _mockTodoService.Setup(s => s.GetAll()).Returns(todos);

        // Act
        var result = _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as List<TodoItem>;
        Assert.NotNull(model);
        Assert.Equal(2, model.Count);
    }

    #endregion

    #region Details Tests

    [Fact]
    public void Details_WithValidId_ReturnsViewWithTodo()
    {
        // Arrange
        var todo = new TodoItem { Id = 1, Title = "Test Todo" };
        _mockTodoService.Setup(s => s.GetById(1)).Returns(todo);

        // Act
        var result = _controller.Details(1) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as TodoItem;
        Assert.NotNull(model);
        Assert.Equal(1, model.Id);
        Assert.Equal("Test Todo", model.Title);
    }

    [Fact]
    public void Details_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockTodoService.Setup(s => s.GetById(999)).Returns((TodoItem?)null);

        // Act
        var result = _controller.Details(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    #endregion

    #region Create GET Tests

    [Fact]
    public void Create_Get_ReturnsView()
    {
        // Act
        var result = _controller.Create() as ViewResult;

        // Assert
        Assert.NotNull(result);
    }

    #endregion

    #region Create POST Tests

    [Fact]
    public void Create_Post_WithValidModel_RedirectsToIndex()
    {
        // Arrange
        var todo = new TodoItem { Title = "New Todo", Description = "Description" };
        _mockTodoService.Setup(s => s.Create(It.IsAny<TodoItem>())).Returns(todo);

        // Act
        var result = _controller.Create(todo) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }

    [Fact]
    public void Create_Post_WithValidModel_CallsServiceCreate()
    {
        // Arrange
        var todo = new TodoItem { Title = "New Todo" };
        _mockTodoService.Setup(s => s.Create(It.IsAny<TodoItem>())).Returns(todo);

        // Act
        _controller.Create(todo);

        // Assert
        _mockTodoService.Verify(s => s.Create(It.Is<TodoItem>(t => t.Title == "New Todo")), Times.Once);
    }

    [Fact]
    public void Create_Post_WithInvalidModel_ReturnsViewWithModel()
    {
        // Arrange
        var todo = new TodoItem { Title = "" };
        _controller.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = _controller.Create(todo) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as TodoItem;
        Assert.NotNull(model);
    }

    #endregion

    #region Edit GET Tests

    [Fact]
    public void Edit_Get_WithValidId_ReturnsViewWithTodo()
    {
        // Arrange
        var todo = new TodoItem { Id = 1, Title = "Test Todo" };
        _mockTodoService.Setup(s => s.GetById(1)).Returns(todo);

        // Act
        var result = _controller.Edit(1) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as TodoItem;
        Assert.NotNull(model);
        Assert.Equal(1, model.Id);
    }

    [Fact]
    public void Edit_Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockTodoService.Setup(s => s.GetById(999)).Returns((TodoItem?)null);

        // Act
        var result = _controller.Edit(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    #endregion

    #region Edit POST Tests

    [Fact]
    public void Edit_Post_WithValidModel_RedirectsToIndex()
    {
        // Arrange
        var todo = new TodoItem { Id = 1, Title = "Updated Todo" };
        _mockTodoService.Setup(s => s.Update(It.IsAny<TodoItem>())).Returns(true);

        // Act
        var result = _controller.Edit(1, todo) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }

    [Fact]
    public void Edit_Post_WithMismatchedId_ReturnsNotFound()
    {
        // Arrange
        var todo = new TodoItem { Id = 2, Title = "Test" };

        // Act
        var result = _controller.Edit(1, todo);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Post_WhenUpdateFails_ReturnsNotFound()
    {
        // Arrange
        var todo = new TodoItem { Id = 1, Title = "Test" };
        _mockTodoService.Setup(s => s.Update(It.IsAny<TodoItem>())).Returns(false);

        // Act
        var result = _controller.Edit(1, todo);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Post_WithInvalidModel_ReturnsViewWithModel()
    {
        // Arrange
        var todo = new TodoItem { Id = 1, Title = "" };
        _controller.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = _controller.Edit(1, todo) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as TodoItem;
        Assert.NotNull(model);
    }

    #endregion

    #region Delete GET Tests

    [Fact]
    public void Delete_Get_WithValidId_ReturnsViewWithTodo()
    {
        // Arrange
        var todo = new TodoItem { Id = 1, Title = "Test Todo" };
        _mockTodoService.Setup(s => s.GetById(1)).Returns(todo);

        // Act
        var result = _controller.Delete(1) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as TodoItem;
        Assert.NotNull(model);
        Assert.Equal(1, model.Id);
    }

    [Fact]
    public void Delete_Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockTodoService.Setup(s => s.GetById(999)).Returns((TodoItem?)null);

        // Act
        var result = _controller.Delete(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    #endregion

    #region DeleteConfirmed Tests

    [Fact]
    public void DeleteConfirmed_RedirectsToIndex()
    {
        // Arrange
        _mockTodoService.Setup(s => s.Delete(1)).Returns(true);

        // Act
        var result = _controller.DeleteConfirmed(1) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }

    [Fact]
    public void DeleteConfirmed_CallsServiceDelete()
    {
        // Arrange
        _mockTodoService.Setup(s => s.Delete(1)).Returns(true);

        // Act
        _controller.DeleteConfirmed(1);

        // Assert
        _mockTodoService.Verify(s => s.Delete(1), Times.Once);
    }

    #endregion

    #region ToggleComplete Tests

    [Fact]
    public void ToggleComplete_RedirectsToIndex()
    {
        // Arrange
        _mockTodoService.Setup(s => s.ToggleComplete(1)).Returns(true);

        // Act
        var result = _controller.ToggleComplete(1) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }

    [Fact]
    public void ToggleComplete_CallsServiceToggleComplete()
    {
        // Arrange
        _mockTodoService.Setup(s => s.ToggleComplete(1)).Returns(true);

        // Act
        _controller.ToggleComplete(1);

        // Assert
        _mockTodoService.Verify(s => s.ToggleComplete(1), Times.Once);
    }

    #endregion
}
