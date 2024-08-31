using DocumentApi.Application.Common.Interfaces;

namespace DocumentApi.Infrastructure.Services
{
    public class BasicTimeProvider : ITimeProvider
    {
        public DateTime GetCurrentTime() => DateTime.Now;
    }
}
