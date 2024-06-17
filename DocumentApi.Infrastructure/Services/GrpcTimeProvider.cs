using DocumentApi.Application.Common.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace DocumentApi.Infrastructure.Services
{
    public class GrpcTimeProvider(IConfiguration configuration) : ITimeProvider
    {
        readonly string _grpcServiceUrl = configuration["TimeService:Address"]!;

        public async Task<DateTime> GetCurrentTimeAsync()
        {
            using var channel = GrpcChannel.ForAddress(_grpcServiceUrl);
            var client = new Time.TimeClient(channel);
            var reply = await client.GetCurrentTimeAsync(new Empty());
            DateTime time = DateTime.Parse(reply.Value);
            return time;
        }
    }
}
