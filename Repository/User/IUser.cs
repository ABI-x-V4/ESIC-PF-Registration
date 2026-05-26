using DataModels;

namespace Repository.User
{
    public interface IUser
    {
        Task<UserDTO?> LoginAsync(string username, string password);
        Task<bool> RegisterUserAsync(UserDTO model);
    }
}
