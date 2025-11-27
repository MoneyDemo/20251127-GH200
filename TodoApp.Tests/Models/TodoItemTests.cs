using System.ComponentModel.DataAnnotations;
using TodoApp.Models;

namespace TodoApp.Tests.Models;

public class TodoItemTests
{
    #region Property Default Value Tests

    [Fact]
    public void TodoItem_Id_DefaultValue_IsZero()
    {
        // Arrange & Act
        var todo = new TodoItem();

        // Assert
        Assert.Equal(0, todo.Id);
    }

    [Fact]
    public void TodoItem_Title_DefaultValue_IsEmptyString()
    {
        // Arrange & Act
        var todo = new TodoItem();

        // Assert
        Assert.Equal(string.Empty, todo.Title);
    }

    [Fact]
    public void TodoItem_Description_DefaultValue_IsNull()
    {
        // Arrange & Act
        var todo = new TodoItem();

        // Assert
        Assert.Null(todo.Description);
    }

    [Fact]
    public void TodoItem_IsCompleted_DefaultValue_IsFalse()
    {
        // Arrange & Act
        var todo = new TodoItem();

        // Assert
        Assert.False(todo.IsCompleted);
    }

    [Fact]
    public void TodoItem_CreatedAt_DefaultValue_IsCloseToNow()
    {
        // Arrange
        var before = DateTime.Now;

        // Act
        var todo = new TodoItem();

        // Assert
        var after = DateTime.Now;
        Assert.True(todo.CreatedAt >= before.AddSeconds(-1));
        Assert.True(todo.CreatedAt <= after.AddSeconds(1));
    }

    #endregion

    #region Property Setting Tests

    [Fact]
    public void TodoItem_CanSetId()
    {
        // Arrange & Act
        var todo = new TodoItem { Id = 42 };

        // Assert
        Assert.Equal(42, todo.Id);
    }

    [Fact]
    public void TodoItem_CanSetTitle()
    {
        // Arrange & Act
        var todo = new TodoItem { Title = "Test Title" };

        // Assert
        Assert.Equal("Test Title", todo.Title);
    }

    [Fact]
    public void TodoItem_CanSetDescription()
    {
        // Arrange & Act
        var todo = new TodoItem { Description = "Test Description" };

        // Assert
        Assert.Equal("Test Description", todo.Description);
    }

    [Fact]
    public void TodoItem_CanSetIsCompleted()
    {
        // Arrange & Act
        var todo = new TodoItem { IsCompleted = true };

        // Assert
        Assert.True(todo.IsCompleted);
    }

    [Fact]
    public void TodoItem_CanSetCreatedAt()
    {
        // Arrange
        var specificDate = new DateTime(2023, 1, 15, 10, 30, 0);

        // Act
        var todo = new TodoItem { CreatedAt = specificDate };

        // Assert
        Assert.Equal(specificDate, todo.CreatedAt);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void TodoItem_Validation_TitleRequired_FailsWhenEmpty()
    {
        // Arrange
        var todo = new TodoItem { Title = "" };
        var validationContext = new ValidationContext(todo);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(todo, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage!.Contains("Title is required"));
    }

    [Fact]
    public void TodoItem_Validation_TitleRequired_PassesWhenProvided()
    {
        // Arrange
        var todo = new TodoItem { Title = "Valid Title" };
        var validationContext = new ValidationContext(todo);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(todo, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void TodoItem_Validation_TitleMaxLength_FailsWhenExceeds100()
    {
        // Arrange
        var todo = new TodoItem { Title = new string('a', 101) };
        var validationContext = new ValidationContext(todo);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(todo, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage!.Contains("100 characters"));
    }

    [Fact]
    public void TodoItem_Validation_TitleMaxLength_PassesAt100()
    {
        // Arrange
        var todo = new TodoItem { Title = new string('a', 100) };
        var validationContext = new ValidationContext(todo);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(todo, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void TodoItem_Validation_DescriptionMaxLength_FailsWhenExceeds500()
    {
        // Arrange
        var todo = new TodoItem 
        { 
            Title = "Valid Title", 
            Description = new string('a', 501) 
        };
        var validationContext = new ValidationContext(todo);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(todo, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage!.Contains("500 characters"));
    }

    [Fact]
    public void TodoItem_Validation_DescriptionMaxLength_PassesAt500()
    {
        // Arrange
        var todo = new TodoItem 
        { 
            Title = "Valid Title", 
            Description = new string('a', 500) 
        };
        var validationContext = new ValidationContext(todo);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(todo, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void TodoItem_Validation_DescriptionIsOptional()
    {
        // Arrange
        var todo = new TodoItem { Title = "Valid Title", Description = null };
        var validationContext = new ValidationContext(todo);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(todo, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    #endregion
}
