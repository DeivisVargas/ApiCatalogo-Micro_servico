using ApiCatalogo_Micro_servico.Context;
using ApiCatalogo_Micro_servico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//recupera a string de conexão 
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

//registro do EF no contexto da classe 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));


var app = builder.Build();


//Definindo End points 
app.MapGet("/" , () => "Catalogo"
    
);


app.MapPost("/categorias" , async (Categoria categoria , AppDbContext db) =>
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

app.MapGet("/categorias", async (AppDbContext db) =>await db.Categorias.ToListAsync());

app.MapGet("/categorias/{id:int}", async (int id , AppDbContext db) => 
{
    return await db.Categorias.FindAsync(id) 
        is Categoria categoria 
          ? Results.Ok(categoria) 
          : Results.NotFound("Categoria não encontrada");

});

app.MapPut("/categoria/{id:int}", async (int id , Categoria categoriainput , AppDbContext db) =>
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
    return Results.Ok( categoriaDb);

});

app.MapDelete("categoria/id:int" , async (int id , AppDbContext db) =>
{
    var categoriaDb = await db.Categorias.FindAsync(id);

    if (categoriaDb is null) return Results.NotFound("Categoria não encontrada");

    db.Remove(categoriaDb);
    db.SaveChanges() ;
    return Results.NoContent();


});

app.MapPost("/produtos" , async (Produto produto , AppDbContext db) =>
{
    try
    {
        if(db.Produtos.Add(produto) is not null)
        {
            await db.SaveChangesAsync();
            return Results.Created( $"/produtos/{produto.ProdutoId}" , produto);
        }
        return Results.BadRequest("Problemas ao cadastror produtos");
    }
    catch (Exception)
    {
        return Results.BadRequest("Problemas ao inserir produto");
    }

}).WithName("Cadastro_produto")
   .Accepts<Produto>("application/json");



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.Run();

