using RestaurantOps.Business.Helpers;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Data;
using RestaurantOps.Models;

namespace RestaurantOps.Business.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public void CreateUser(User user, string password)
    {
        // Hash password
        user.PasswordHash = PasswordHelper.HashPassword(password);

        _context.Users.Add(user);
        _context.SaveChanges();
    }
}