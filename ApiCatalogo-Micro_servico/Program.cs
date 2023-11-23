using ApiCatalogo_Micro_servico.Context;
using ApiCatalogo_Micro_servico.Models;
using ApiCatalogo_Micro_servico.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.OpenApi.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using ApiCatalogo_Micro_servico.ApiEndPoints;
using ApiCatalogo_Micro_servico.AppServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Habilita a atutenticação via swagger com jwt Bearer Token
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apiagenda", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"JWT Authorization header using the Bearer scheme use 'Bearer Token' exemplo 'Bearer 1234token'"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
});


//recupera a string de conexão 
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

//registro do EF no contexto da classe 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

//incluindo o serviço de gerar token 
builder.Services.AddSingleton<ITokenService>( new TokenService());

//autenticando o token 

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey =
                    new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration["Jwt:Key"]))
        };
    });

//incluir serviço de autorização
builder.Services.AddAuthorization();

var app = builder.Build();

//login criado metodo de extenção para melhorar a visualização do código da APi JWT
app.MapAutenticacaoEndPoints();

//Chamada de metodo de extenção para rotas de Categorias 
app.MapCategoriasEndpoints();

//Chamada de metodo de extenção para rotas de Produtos 
app.MapProdutosEndpoints();


// Configure the HTTP request pipeline.
var enviroment = app.Environment;

//fazendo as chamadas dos metodos de extenção criados separadamente 
app.UseExeptionHandling(enviroment)
    .UseSwaggerBuilder()
    .UseAppCors();


//incluido serviço de autorização e autenticação ;
app.UseAuthentication();
app.UseAuthorization();


app.Run();

