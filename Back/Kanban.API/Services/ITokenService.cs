using Kanban.API.Models;

namespace Kanban.API.Services
{
    public interface ITokenService
    {
        string GerarToken(string key, string issuer, string audience, UserModel user);
    }

}