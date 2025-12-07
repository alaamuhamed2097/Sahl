namespace UnitTests.Shared.DTOs.User.Customer;

using global::Shared.DTOs.User.Customer;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Unit tests for CustomerRegistrationDto validation attributes
/// Tests all validation rules for phone-based customer registration
/// </summary>
public class CustomerRegistrationDtoValidationTests
{
    private static bool IsValid(object instance)
    {
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();
        return Validator.TryValidateObject(instance, context, results, true);
    }

    private static List<ValidationResult> GetValidationErrors(object instance)
    {
        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, context, results, true);
        return results;
    }

    #region FirstName and LastName Tests

    [Fact]
    public void FirstName_Required_FailsWhenNull()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = null!,
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void FirstName_MaxLength50_FailsWhenExceeded()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = new string('A', 51),
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void LastName_Required_FailsWhenNull()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = null!,
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.False(isValid);
    }

    #endregion

    #region PhoneCode Tests

    [Fact]
    public void PhoneCode_Required_FailsWhenNull()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = null!,
            PhoneNumber = "1001234567",
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.False(isValid);
    }

    [Theory]
    [InlineData("+20")]
    [InlineData("+966")]
    [InlineData("+1")]
    [InlineData("20")]
    [InlineData("+212")]
    public void PhoneCode_ValidFormat_Passes(string phoneCode)
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = phoneCode,
            PhoneNumber = "1001234567",
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.True(isValid);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("+abc")]
    [InlineData("++20")]
    [InlineData("+123456")]
    [InlineData("")]
    public void PhoneCode_InvalidFormat_Fails(string phoneCode)
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = phoneCode,
            PhoneNumber = "1001234567",
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.False(isValid);
    }

    #endregion

    #region PhoneNumber Tests

    [Fact]
    public void PhoneNumber_Required_FailsWhenNull()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = null!,
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.False(isValid);
    }

    [Theory]
    [InlineData("1001234567")]
    [InlineData("100-123-4567")]
    [InlineData("100 123 4567")]
    [InlineData("(100)1234567")]
    [InlineData("100123456")]
    [InlineData("10012345678901")]
    public void PhoneNumber_ValidFormat_Passes(string phoneNumber)
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = phoneNumber,
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.True(isValid);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("12345678901234567")]
    [InlineData("abcdefghij")]
    [InlineData("100@123#4567")]
    public void PhoneNumber_InvalidFormat_Fails(string phoneNumber)
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = phoneNumber,
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.False(isValid);
    }

    #endregion

    #region Email Tests

    [Fact]
    public void Email_Optional_PassesWhenNull()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Email = null,
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.True(isValid);
    }

    [Theory]
    [InlineData("ahmed@example.com")]
    [InlineData("user@domain.co.uk")]
    [InlineData("test+label@example.com")]
    public void Email_ValidFormat_Passes(string email)
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Email = email,
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.True(isValid);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("user@")]
    public void Email_InvalidFormat_Fails(string email)
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Email = email,
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.False(isValid);
    }

    #endregion

    #region UserName Tests

    [Fact]
    public void UserName_Optional_PassesWhenNull()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.True(isValid);
    }
    #endregion

    #region Password Tests

    [Fact]
    public void Password_Required_FailsWhenNull()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Password = null!,
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Password_MinLength8_FailsWhenTooShort()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Password = "Short!1",
            ConfirmPassword = "Short!1"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.False(isValid);
    }

    [Theory]
    [InlineData("SecurePass123!")]
    [InlineData("VeryLongPassword123!@#")]
    [InlineData("P@ssw0rd")]
    public void Password_ValidLength_Passes(string password)
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Password = password,
            ConfirmPassword = password
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.True(isValid);
    }

    #endregion

    #region ConfirmPassword Tests

    [Fact]
    public void ConfirmPassword_Required_FailsWhenNull()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Password = "SecurePass123!",
            ConfirmPassword = null!
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void ConfirmPassword_MustMatch_FailsWhenDifferent()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Password = "SecurePass123!",
            ConfirmPassword = "DifferentPass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void ConfirmPassword_MustMatch_PassesWhenSame()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.True(isValid);
    }

    #endregion

    #region Complete DTO Tests

    [Fact]
    public void CompleteDto_AllFieldsValid_Passes()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Email = "ahmed@example.com",
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void MinimalDto_OnlyRequiredFields_Passes()
    {
        // Arrange
        var dto = new CustomerRegistrationDto
        {
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneCode = "+20",
            PhoneNumber = "1001234567",
            Email = null,
            Password = "SecurePass123!",
            ConfirmPassword = "SecurePass123!"
        };

        // Act
        var isValid = IsValid(dto);

        // Assert
        Assert.True(isValid);
    }

    #endregion
}
