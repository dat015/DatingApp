using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using API.Entities;
using API.DTOs;
using API.Interface;
using Microsoft.EntityFrameworkCore;
using DatingWebApp.Data;

namespace API.Controllers
{

    public class AccountController(ApplicationDbContext context, ITokenService tokenService) : BaseApiController
    {

        [HttpPost("register")]//acconut/register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO dto)
        {

            if (await UserExits(dto.userName)) return BadRequest("UserName is taken");
            using var hmac = new HMACSHA256();

            var user = new AppUser
            {
                UserName = dto.userName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.password)),
                PasswordSalt = hmac.Key
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDTO
            {
                userName = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.userName) || string.IsNullOrEmpty(dto.password))
            {
                return BadRequest("Username and password are required.");
            }
            var user = await context.Users
                .FirstOrDefaultAsync(x => x.UserName.ToLower() == dto.userName.ToLower());

            if (user == null) return Unauthorized("Invalid username.");

            if (user.PasswordSalt == null || user.PasswordHash == null)
            {
                return Unauthorized("Invalid password configuration.");
            }

            using var hmac = new HMACSHA256(user.PasswordSalt);

            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.password));

            if (computeHash.Length != user.PasswordHash.Length)
            {
                return Unauthorized("Invalid password.");
            }

            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid password.");
                }
            }
            
            

            return new UserDTO{
                userName = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }
        private async Task<bool> UserExits(string userName)
        {
            return await context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower()); //Bob != bob
        }
        
    }
}