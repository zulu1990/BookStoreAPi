using BookStoreAPi.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using BookStoreApi.Contracts.Entity;
using Microsoft.Extensions.Configuration;
using BookStoreApi.Contracts;
using BookStoreApi.Contracts.Response;
using BookStoreAPi.DAL.Interfaces;
using System.Security.Cryptography;
using AutoMapper;
using BookStoreApi.Contracts.DTO;

namespace BookStoreAPi.BL.Managers
{
    public class AuthManager : IAuthManager
    {
        private readonly IConfiguration _config;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public AuthManager(IConfiguration config, IGenericRepository<User> userRepository, IMapper mapper)
        {
            _config = config;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<LoginResponse>> Login(string username, string password)
        {
            var findUserResult = await _userRepository.GetByExpression(x => x.Username.ToLower() == username, includes: new List<string> { "Cart" });
            if (findUserResult.Success == false)
                return Result<LoginResponse>.Fail("Username Doesn't Exists");

            var user = findUserResult.Data;

            if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) == false)
                return Result<LoginResponse>.Fail("Incorrect Password");



            var token = GenerateToken(user);

            var userDto = _mapper.Map<UserDto>(user);

            var result = new LoginResponse()
            {
                Token = token,
                User = userDto
            };

            return Result<LoginResponse>.Succeed(result);
        }

        public Task<Result> Logout(string token)
        {
            throw new NotImplementedException();
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Secrets:JwtToken").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(3),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }




        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return AreEqualArrays(computedHash, passwordHash);
        }

        private static bool AreEqualArrays(byte[] computed, byte[] passed)
        {
            for (var i = 0; i < computed.Length; i++)
            {
                if (computed[i] != passed[i])
                    return false;
            }

            return true;
        }
    }
}
