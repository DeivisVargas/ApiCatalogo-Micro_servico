using ApiCatalogo_Micro_servico.Context;
using ApiCatalogo_Micro_servico.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo_Micro_servico.ApiEndPoints
{
    public static class ProdutosEndpoints
    {
        public static void MapProdutosEndpoints(this WebApplication app)
        {
            app.MapPost("/produtos", async (Produto produto, AppDbContext db) =>
            {
                try
                {
                    if (db.Produtos.Add(produto) is not null)
                    {
                        await db.SaveChangesAsync();
                        return Results.Created($"/produtos/{produto.ProdutoId}", produto);
                    }
                    return Results.BadRequest("Problemas ao cadastror produtos");
                }
                catch (Exception)
                {
                    return Results.BadRequest("Problemas ao inserir produto");
                }

            }).WithName("Cadastro_produto")
              .Accepts<Produto>("application/json");

            app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync())
                .WithTags("Produtos")
                .RequireAuthorization();

            app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
            {
                return await db.Produtos.FindAsync(id)
                    is Produto produto
                      ? Results.Ok(produto)
                      : Results.NotFound("Produto não encontrado");

            });

            app.MapPut("/produto/{id:int}", async (int id, Produto produtoinput, AppDbContext db) =>
            {
                if (produtoinput.ProdutoId != id)
                {
                    return Results.BadRequest();
                }
                var produtoDb = await db.Produtos.FindAsync(id);

                if (produtoDb is null) return Results.NotFound();

                produtoDb.Nome = produtoinput.Nome;
                produtoDb.Descricao = produtoinput.Descricao;
                produtoDb.Preco = produtoinput.Preco;
                produtoDb.Imagem = produtoinput.Imagem;
                produtoDb.DataCompra = produtoinput.DataCompra;
                produtoDb.Estoque = produtoinput.Estoque;
                produtoDb.CategoriaId = produtoinput.CategoriaId;

                db.SaveChanges();
                return Results.Ok(produtoDb);

            });

            app.MapDelete("produto/id:int", async (int id, AppDbContext db) =>
            {
                var produtoDb = await db.Produtos.FindAsync(id);

                if (produtoDb is null) return Results.NotFound("Produto não encontrado");

                db.Remove(produtoDb);
                db.SaveChanges();
                return Results.NoContent();
            });
        }
    }
}
