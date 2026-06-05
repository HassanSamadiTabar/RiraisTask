using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RiraisTask.Application.Exceptions;
using RiraisTask.Application.Models;
using RiraisTask.Domain;
using RiraisTask.Infrastructure;
using RiraisTask.Infrastructure.Repositories;

namespace RiraisTask.Application;

public class PersonService(
    IPersonRepository repository,
    ILogger<PersonService> logger) : IPersonService
{
    public async Task<PersonModel> CreateAsync(CreatePersonInput input, CancellationToken cancellationToken)
    {
        if (await repository.ExistsByNationalCodeAsync(input.NationalCode, excludeId: null, cancellationToken))
        {
            throw new PersonConflictException("National code already exists.");
        }

        var person = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = input.FirstName,
            LastName = input.LastName,
            NationalCode = input.NationalCode,
            BirthDate = input.BirthDate
        };

        await repository.AddAsync(person, cancellationToken);

        try
        {
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to create person.");
            throw DatabaseExceptionHelper.MapDbUpdateException(ex, "creating person");
        }

        logger.LogInformation("Created person {PersonId}", person.Id);
        return Map(person);
    }

    public async Task<PersonModel> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var person = await repository.GetByIdAsync(id, tracking: false, cancellationToken);

        if (person is null)
        {
            throw new PersonNotFoundException("Person not found.");
        }

        return Map(person);
    }

    public async Task<PagedPeopleResult> GetAllAsync(
        int page,
        int pageSize,
        string? search,
        CancellationToken cancellationToken)
    {
        page = PersonValidator.NormalizePage(page);
        pageSize = PersonValidator.NormalizePagination(page, pageSize);
        search = PersonValidator.NormalizeSearch(search);

        var (people, totalCount) = await repository.GetPagedAsync(page, pageSize, search, cancellationToken);
        return new PagedPeopleResult(people.Select(Map).ToList(), totalCount);
    }

    public async Task<PersonModel> UpdateAsync(UpdatePersonInput input, CancellationToken cancellationToken)
    {
        var person = await repository.GetByIdAsync(input.Id, tracking: true, cancellationToken);

        if (person is null)
        {
            throw new PersonNotFoundException("Person not found.");
        }

        if (await repository.ExistsByNationalCodeAsync(input.NationalCode, input.Id, cancellationToken))
        {
            throw new PersonConflictException("National code already exists.");
        }

        person.FirstName = input.FirstName;
        person.LastName = input.LastName;
        person.NationalCode = input.NationalCode;
        person.BirthDate = input.BirthDate;
        person.RowVersion = input.RowVersion;

        try
        {
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            if (ex is DbUpdateConcurrencyException concurrencyException)
            {
                logger.LogWarning(concurrencyException, "Concurrency conflict while updating person {PersonId}", input.Id);
            }
            else
            {
                logger.LogError(ex, "Failed to update person {PersonId}", input.Id);
            }

            throw DatabaseExceptionHelper.MapDbUpdateException(ex, "updating person");
        }

        logger.LogInformation("Updated person {PersonId}", input.Id);
        return Map(person);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var person = await repository.GetByIdAsync(id, tracking: true, cancellationToken);

        if (person is null)
        {
            throw new PersonNotFoundException("Person not found.");
        }

        await repository.RemoveAsync(person, cancellationToken);

        try
        {
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to delete person {PersonId}", id);
            throw DatabaseExceptionHelper.MapDbUpdateException(ex, "deleting person");
        }

        logger.LogInformation("Deleted person {PersonId}", id);
    }

    private static PersonModel Map(Person person) => new(
        person.Id,
        person.FirstName,
        person.LastName,
        person.NationalCode,
        person.BirthDate,
        person.RowVersion ?? Array.Empty<byte>());
}
