using App;
using App.Dtos.CreateDtos;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.Common;
using System.Net.Http.Headers;
using System.Text;

namespace IntegrationTests
{
    public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public RestaurantControllerTests(WebApplicationFactory<Program> factory)
        {

            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(o =>
                    {
                        o.DefaultAuthenticateScheme = "TestScheme";
                        o.DefaultChallengeScheme = "TestScheme";
                    }).AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                            "TestScheme", options => { });
                });
            })
       .CreateClient();

            _client.DefaultRequestHeaders.Authorization =
       new AuthenticationHeaderValue(scheme: "TestScheme");


        }


        [Fact]
        public async Task CreateRestaurant_WithValidModel_ReturnsOk()
        {
            var restaurantModel = new CreateRestaurantDto()
            {
                Name = "Test Rest",
                Description = "Test descr"
            };

            
               
            var response = await _client.PostAsJsonAsync("/api/restaurants", restaurantModel);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        }




        [Fact]
        public async Task GetRestaurants_ValidQuery_ReturnStatusCodeOk()
        {
          
            var response = await _client.GetAsync("/api/restaurants");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK); 

        }


        [Theory]
        [InlineData("includeReviews=true")]
        [InlineData("includeReviews=true&restaurantName=keb")]
        public async Task GetRestaurants_ValidQueryWithParams_ReturnStatusCodeOk(string phrase)
        {   

            var response = await _client.GetAsync($"/api/restaurants?{phrase}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);


        }


        [Theory]
        [InlineData("all")]
        [InlineData("include=false")]
        [InlineData("name=keb")]

        public async Task GetRestaurants_WithInvalidQuery_ReturnsBadRequest(string phrase)
        {
            var response = await _client.GetAsync($"/api/restaurants/{phrase}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }




    }
}
