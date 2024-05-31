namespace DocumentApi.Application.Common.Interfaces
{
    public interface ITimeProvider
    {
        Task<DateTime> GetCurrentTimeAsync();
    }
}
