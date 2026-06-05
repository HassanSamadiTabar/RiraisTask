using RiraisTask.Application.Models;

namespace RiraisTask.Application;

public interface IPersonService
{
    Task<PersonModel> CreateAsync(CreatePersonInput input, CancellationToken cancellationToken);
    Task<PersonModel> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedPeopleResult> GetAllAsync(int page, int pageSize, string? search, CancellationToken cancellationToken);
    Task<PersonModel> UpdateAsync(UpdatePersonInput input, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
