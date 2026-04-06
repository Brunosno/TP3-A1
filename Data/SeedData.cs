using Microsoft.AspNetCore.Identity;
using Restaurante.Data;
using Restaurante.Models;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync("ADMIN"))
            await roleManager.CreateAsync(new IdentityRole("ADMIN"));

        if (!await roleManager.RoleExistsAsync("CLIENT"))
            await roleManager.CreateAsync(new IdentityRole("CLIENT"));

        var adminEmail = "admin@admin.com";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            var user = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Name = "Administrador",
                Perfil = Perfil.ADMIN
            };

            var result = await userManager.CreateAsync(user, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "ADMIN");
            }
        }

        if (context.Dishes.Any()) return;

        var ingredients = new List<Ingredient>();

        for (int i = 1; i <= 80; i++)
        {
            ingredients.Add(new Ingredient
            {
                Name = $"Ingrediente {i}",
                Description = $"Descrição do ingrediente {i}"
            });
        }

        context.Ingredients.AddRange(ingredients);
        await context.SaveChangesAsync();

        var dishes = new List<Dish>();

        for (int i = 1; i <= 40; i++)
        {
            dishes.Add(new Dish
            {
                Name = $"Prato {i}",
                Description = $"Descrição do prato {i}",
                BasePrice = 20 + i,
                Period = i <= 20 ? Period.LUNCH : Period.DINNER,
                CheffSuggest = false
            });
        }

        dishes[0].CheffSuggest = true;
        dishes[20].CheffSuggest = true;

        context.Dishes.AddRange(dishes);
        await context.SaveChangesAsync();

        var random = new Random();
        var dishIngredients = new List<DishIngredient>();

        foreach (var dish in dishes)
        {
            var selected = ingredients
                .OrderBy(x => random.Next())
                .Take(5)
                .ToList();

            foreach (var ing in selected)
            {
                dishIngredients.Add(new DishIngredient
                {
                    DishId = dish.Id,
                    IngredientId = ing.Id
                });
            }
        }

        context.DishIngredients.AddRange(dishIngredients);
        await context.SaveChangesAsync();

        var tables = new List<Table>();

        for (int i = 1; i <= 100; i++)
        {
            tables.Add(new Table
            {
                Number = i,
                SupportedPeoples = (i % 6) + 2
            });
        }

        context.Tables.AddRange(tables);
        await context.SaveChangesAsync();
    }
}