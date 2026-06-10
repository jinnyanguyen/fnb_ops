using RestaurantOps.Models;

namespace RestaurantOps.Business.Interfaces;

public interface IUserService
{
    void CreateUser(User user, string password);
}