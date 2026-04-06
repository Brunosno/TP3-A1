namespace Restaurante.Models;

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double BasePrice { get; set; }
    public bool CheffSuggest { get; set; }

    public Period Period { get; set; }

    public List<DishIngredient> DishIngredients { get; set; }

    public string? ImageUrl { get; set; }
}