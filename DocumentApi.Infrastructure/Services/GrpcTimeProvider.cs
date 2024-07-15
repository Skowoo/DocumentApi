using DocumentApi.Application.Common.Interfaces;

namespace DocumentApi.Infrastructure.Services
{
    public class GrpcTimeProvider : ITimeProvider
    {
        public DateTime GetCurrentTime() => DateTime.Now;
    }
}
