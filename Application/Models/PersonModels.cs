namespace RiraisTask.Application.Models;

public sealed record PersonModel(
    Guid Id,
    string FirstName,
    string LastName,
    string NationalCode,
    DateOnly BirthDate,
    byte[] RowVersion);

public sealed record CreatePersonInput(
    string FirstName,
    string LastName,
    string NationalCode,
    DateOnly BirthDate);

public sealed record UpdatePersonInput(
    Guid Id,
    string FirstName,
    string LastName,
    string NationalCode,
    DateOnly BirthDate,
    byte[] RowVersion);

public sealed record PagedPeopleResult(
    IReadOnlyList<PersonModel> People,
    int TotalCount);
