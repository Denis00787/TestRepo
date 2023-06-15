using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OauthWebAPI.DB;
using OauthWebAPI.Models;

namespace OauthWebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CrudController : ControllerBase
	{

        private readonly IDbRepository dbRepository;
        public CrudController(IDbRepository repository)
		{
            dbRepository = repository;
		}

        [HttpPost(Name = "AddUser")]
        public async Task<ActionResult> AddUser(User user)
        {
            await dbRepository.InsertAsync(user);
            return Ok();
        }
    }
}

