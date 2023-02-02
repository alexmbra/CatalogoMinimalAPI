using CatalogoMinimalAPI.Context;
using CatalogoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMinimalAPI.Endpoints;

public static class ProdutosEndpoints
{
    public static void MapProdutosEndpoints(this WebApplication app)
    {
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
        }).WithTags("Produtos").RequireAuthorization();

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
