using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bcrypt.Data;
using Bcrypt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bcrypt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private AppDbContext _context { get; set; }

        public LoginController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        public async Task<ActionResult<User>> UserLogin(string email, string senha)
        {
            if (String.IsNullOrEmpty(email) && String.IsNullOrEmpty(senha))
                return StatusCode(403, new { message = "Usuário e/ou senha inválidos" });

            var userExists = UserLoginExists(email);

            if (userExists)
            {
                var user = await _context.Users.Where(x => x.Email == email).SingleOrDefaultAsync();

                if (user != null)
                {
                    var validatePassword = BCrypt.Net.BCrypt.EnhancedVerify(senha, user.PassHash);

                    if (!validatePassword)
                        return StatusCode(403, new { message = "Usuário e/ou senha inválidos" });

                    return user;
                }
            }

            return StatusCode(403, new { message = "Usuário e/ou senha inválidos" });
        }

        private bool UserLoginExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }
        
    }

}
