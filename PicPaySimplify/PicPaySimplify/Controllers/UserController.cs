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
        private readonly IUserRepository _buyerUserRepository;

        public UserController(IUserRepository buyerUserRepository)
        {
            _buyerUserRepository = buyerUserRepository;
        }

        [HttpPost("/user/register")]
        public async Task<ActionResult<UserModel>> Register(UserModel newModel)
        {
            try
            {
                return await _buyerUserRepository.RegisterUser(newModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("/user/remove/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                return await _buyerUserRepository.DeleteUser(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
