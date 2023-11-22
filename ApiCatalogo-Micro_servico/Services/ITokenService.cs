using ApiCatalogo_Micro_servico.Models;

namespace ApiCatalogo_Micro_servico.Services
{
    public interface ITokenService
    {

        string GetToken(string Key , string issuer , string audience , UserModel user);
    }
}
