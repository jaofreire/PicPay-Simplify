using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PicPaySimplify.Models;
using PicPaySimplify.Repositories.Interface;

namespace PicPaySimplify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;

        public UserController(IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
        }

        [HttpPost("/user/register")]
        public async Task<ActionResult<UserModel>> Register(UserModel newModel)
        {
            try
            {
                return await _UserRepository.RegisterUser(newModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/user/getAll")]
        public async Task<ActionResult<List<UserModel>>> GetAll()
        {
            try
            {
                return await _UserRepository.GetAllUsers();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("/user/remove/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                return await _UserRepository.DeleteUser(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
