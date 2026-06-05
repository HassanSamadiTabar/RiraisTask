using System.Globalization;
using RiraisTask.Application.Exceptions;
using RiraisTask.Application.Models;

namespace RiraisTask.Application;

public static class PersonValidator
{
    private const int MaxNameLength = 100;
    private const int NationalCodeLength = 10;

    public static CreatePersonInput ValidateCreate(
        string firstName,
        string lastName,
        string nationalCode,
        string birthDate)
    {
        var normalized = Normalize(firstName, lastName, nationalCode, birthDate);
        return new CreatePersonInput(
            normalized.FirstName,
            normalized.LastName,
            normalized.NationalCode,
            normalized.BirthDate);
    }

    public static UpdatePersonInput ValidateUpdate(
        Guid id,
        string firstName,
        string lastName,
        string nationalCode,
        string birthDate,
        byte[] rowVersion)
    {
        if (rowVersion.Length == 0)
        {
            throw new PersonValidationException("Row version is required for updates.");
        }

        var normalized = Normalize(firstName, lastName, nationalCode, birthDate);
        return new UpdatePersonInput(
            id,
            normalized.FirstName,
            normalized.LastName,
            normalized.NationalCode,
            normalized.BirthDate,
            rowVersion);
    }

    public static int NormalizePagination(int page, int pageSize)
    {
        if (pageSize <= 0)
        {
            pageSize = 50;
        }

        if (pageSize > 200)
        {
            throw new PersonValidationException("Page size cannot exceed 200.");
        }

        return pageSize;
    }

    public static int NormalizePage(int page)
    {
        return page <= 0 ? 1 : page;
    }

    private static (string FirstName, string LastName, string NationalCode, DateOnly BirthDate) Normalize(
        string firstName,
        string lastName,
        string nationalCode,
        string birthDate)
    {
        firstName = firstName.Trim();
        lastName = lastName.Trim();
        nationalCode = nationalCode.Trim();

        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new PersonValidationException("First name is required.");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new PersonValidationException("Last name is required.");
        }

        if (string.IsNullOrWhiteSpace(nationalCode))
        {
            throw new PersonValidationException("National code is required.");
        }

        if (firstName.Length > MaxNameLength)
        {
            throw new PersonValidationException($"First name cannot exceed {MaxNameLength} characters.");
        }

        if (lastName.Length > MaxNameLength)
        {
            throw new PersonValidationException($"Last name cannot exceed {MaxNameLength} characters.");
        }

        if (nationalCode.Length != NationalCodeLength || !nationalCode.All(char.IsDigit))
        {
            throw new PersonValidationException("National code must be exactly 10 digits.");
        }

        if (!DateOnly.TryParseExact(birthDate.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedBirthDate))
        {
            throw new PersonValidationException("Birth date must be in yyyy-MM-dd format.");
        }

        if (parsedBirthDate > DateOnly.FromDateTime(DateTime.UtcNow.Date))
        {
            throw new PersonValidationException("Birth date cannot be in the future.");
        }

        return (firstName, lastName, nationalCode, parsedBirthDate);
    }
}
