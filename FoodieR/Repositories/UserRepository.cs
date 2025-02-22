using FoodieR.Data;
using Microsoft.AspNetCore.Identity;

namespace FoodieR.Repositories;

public class UserRepository
{

    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public IdentityUser GetUserByUserName(string userName)
    {
        return _context.Users.FirstOrDefault(user => user.UserName == userName);
    }
}