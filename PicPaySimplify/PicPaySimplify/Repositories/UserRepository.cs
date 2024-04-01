using Microsoft.EntityFrameworkCore;
using PicPaySimplify.Data;
using PicPaySimplify.Models;
using PicPaySimplify.Repositories.Interface;

namespace PicPaySimplify.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PicPayDbContext _dbContext;

        public UserRepository(PicPayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserModel> RegisterUser(UserModel newModel)
        {
            var existEmail = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == newModel.Email);
            var existDocment = await _dbContext.Users.FirstOrDefaultAsync(x => x.DocumentNumber == newModel.DocumentNumber);

            if (existEmail != null) throw new Exception("This email already exist");
            if (existDocment != null) throw new Exception("This" + newModel.Document + "already exist");
                
            await _dbContext.Users.AddAsync(newModel);
            await _dbContext.SaveChangesAsync();

            return newModel;
        }

        public async Task<UserModel> GetUserById(int id)
        {
            return await _dbContext.Users.FindAsync(id) ??
                throw new Exception("User not found");
        }

        public async Task<bool> DeleteUser(int id)
        {
            var model = await GetUserById(id);

            _dbContext.Users.Remove(model);
            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
