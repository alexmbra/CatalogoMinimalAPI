using CatalogoMinimalAPI.Context;
using CatalogoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMinimalAPI.Endpoints;

public static class CategoriasEndpoints
{
    public static void MapCategoriasEndpoints(this WebApplication app)
    {
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

        app.MapGet("/categorias", async (AppDBContext db) =>
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
        }).WithTags("Categorias").RequireAuthorization();

        app.MapGet("/categorias/{id:int}", async (int id, AppDBContext db) =>
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

        app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDBContext db) =>
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

        app.MapDelete("/categorias/{id:int}", async (int id, AppDBContext db) =>
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
    }
}
