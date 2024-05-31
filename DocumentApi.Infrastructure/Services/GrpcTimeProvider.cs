using DocumentApi.Application.Common.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace DocumentApi.Infrastructure.Services
{
    public class GrpcTimeProvider : ITimeProvider
    {
        public async Task<DateTime> GetCurrentTimeAsync()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7175");
            var client = new Time.TimeClient(channel);
            var reply = await client.GetCurrentTimeAsync(new Empty());
            DateTime time = DateTime.Parse(reply.Value);
            return time;
        }
    }
}
