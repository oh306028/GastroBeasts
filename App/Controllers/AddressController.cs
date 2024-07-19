using App.Dtos.CreateDtos;
using App.Dtos.DisplayDtos;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/restaurants/{restaurantId}/address")]
    [ApiController]
   
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }



        [HttpGet]
        public ActionResult<AddressDto> GetAddress([FromRoute] int restaurantId)
        {
            var address = _addressService.GetAddressFromRestaurant(restaurantId);

            return Ok(address);

        }


        [HttpPost]
        public ActionResult<int> CreateAddress([FromRoute] int restaurantId, [FromBody] CreateAddressDto dto)
        {
            int addressId = _addressService.CreateAddressToRestaurant(restaurantId, dto);

            return Created($"api/restaurants/{restaurantId}/address/{addressId}", null);
        }

    }
}
