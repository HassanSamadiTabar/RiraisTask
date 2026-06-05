using Grpc.Core;

namespace RiraisTask.Grpc;

public static class GrpcRequestParser
{
    public static Guid ParsePersonId(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid id."));
        }

        return parsedId;
    }
}
