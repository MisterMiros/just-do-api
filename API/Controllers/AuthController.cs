using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Filters;
using API.Model.Repositories;
using API.Model.Utils;
using API.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [ApiController]
    [Route("/")]
    public class AuthController : ControllerBase
    {
        protected UserRepository UserRepository {
            get { return HttpContext.RequestServices.GetService<UserRepository>(); }
        }

        private readonly JwtConfig _jwtConfig;
        public AuthController(JwtConfig jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        [HttpPost("/auth")]
        [TransactionFilter]
        public async Task<ActionResult<ResponseData>> Auth(RequestData requestData)
        {
            if (!ValidationUtils.ValidateEmail(requestData.Email))
            {
                return BadRequest(new { Message = "Email is invalid" });
            }
            UserRepository userRepository = UserRepository;
            var user = userRepository.Find(requestData.Email);
            if (user == null || !PasswordUtils.Verify(requestData.Password, user.PasswordHash))
            {
                return new UnauthorizedResult();
            }



            var response = new ResponseData()
            {
                Id = user.Id,
                Email = user.Email,
                Token = JwtUtils.GenerateToken(user, _jwtConfig)
            };

            return Ok(response);

        }

        public class RequestData
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class ResponseData
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string Token { get; set; }
        }
    }
}
