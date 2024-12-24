using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Admin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Admin.Models
{
    [Index(nameof(BareCode), IsUnique = true)] // Add this attribute

    public class Product
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string? Name { get; set; }
        [Display(Name = "Bare Code")]
        [Required(ErrorMessage = "This field is required.")]
        public string? BareCode { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Country")]
        [Required(ErrorMessage = "This field is required.")]
        public string? Country { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Ingrediants")]
        public string? Ingrediants { get; set; }
        public string? Image { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }


public static class ProductEndpoints
{
	public static void MapProductEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Product").WithTags(nameof(Product));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Product.ToListAsync();
        })
        .WithName("GetAllProducts")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Product>, NotFound>> (int id, AppDbContext db) =>
        {
            return await db.Product.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Product model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetProductById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Product product, AppDbContext db) =>
        {
            var affected = await db.Product
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, product.Id)
                  .SetProperty(m => m.Name, product.Name)
                  .SetProperty(m => m.BareCode, product.BareCode)
                  .SetProperty(m => m.Country, product.Country)
                  .SetProperty(m => m.Ingrediants, product.Ingrediants)
                  .SetProperty(m => m.Image, product.Image)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateProduct")
        .WithOpenApi();

        group.MapPost("/", async (Product product, AppDbContext db) =>
        {
            db.Product.Add(product);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Product/{product.Id}",product);
        })
        .WithName("CreateProduct")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, AppDbContext db) =>
        {
            var affected = await db.Product
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteProduct")
        .WithOpenApi();
    }
}}
