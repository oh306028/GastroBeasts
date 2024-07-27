using App;
using App.Dtos.CreateDtos;
using App.Entities;
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
        private WebApplicationFactory<Program> _factory;    

        public RestaurantControllerTests(WebApplicationFactory<Program> factory)
        {

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(o =>
                    {
                        o.DefaultAuthenticateScheme = "TestScheme";
                        o.DefaultChallengeScheme = "TestScheme";
                    }).AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                            "TestScheme", options => { });

                    var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<AppDbContext>));

                    services.Remove(dbContextDescriptor);


                    services.AddDbContext<AppDbContext>(options =>
                     options.UseInMemoryDatabase("TestDb"));

                });
            });

            _client = _factory.CreateClient();
      

            _client.DefaultRequestHeaders.Authorization =
       new AuthenticationHeaderValue(scheme: "TestScheme");


        }


        [Fact]
        public async Task GetRestaurantById_WithValidIdAndModel_ReturnsOk()
        {


            var restaurant = new Restaurant()
            {
                Name = "test rest",
                Description = "test desc",
            };


            var serviceScope = _factory.Services.GetService<IServiceScopeFactory>();

            using var scope = serviceScope.CreateScope();

            var _dbContextScope = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            _dbContextScope.Restaurants.Add(restaurant);
            _dbContextScope.SaveChanges();

            var restaurantToGet = _dbContextScope.Restaurants.First(n => n.Name == "test rest");

            var response = await _client.GetAsync($"/api/restaurants/{restaurantToGet.Id}");    
                
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        
            _dbContextScope.Restaurants.Remove(restaurant);
            _dbContextScope.SaveChanges();

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


        [Theory]
        [InlineData("oneNamethat is already too long to be a restaurant name so its shouldnt be created right now")]
        public async Task CreateRestaurant_WithInvalidModel_ReturnsBadRequest(string name)
        {
            var restaurantModel = new CreateRestaurantDto()
            {
                Name = name
            };


            var response = await _client.PostAsJsonAsync("/api/restaurants",restaurantModel);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);



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
