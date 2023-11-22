using ApiCatalogo_Micro_servico.Context;
using ApiCatalogo_Micro_servico.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo_Micro_servico.ApiEndPoints
{
    public static class CategoriasEndpoints
    {

        public static void MapCategoriasEndpoints(this WebApplication app)
        {
            app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias.ToListAsync())
                .WithTags("Categoria")
                .RequireAuthorization();

            app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
            {
                try
                {
                    if (db.Categorias.Add(categoria) is not null)
                    {
                        await db.SaveChangesAsync();
                        return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
                        //return Results.Ok(categoria);
                    }
                    return Results.BadRequest("Problemas ao inserir a categoria");
                }
                catch (Exception)
                {
                    return Results.BadRequest("Problemas ao inserir categoria");

                }


            }).WithName("Cadastro_categoria")
              .Accepts<Categoria>("application/json");


            app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
            {
                return await db.Categorias.FindAsync(id)
                    is Categoria categoria
                      ? Results.Ok(categoria)
                      : Results.NotFound("Categoria não encontrada");

            });

            app.MapPut("/categoria/{id:int}", async (int id, Categoria categoriainput, AppDbContext db) =>
            {
                if (categoriainput.CategoriaId != id)
                {
                    return Results.BadRequest();
                }
                var categoriaDb = await db.Categorias.FindAsync(id);

                if (categoriaDb is null) return Results.NotFound();

                categoriaDb.Nome = categoriainput.Nome;
                categoriaDb.Descricao = categoriainput.Descricao;

                db.SaveChanges();
                return Results.Ok(categoriaDb);

            });

            app.MapDelete("categoria/id:int", async (int id, AppDbContext db) =>
            {
                var categoriaDb = await db.Categorias.FindAsync(id);

                if (categoriaDb is null) return Results.NotFound("Categoria não encontrada");

                db.Remove(categoriaDb);
                db.SaveChanges();
                return Results.NoContent();

            });

        }
    }
}
