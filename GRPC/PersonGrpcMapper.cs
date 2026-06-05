using System.Globalization;
using Google.Protobuf;
using RiraisTask.Application.Models;
using RiraisTask.Protos;

namespace RiraisTask.Grpc;

public static class PersonGrpcMapper
{
    public static PersonDto ToDto(PersonModel person) => new()
    {
        Id = person.Id.ToString(),
        FirstName = person.FirstName,
        LastName = person.LastName,
        NationalCode = person.NationalCode,
        BirthDate = person.BirthDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        RowVersion = ByteString.CopyFrom(person.RowVersion)
    };

    public static GetAllPeopleResponse ToPagedResponse(PagedPeopleResult result)
    {
        var response = new GetAllPeopleResponse
        {
            TotalCount = result.TotalCount
        };

        response.People.AddRange(result.People.Select(ToDto));
        return response;
    }
}
