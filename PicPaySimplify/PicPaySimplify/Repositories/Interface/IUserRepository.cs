using PicPaySimplify.Models;

namespace PicPaySimplify.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<List<UserModel>> GetAllUsers();
        Task<UserModel> RegisterUser(UserModel newModel);
        Task<UserModel> GetUserById(int id);
        Task<bool> DeleteUser(int id);
    }
}
