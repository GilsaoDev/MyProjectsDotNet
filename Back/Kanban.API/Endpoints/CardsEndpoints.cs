using Kanban.API.Context;
using Kanban.API.Models;
using Kanban.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kanban.API.ApiEndpoints
{
    public static class CardsEndpoints
    {
        public static void MapCardsEndpoints(this WebApplication app)
        {            

            app.MapGet("0.0.0:5000/cards", async (AppDbContext db) =>
               await db.Cards.ToListAsync()).WithTags("Cards").RequireAuthorization();

            app.MapGet("0.0.0:5000/cards/{id:int}", async (int id, AppDbContext db)
                =>
            {
                return await db.Cards.FindAsync(id)
                             is Card card
                             ? Results.Ok(card)
                             : Results.NotFound();
            });

            app.MapPost("0.0.0:5000/cards", async (CreateCardViewModel createCardViewModel, AppDbContext db)
                =>
            {
                var lastCard = db.Cards.Last<Card>();
                int newIdCard = (lastCard != null) ? (lastCard.Id + 1) : 1;

                var newCard = createCardViewModel.MapTo(newIdCard);
                if (!createCardViewModel.IsValid)
                    return Results.BadRequest(createCardViewModel.Notifications);

                db.Cards.Add(newCard);
                await db.SaveChangesAsync();

                return Results.Created($"0.0.0:5000/cards/{newIdCard}", newCard);
            });

            app.MapPut("0.0.0:5000/cards/{id:int}", async (int id, UpdateCardViewModel updateCardViewModel, AppDbContext db) 
                =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest("Id do Card não pode ser menor que 1.");
                }

                var cardRetornoDB = await db.Cards.FindAsync(id);

                if (cardRetornoDB is null) return Results.NotFound("Não existe Card com esse Id na Base de Dados.");

                var cardUpdated = updateCardViewModel.MapTo(cardRetornoDB);
                if (!updateCardViewModel.IsValid)
                    return Results.BadRequest(updateCardViewModel.Notifications);

                db.Update(cardUpdated);
                await db.SaveChangesAsync();

                return Results.Ok(cardUpdated);
            });

            app.MapDelete("0.0.0:5000/cards/{id:int}", async (int id, AppDbContext db) 
                =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest("Id do Card não pode ser menor que 1.");
                }

                try
                {
                    var cardRetornoDB = await db.Cards.FindAsync(id);

                    if (cardRetornoDB is null)
                    {
                        return Results.NotFound();
                    }

                    db.Cards.Remove(cardRetornoDB);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var problemDetails = new ProblemDetails()
                    {
                        Detail = $"Erro ao tentar recuperar eventos. Erro: {ex.Message}",
                        Status = StatusCodes.Status500InternalServerError,
                    };
                    return Results.Problem(problemDetails);
                }
                return Results.NoContent();
            });
        }
    }
}
