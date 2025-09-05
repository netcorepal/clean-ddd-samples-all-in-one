using Refit;

namespace TradingEngine.Web.Clients;

public interface IUserServiceClient
{
    [Get("/users/{userId}")]
    Task<UserDto> GetUserAsync(long userId);
}

public record UserDto(string Name, string Email, string Phone);