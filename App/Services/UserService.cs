using App.Dtos.CreateDtos;
using App.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace App.Services
{
    public interface IUserService
    {
        void RegisterNewUser(RegisterUserDto dto);
    }   


    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public void RegisterNewUser(RegisterUserDto dto)
        {
            var hasher = new PasswordHasher<User>();


            var mappedUser = _mapper.Map<User>(dto);
            mappedUser.PasswordHash = hasher.HashPassword(mappedUser, dto.Password);


            _dbContext.Users.Add(mappedUser);
            _dbContext.SaveChanges();
            
        }

    }
}
