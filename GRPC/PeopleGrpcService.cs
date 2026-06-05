using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
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
        var person = await personService.GetByIdAsync(
            GrpcRequestParser.ParsePersonId(request.Id),
            context.CancellationToken);

        return PersonGrpcMapper.ToDto(person);
    }

    public override async Task<GetAllPeopleResponse> GetAllPeople(GetAllPeopleRequest request, ServerCallContext context)
    {
        var result = await personService.GetAllAsync(
            request.Page,
            request.PageSize,
            request.Search,
            context.CancellationToken);

        return PersonGrpcMapper.ToPagedResponse(result);
    }

    public override async Task<PersonDto> UpdatePerson(UpdatePersonRequest request, ServerCallContext context)
    {
        var input = PersonValidator.ValidateUpdate(
            GrpcRequestParser.ParsePersonId(request.Id),
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
        await personService.DeleteAsync(
            GrpcRequestParser.ParsePersonId(request.Id),
            context.CancellationToken);

        return new Empty();
    }
}
