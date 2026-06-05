using RiraisTask.Application;
using RiraisTask.Application.Exceptions;
using Xunit;

namespace RiraisTask.Tests.Application;

public class PersonValidatorTests
{
    [Fact]
    public void ValidateCreate_ReturnsNormalizedInput_WhenValid()
    {
        var input = PersonValidator.ValidateCreate(
            "  Ali  ",
            " Rezaei ",
            " 1234567890 ",
            "1990-05-15");

        Assert.Equal("Ali", input.FirstName);
        Assert.Equal("Rezaei", input.LastName);
        Assert.Equal("1234567890", input.NationalCode);
        Assert.Equal(new DateOnly(1990, 5, 15), input.BirthDate);
    }

    [Theory]
    [InlineData("", "Rezaei", "1234567890", "1990-05-15", "First name is required.")]
    [InlineData("Ali", "", "1234567890", "1990-05-15", "Last name is required.")]
    [InlineData("Ali", "Rezaei", "", "1990-05-15", "National code is required.")]
    [InlineData("Ali", "Rezaei", "12345", "1990-05-15", "National code must be exactly 10 digits.")]
    [InlineData("Ali", "Rezaei", "1234567890", "15-05-1990", "Birth date must be in yyyy-MM-dd format.")]
    public void ValidateCreate_ThrowsValidationException_WhenInvalid(
        string firstName,
        string lastName,
        string nationalCode,
        string birthDate,
        string expectedMessage)
    {
        var exception = Assert.Throws<PersonValidationException>(() =>
            PersonValidator.ValidateCreate(firstName, lastName, nationalCode, birthDate));

        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void ValidateUpdate_RequiresRowVersion()
    {
        var exception = Assert.Throws<PersonValidationException>(() =>
            PersonValidator.ValidateUpdate(
                Guid.NewGuid(),
                "Ali",
                "Rezaei",
                "1234567890",
                "1990-05-15",
                []));

        Assert.Equal("Row version is required for updates.", exception.Message);
    }

    [Fact]
    public void NormalizePagination_UsesDefaultPageSize_WhenZero()
    {
        Assert.Equal(50, PersonValidator.NormalizePagination(1, 0));
    }

    [Fact]
    public void NormalizePagination_Throws_WhenPageSizeTooLarge()
    {
        var exception = Assert.Throws<PersonValidationException>(() =>
            PersonValidator.NormalizePagination(1, 201));

        Assert.Equal("Page size cannot exceed 200.", exception.Message);
    }

    [Fact]
    public void NormalizeSearch_ReturnsNull_WhenEmpty()
    {
        Assert.Null(PersonValidator.NormalizeSearch(null));
        Assert.Null(PersonValidator.NormalizeSearch("   "));
    }

    [Fact]
    public void NormalizeSearch_TrimsValue()
    {
        Assert.Equal("Ali", PersonValidator.NormalizeSearch("  Ali  "));
    }
}
