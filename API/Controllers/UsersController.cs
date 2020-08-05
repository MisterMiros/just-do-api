using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using API.Model.Repositories;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Authorization;
using API.Settings;
using API.Filters;
using API.Model.Utils;
using Newtonsoft.Json;

namespace API.Controllers.API
{
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private UserRepository UserRepository {
            get { return HttpContext.RequestServices.GetService<UserRepository>(); }
        }

        private User CurrentUser {
            get { return HttpContext.Items[Constants.HTTP_CONTEXT_CURRENT_USER_ITEM] as User; }
        }

        private JwtConfig _jwtConfig;

        public UsersController(JwtConfig jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        [HttpGet]
        [Authorize]
        [UserFilter]
        public ActionResult<IEnumerable<UserObject>> Get()
        {
            return Ok(new List<UserObject>() { transform(CurrentUser) });
        }

        [HttpGet("current")]
        [Authorize]
        [UserFilter]
        public ActionResult<UserObject> GetCurrent()
        {
            return Ok(transform(CurrentUser));
        }

        [HttpPost]
        [TransactionFilter]
        public ActionResult<PostResponseData> Post([FromBody] PostRequestData requestData)
        {
            if (!ValidationUtils.ValidateEmail(requestData.Email))
            {
                return BadRequest(new { Message = "Email is invalid" });
            }

            if (!ValidationUtils.ValidatePassword(requestData.Password))
            {
                return BadRequest(new { Message = "Password is invalid" });
            }

            var userRepository = UserRepository;
            User existingUser = userRepository.Find(requestData.Email);
            if (existingUser != null) {
                return BadRequest(new { Message = "User with given email already exists" });
            }

            User user = UserRepository.Create(requestData.Email, requestData.Password);
            return Ok(new PostResponseData()
            {
                Id = user.Id,
                Email = user.Email,
                Token = JwtUtils.GenerateToken(user, _jwtConfig)
            });
        }

        private UserObject transform(User user)
        {
            return new UserObject()
            {
                Id = user.Id,
                Email = user.Email
            };
        }

        public class UserObject
        {
            public int Id { get; set; }
            public string Email { get; set; }
        }

        public class PostResponseData
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string Token { get; set; }
        }

        public class PostRequestData
        {
            [JsonProperty(Required = Required.Always)]
            public string Email { get; set; }
            [JsonProperty(Required = Required.Always)]
            public string Password { get; set; }
        }
    }
}
