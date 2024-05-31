using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GrpcTimeService.Services
{
    public class TimeService : Time.TimeBase
    {
        public override Task<SingleTimeReply> GetCurrentTime(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new SingleTimeReply
            {
                Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
    }
}
