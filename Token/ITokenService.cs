using RanqueDev.Domain.Entities.Identity;

namespace RanqueDev.Api.Token
{
    public interface ITokenService
    {
        Task<string> GenereteToken(User user);

    }
}
