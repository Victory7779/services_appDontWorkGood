using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using services_app.Models;
using services_app.Services;

namespace services_app.Controllers
{
    [Route("api/[controller]")] //https://localhost:7700/api/home
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IServiceUsers? _serviceUsers;
        private readonly UserContext? _userContext;
        public HomeController(IServiceUsers? serviceUsers, UserContext userContext)
        {
            _serviceUsers = serviceUsers;
            _userContext = userContext;
            _serviceUsers.db = _userContext;
        }
        //https://localhost:7700/api/home
        [HttpGet]
        public JsonResult Get()
            => Json(_serviceUsers?.Read());

        [HttpGet("{id}")]
        public JsonResult GetUser(int id)
            => Json(_serviceUsers?.GetUserById(id));
        [HttpPost]
        public JsonResult PostUser(User? user)
            => Json(_serviceUsers?.Create(user));
        [HttpPut]
        public JsonResult PutUser(User? user)
            => Json(_serviceUsers?.Update(user));
        [HttpDelete("{id}")]
        public void DeleteUser(int id)
            => _serviceUsers?.Delete(id);
    }
}
