using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Controllers;
using TodoApp.Models;

namespace TodoApp.Tests.Controllers;

public class HomeControllerTests
{
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _controller = new HomeController();
    }

    [Fact]
    public void Index_ReturnsViewResult()
    {
        // Act
        var result = _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Privacy_ReturnsViewResult()
    {
        // Act
        var result = _controller.Privacy();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error_ReturnsViewResult()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = _controller.Error() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as ErrorViewModel;
        Assert.NotNull(model);
    }

    [Fact]
    public void Error_SetsRequestIdFromHttpContext()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = "test-trace-id";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = _controller.Error() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as ErrorViewModel;
        Assert.NotNull(model);
        Assert.Equal("test-trace-id", model.RequestId);
    }
}
