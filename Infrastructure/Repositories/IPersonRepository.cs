using RiraisTask.Application.Models;
using RiraisTask.Domain;

namespace RiraisTask.Infrastructure.Repositories;

public interface IPersonRepository
{
    Task<bool> ExistsByNationalCodeAsync(string nationalCode, Guid? excludeId, CancellationToken cancellationToken);
    Task AddAsync(Person person, CancellationToken cancellationToken);
    Task<Person?> GetByIdAsync(Guid id, bool tracking, CancellationToken cancellationToken);
    Task<(List<Person> People, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task RemoveAsync(Person person, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
