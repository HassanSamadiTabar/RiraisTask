using Microsoft.EntityFrameworkCore;
using RiraisTask.Domain;

namespace RiraisTask.Infrastructure.Repositories;

public class PersonRepository(AppDbContext db) : IPersonRepository
{
    public Task<bool> ExistsByNationalCodeAsync(string nationalCode, Guid? excludeId, CancellationToken cancellationToken)
    {
        var query = db.People.AsNoTracking().Where(x => x.NationalCode == nationalCode);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return query.AnyAsync(cancellationToken);
    }

    public async Task AddAsync(Person person, CancellationToken cancellationToken)
    {
        await db.People.AddAsync(person, cancellationToken);
    }

    public Task<Person?> GetByIdAsync(Guid id, bool tracking, CancellationToken cancellationToken)
    {
        var query = db.People.AsQueryable();

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<(List<Person> People, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = db.People.AsNoTracking()
            .OrderBy(x => x.LastName)
            .ThenBy(x => x.FirstName);

        var totalCount = await query.CountAsync(cancellationToken);

        var people = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (people, totalCount);
    }

    public Task RemoveAsync(Person person, CancellationToken cancellationToken)
    {
        db.People.Remove(person);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return db.SaveChangesAsync(cancellationToken);
    }
}
