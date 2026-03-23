using RestaurantOps.Data.Interfaces;
using RestaurantOps.Models;

namespace RestaurantOps.Data.Repositories;

/// <summary>
/// Handles database operations for Ingredient.
/// </summary>
public class IngredientRepository : IIngredientRepository
{
    private readonly ApplicationDbContext _context;

    public IngredientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Ingredient> GetAll()
    {
        return _context.Ingredients.ToList();
    }

    public Ingredient? GetById(int id)
    {
        return _context.Ingredients.Find(id);
    }

    public void Add(Ingredient ingredient)
    {
        _context.Ingredients.Add(ingredient);
    }

    public void Update(Ingredient ingredient)
    {
        _context.Ingredients.Update(ingredient);
    }

    public void Delete(int id)
    {
        var ingredient = _context.Ingredients.Find(id);
        if (ingredient != null)
        {
            _context.Ingredients.Remove(ingredient);
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}