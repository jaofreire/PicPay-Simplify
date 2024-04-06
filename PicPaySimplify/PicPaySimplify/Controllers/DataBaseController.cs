using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PicPaySimplify.Helper;
using System.Management.Automation;

namespace PicPaySimplify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataBaseController : ControllerBase
    {
        [HttpPost("/migrationUpdate")]
        public ActionResult<bool> UpdateMigration()
        {
            try
            {
                var dataBase = new DataBaseFixture();
                return true;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
