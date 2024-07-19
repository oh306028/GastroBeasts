using App.Dtos.CreateDtos;
using App.Dtos.DisplayDtos;
using App.Entities;
using App.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace App.Services
{
    public interface IAddressService
    {
        AddressDto GetAddressFromRestaurant([FromRoute] int restaurantId);
        int CreateAddressToRestaurant(int restaurantId, CreateAddressDto dto);
    }

    public class AddressService : IAddressService   
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        private Restaurant CheckRestaurant(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants.Include(a => a.Address).FirstOrDefault(i => i.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            return restaurant;
        }

        public AddressService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public AddressDto GetAddressFromRestaurant(int restaurantId)
        {
            
            var restaurant = CheckRestaurant(restaurantId);

            var address = restaurant.Address;
            var mappedAddress = _mapper.Map<AddressDto>(address);

          
            return (mappedAddress);


        }

        public int CreateAddressToRestaurant(int restaurantId, CreateAddressDto dto)
        {
            var restaurant = CheckRestaurant(restaurantId);

            var createdAddress = _mapper.Map<Address>(dto);
            createdAddress.Restaurant = restaurant;

            _dbContext.Addresses.Add(createdAddress);
            _dbContext.SaveChanges();

            return createdAddress.Id;

        }
    }
}
