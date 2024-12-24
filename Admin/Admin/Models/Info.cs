using Admin.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace Admin.Models
{
    [Index(nameof(Barcode),IsUnique = true)]
    public class Info
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Barcode { get; set; }
    }

    public static class InfoEndpoints
    {
        public static void MapInfoEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Infos").WithTags(nameof(Info));

            group.MapGet("/", async (AppDbContext db) =>
            {
                return await db.Info.ToListAsync();
            })
            .WithName("GetAllInfos")
            .WithOpenApi();

            group.MapGet("/{id}", async Task<Results<Ok<Info>, NotFound>> (int id, AppDbContext db) =>
            {
                return await db.Info.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.Id == id)
                    is Info model
                        ? TypedResults.Ok(model)
                        : TypedResults.NotFound();
            })
            .WithName("GetInfoById")
            .WithOpenApi();

            group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Info info, AppDbContext db) =>
            {
                var affected = await db.Info
                    .Where(model => model.Id == id)
                    .ExecuteUpdateAsync(setters => setters
                      .SetProperty(m => m.Id, info.Id)
                      .SetProperty(m => m.Name, info.Name)
                      .SetProperty(m => m.Barcode, info.Barcode)
                      );
                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            })
            .WithName("UpdateInfo")
            .WithOpenApi();

            group.MapPost("/", async (Info info, AppDbContext db) =>
            {
                db.Info.Add(info);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/api/Info/{info.Id}", info);
            })
            .WithName("CreateInfo")
            .WithOpenApi();

            group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, AppDbContext db) =>
            {
                var affected = await db.Info
                    .Where(model => model.Id == id)
                    .ExecuteDeleteAsync();
                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            })
            .WithName("DeleteInfo")
            .WithOpenApi();
        }
    }

}
