using TodoApp.Models;

namespace TodoApp.Tests.Models;

public class ErrorViewModelTests
{
    [Fact]
    public void ErrorViewModel_RequestId_DefaultValue_IsNull()
    {
        // Arrange & Act
        var errorViewModel = new ErrorViewModel();

        // Assert
        Assert.Null(errorViewModel.RequestId);
    }

    [Fact]
    public void ErrorViewModel_ShowRequestId_ReturnsFalse_WhenRequestIdIsNull()
    {
        // Arrange
        var errorViewModel = new ErrorViewModel { RequestId = null };

        // Act & Assert
        Assert.False(errorViewModel.ShowRequestId);
    }

    [Fact]
    public void ErrorViewModel_ShowRequestId_ReturnsFalse_WhenRequestIdIsEmpty()
    {
        // Arrange
        var errorViewModel = new ErrorViewModel { RequestId = "" };

        // Act & Assert
        Assert.False(errorViewModel.ShowRequestId);
    }

    [Fact]
    public void ErrorViewModel_ShowRequestId_ReturnsTrue_WhenRequestIdHasValue()
    {
        // Arrange
        var errorViewModel = new ErrorViewModel { RequestId = "test-request-id" };

        // Act & Assert
        Assert.True(errorViewModel.ShowRequestId);
    }

    [Fact]
    public void ErrorViewModel_CanSetRequestId()
    {
        // Arrange
        var requestId = "12345-abcde";

        // Act
        var errorViewModel = new ErrorViewModel { RequestId = requestId };

        // Assert
        Assert.Equal(requestId, errorViewModel.RequestId);
    }
}
