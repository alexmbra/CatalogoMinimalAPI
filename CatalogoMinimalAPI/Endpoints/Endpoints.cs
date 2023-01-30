using CatalogoMinimalAPI.Context;
using CatalogoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;

namespace CatalogoMinimalAPI.Endpoints
{
    public static class Endpoints
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => "Catálogo de Produtos - 2023").ExcludeFromDescription();

            app.MapPost("/categorias", async (Categoria categoria, AppDBContext db) =>
            {
                try
                {
                    db.Categorias.Add(categoria);
                    await db.SaveChangesAsync();

                    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });            

            app.MapGet("/categorias", async(AppDBContext db) =>
            {
                try
                {
                    return await db.Categorias.Take(10).AsNoTracking().ToListAsync()
                                    is List<Categoria> categorias
                                    ? Results.Ok(categorias)
                                    : Results.NotFound("Não foi encontrada nenhuma categoria");
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });          

            app.MapGet("/categorias/{id:int}", async(int id, AppDBContext db) =>
            {
                try
                {
                    return await db.Categorias.FindAsync(id) 
                                    is Categoria categoria
                                    ? Results.Ok(categoria)
                                    : Results.NotFound($"Categoria {id} não encontrada...");
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });

            app.MapPut("/categorias/{id:int}", async(int id, Categoria categoria, AppDBContext db) =>
            {
                try
                {
                    if (categoria.CategoriaId != id) return Results.BadRequest();

                    var categoriaDB = await db.Categorias.FindAsync(id);
                    if (categoriaDB is null) return Results.NotFound($"Categoria {id} não encontrada...");

                    categoriaDB.Nome = categoria.Nome;
                    categoriaDB.Descricao = categoria.Descricao;

                    await db.SaveChangesAsync();

                    return Results.Ok(categoriaDB);
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });

            app.MapDelete("/categorias/{id:int}", async(int id, AppDBContext db) =>
            {
                try
                {
                    var categoriaDB = await db.Categorias.FindAsync(id);
                    if (categoriaDB is null) return Results.NotFound($"Categoria {id} não encontrada...");

                    db.Categorias.Remove(categoriaDB);
                    await db.SaveChangesAsync();

                    return Results.NoContent();

                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });


            


            app.MapPost("/produtos", async (Produto produto, AppDBContext db) =>
            {
                try
                {
                    db.Produtos.Add(produto);
                    await db.SaveChangesAsync();

                    return Results.Created($"/produtos/{produto.ProdutoId}", produto);
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });


            app.MapGet("/produtos", async (AppDBContext db) =>
            {
                try
                {
                    return await db.Produtos.Take(10).AsNoTracking().ToListAsync()
                                is List<Produto> produtos
                                ? Results.Ok(produtos)
                                : Results.NotFound("Não foi encontrado nenhum produto");

                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });

            app.MapGet("/produtos/{id:int}", async (int id, AppDBContext db) =>
            {
                try
                {
                    return await db.Produtos.FindAsync(id)
                                is Produto produto
                                ? Results.Ok(produto)
                                : Results.NotFound($"Produto {id} não encontrado...");
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });

            app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDBContext db) =>
            {
                try
                {
                    if (produto.ProdutoId != id) return Results.BadRequest();

                    var produtoDB = await db.Produtos.FindAsync(id);
                    if (produtoDB is null) return Results.NotFound($"Produto {id} não encontrada...");

                    produtoDB.Nome = produto.Nome;
                    produtoDB.Descricao = produto.Descricao;
                    produtoDB.Preco = produto.Preco;

                    await db.SaveChangesAsync();

                    return Results.Ok(produtoDB);
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });

            app.MapDelete("/produtos/{id:int}", async (int id, AppDBContext db) =>
            {
                try
                {
                    var produtoDB = await db.Produtos.FindAsync(id);
                    if (produtoDB is null) return Results.NotFound($"Produto {id} não encontrada...");

                    db.Produtos.Remove(produtoDB);
                    await db.SaveChangesAsync();

                    return Results.NoContent();
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });
        }
    }
}
