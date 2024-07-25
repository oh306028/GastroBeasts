using App.Dtos.CreateDtos;
using App.Dtos.DisplayDtos;
using App.Entities;
using App.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.Services
{
    public interface IUserService
    {
        void RegisterNewUser(RegisterUserDto dto);
        string Login(LoginUserDto dto);
    }   


    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly JwtOptions _authenticationOptions;

        public UserService(AppDbContext dbContext, IMapper mapper, JwtOptions authenticationOptions)
        {
            _dbContext = dbContext;
            _mapper = mapper;   
            _authenticationOptions = authenticationOptions;
        }


        public void RegisterNewUser(RegisterUserDto dto)
        {
            var hasher = new PasswordHasher<User>();


            var mappedUser = _mapper.Map<User>(dto);
            mappedUser.PasswordHash = hasher.HashPassword(mappedUser, dto.Password);


            _dbContext.Users.Add(mappedUser);
            _dbContext.SaveChanges();
            
        }

      

        public string Login(LoginUserDto dto)
        {
            var currentUser = _dbContext.Users.FirstOrDefault(e => e.Email == dto.Email);

            if(currentUser is null)
                throw new NotFoundException("Invalid Email or password");

            var hasher = new PasswordHasher<User>();

            var verifyPasswordResult = hasher.VerifyHashedPassword(currentUser, currentUser.PasswordHash, dto.Password);

            if(verifyPasswordResult == PasswordVerificationResult.Failed)
                throw new NotFoundException("Invalid Email or password");
                

            var token = GenerateToken(currentUser); 

            return token;

        }
            

        private string GenerateToken(User user)
        {
           

            var key = Encoding.ASCII.GetBytes(_authenticationOptions.Key);
            var securityKey = new SymmetricSecurityKey(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())

            }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _authenticationOptions.Issuer,
                Audience = _authenticationOptions.Audience,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }




    }
}
