using App;
using App.Dtos.CreateDtos;
using App.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Xunit;

namespace IntegrationTests
{
    public class ReviewControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private  WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ReviewControllerTests(WebApplicationFactory<Program> factory)
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



        private void SeedDbWithRestaurant(AppDbContext context)
        {
            var restaurant = new Restaurant()
            {
                Name = "Test",
                Description = "Test descr"
            };

            context.Restaurants.Add(restaurant);
            context.SaveChanges();
            
        }


        [Fact]
        public async Task CreateReview_ForValidModelAndAuth_ReturnsCreated()
        {
            //arrange
            var factoryService = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = factoryService.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            SeedDbWithRestaurant(_dbContext);

            var restaurantToAddReview = _dbContext.Restaurants.FirstOrDefault(i => i.Name == "Test");

            var review = new CreateReviewDto()
            {
                Comment = "Test comment",
                Stars = 3

            };

            //act
            var response = await _client.PostAsJsonAsync($"/api/restaurants/{restaurantToAddReview.Id}/reviews", review);


            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);


            var revieToDelete = _dbContext.Reviews.FirstOrDefault(c => c.Comment == "Test comment");
                
            _dbContext.Remove(revieToDelete);
            _dbContext.Remove(restaurantToAddReview);

            _dbContext.SaveChanges();

        }

        [Fact]

        public async Task DeleteReview_WithValidRequest_ReturnsNoconent()
        {
            //arrange
            var factoryService = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = factoryService.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            SeedDbWithRestaurant(_dbContext);

            var restaurantToAddReview = _dbContext.Restaurants.FirstOrDefault(i => i.Name == "Test");

            var review = new Review()
            {
                Comment = "Test comment",
                StarsId = 3,
                UserId = 1,
                Restaurant = restaurantToAddReview

            };

            _dbContext.Reviews.Add(review);
            _dbContext.SaveChanges();


            //act
            var reviewToDel = _dbContext.Reviews.FirstOrDefault(n => n.Comment == "Test comment");
            var response = await _client.DeleteAsync($"/api/restaurants/{restaurantToAddReview.Id}/reviews/{reviewToDel.Id}/delete");

                
                
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);


            _dbContext.Remove(restaurantToAddReview);

            _dbContext.SaveChanges();

        }

    }
}
