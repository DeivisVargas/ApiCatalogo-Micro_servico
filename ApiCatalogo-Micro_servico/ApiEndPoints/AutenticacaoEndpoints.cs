using ApiCatalogo_Micro_servico.Models;
using ApiCatalogo_Micro_servico.Services;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;

namespace ApiCatalogo_Micro_servico.ApiEndPoints
{
    public static class AutenticacaoEndpoints
    {
        public static void MapAutenticacaoEndPoints(this WebApplication app)
        {
            app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
            {
                if (userModel == null)
                {
                    return Results.BadRequest("Login inválido");
                }

                if (userModel.UserName == "zubat" && userModel.Password == "123456")
                {
                    var tokenString = tokenService.GetToken(app.Configuration["Jwt:key"],
                                app.Configuration["Jwt:Issuer"],
                                app.Configuration["Jwt:Audience"],
                                userModel
                                );
                    return Results.Ok(new { token = tokenString });
                }
                else
                {
                    return Results.BadRequest("Login inválido");
                }
            }).WithName("Login")
              .WithTags("Autenticação");
        }
    }
}
