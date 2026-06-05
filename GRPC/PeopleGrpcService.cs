using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RiraisTask.Application;
using RiraisTask.Protos;

namespace RiraisTask.Grpc;

public class PeopleGrpcService(IPersonService personService) : PeopleService.PeopleServiceBase
{
    public override async Task<PersonDto> CreatePerson(CreatePersonRequest request, ServerCallContext context)
    {
        var input = PersonValidator.ValidateCreate(
            request.FirstName,
            request.LastName,
            request.NationalCode,
            request.BirthDate);

        var person = await personService.CreateAsync(input, context.CancellationToken);
        return PersonGrpcMapper.ToDto(person);
    }

    public override async Task<PersonDto> GetPerson(GetPersonRequest request, ServerCallContext context)
    {
        var id = ParseId(request.Id);
        var person = await personService.GetByIdAsync(id, context.CancellationToken);
        return PersonGrpcMapper.ToDto(person);
    }

    public override async Task<GetAllPeopleResponse> GetAllPeople(GetAllPeopleRequest request, ServerCallContext context)
    {
        var result = await personService.GetAllAsync(
            request.Page,
            request.PageSize,
            context.CancellationToken);

        return PersonGrpcMapper.ToPagedResponse(result);
    }

    public override async Task<PersonDto> UpdatePerson(UpdatePersonRequest request, ServerCallContext context)
    {
        var input = PersonValidator.ValidateUpdate(
            ParseId(request.Id),
            request.FirstName,
            request.LastName,
            request.NationalCode,
            request.BirthDate,
            request.RowVersion.ToByteArray());

        var person = await personService.UpdateAsync(input, context.CancellationToken);
        return PersonGrpcMapper.ToDto(person);
    }

    public override async Task<Empty> DeletePerson(DeletePersonRequest request, ServerCallContext context)
    {
        await personService.DeleteAsync(ParseId(request.Id), context.CancellationToken);
        return new Empty();
    }

    private static Guid ParseId(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid id."));
        }

        return parsedId;
    }
}
