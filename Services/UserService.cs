using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ecomak.Models;
using Ecomak.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Ecomak.Services
{
    public class UserService : IUserService
    {
        private UserManager<IdentityUser> userManager;
        private IConfiguration configuration;

      
        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

      
        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Name);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message  = "Usuario o contraseña incorrecto",
                    IsSuccess = false,
                };
            }

            var result = await userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new UserManagerResponse
                {
                    Message  = "Usuario o contraseña incorrecto",
                    IsSuccess = false,
                };

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim("Name", model.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: configuration["AuthSettings:Issuer"],
                audience: configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message  = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
           if(model == null )
            {
                throw new NullReferenceException("model is null");
            }
            if (model.Password != model.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message  = "incorrect Password",
                    IsSuccess = false
                };
            }
            var identityUser = new IdentityUser
            {
                Email = model.Name,
                UserName = model.Name
            };
            var result = await userManager.CreateAsync(identityUser, model.Password);
            if(result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message  = "User created successfully",
                    IsSuccess = true
                };
            }
            return new UserManagerResponse
            {
                Message  = "User did not Create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }
    }
}
