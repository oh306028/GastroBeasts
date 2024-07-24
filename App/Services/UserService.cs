using App.Dtos.CreateDtos;
using App.Entities;
using AutoMapper;

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

        //TO DO:
        //create validator to check if the passed email is already used
        //FluentValidation**

        public void RegisterNewUser(RegisterUserDto dto)
        {

            var mapperUser =_mapper.Map<User>(dto);

            //validate

            _dbContext.Users.Add(mapperUser);
            _dbContext.SaveChanges();
            
        }

    }
}
