using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using RiraisTask.Domain;
using RiraisTask.Infrastructure;
using RiraisTask.Protos;
using System.Globalization;

namespace RiraisTask.GRPC
{
    public class PeopleGrpcService : PeopleService.PeopleServiceBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<PeopleGrpcService> _logger;

        public PeopleGrpcService(AppDbContext db, ILogger<PeopleGrpcService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override async Task<PersonDto> CreatePerson(CreatePersonRequest request, ServerCallContext context)
        {
            ValidateCreateOrUpdate(request.FirstName, request.LastName, request.NationalCode, request.BirthDate);

            var birthDate = ParseBirthDate(request.BirthDate);

            var exists = await _db.People.AnyAsync(x => x.NationalCode == request.NationalCode, context.CancellationToken);
            if (exists)
                throw new RpcException(new Status(StatusCode.AlreadyExists, "National code already exists."));

            var person = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                NationalCode = request.NationalCode.Trim(),
                BirthDate = birthDate
            };

            _db.People.Add(person);

            try
            {
                await _db.SaveChangesAsync(context.CancellationToken);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to create person.");
                throw new RpcException(new Status(StatusCode.Internal, "Database error while creating person."));
            }

            return Map(person);
        }

        public override async Task<PersonDto> GetPerson(GetPersonRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out var id))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid id."));

            var person = await _db.People.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, context.CancellationToken);

            if (person is null)
                throw new RpcException(new Status(StatusCode.NotFound, "Person not found."));

            return Map(person);
        }

        public override async Task<GetAllPeopleResponse> GetAllPeople(GetAllPeopleRequest request, ServerCallContext context)
        {
            var people = await _db.People.AsNoTracking()
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToListAsync(context.CancellationToken);

            var response = new GetAllPeopleResponse();
            response.People.AddRange(people.Select(Map));
            return response;
        }

        public override async Task<PersonDto> UpdatePerson(UpdatePersonRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out var id))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid id."));

            ValidateCreateOrUpdate(request.FirstName, request.LastName, request.NationalCode, request.BirthDate);

            var birthDate = ParseBirthDate(request.BirthDate);

            var person = await _db.People.FirstOrDefaultAsync(x => x.Id == id, context.CancellationToken);
            if (person is null)
                throw new RpcException(new Status(StatusCode.NotFound, "Person not found."));

            var duplicateNationalCode = await _db.People.AnyAsync(
                x => x.Id != id && x.NationalCode == request.NationalCode,
                context.CancellationToken);

            if (duplicateNationalCode)
                throw new RpcException(new Status(StatusCode.AlreadyExists, "National code already exists."));

            person.FirstName = request.FirstName.Trim();
            person.LastName = request.LastName.Trim();
            person.NationalCode = request.NationalCode.Trim();
            person.BirthDate = birthDate;

            try
            {
                await _db.SaveChangesAsync(context.CancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict while updating person {PersonId}", id);
                throw new RpcException(new Status(StatusCode.Aborted, "The record was modified by another process."));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to update person {PersonId}", id);
                throw new RpcException(new Status(StatusCode.Internal, "Database error while updating person."));
            }

            return Map(person);
        }

        public override async Task<DeletePersonResponse> DeletePerson(DeletePersonRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out var id))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid id."));

            var person = await _db.People.FirstOrDefaultAsync(x => x.Id == id, context.CancellationToken);
            if (person is null)
                throw new RpcException(new Status(StatusCode.NotFound, "Person not found."));

            _db.People.Remove(person);

            try
            {
                await _db.SaveChangesAsync(context.CancellationToken);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to delete person {PersonId}", id);
                throw new RpcException(new Status(StatusCode.Internal, "Database error while deleting person."));
            }

            return new DeletePersonResponse { Success = true };
        }

        private static PersonDto Map(Person person) => new()
        {
            Id = person.Id.ToString(),
            FirstName = person.FirstName,
            LastName = person.LastName,
            NationalCode = person.NationalCode,
            BirthDate = person.BirthDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        };

        private static void ValidateCreateOrUpdate(string firstName, string lastName, string nationalCode, string birthDate)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "First name is required."));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Last name is required."));

            if (string.IsNullOrWhiteSpace(nationalCode))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "National code is required."));

            nationalCode = nationalCode.Trim();
            if (nationalCode.Length != 10 || !nationalCode.All(char.IsDigit))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "National code must be exactly 10 digits."));

            _ = ParseBirthDate(birthDate);
        }

        private static DateOnly ParseBirthDate(string birthDate)
        {
            if (!DateOnly.TryParseExact(birthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Birth date must be in yyyy-MM-dd format."));

            if (date > DateOnly.FromDateTime(DateTime.UtcNow.Date))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Birth date cannot be in the future."));

            return date;
        }
    }
}
