using App;
using App.Dtos.CreateDtos;
using App.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Moq;
using System.Net.Http.Headers;

namespace IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        private Mock<IUserService> _userServiceMock = new Mock<IUserService>();

            
        public AccountControllerTests(WebApplicationFactory<Program> factory)
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

                    services.AddSingleton(_userServiceMock.Object);     

                    services.AddDbContext<AppDbContext>(options =>
                     options.UseInMemoryDatabase("TestDb"));

                });
            });

            _client = _factory.CreateClient();


            _client.DefaultRequestHeaders.Authorization =
       new AuthenticationHeaderValue(scheme: "TestScheme");

        }


        [Fact]
        public async Task Login_ForRegisterdUser_ReturnsOk()
        {
            //arrange
            var loginDto = new LoginUserDto()
            {
                Email = "test@email.com",
                Password = "testpassword"
            };

            _userServiceMock
                .Setup(e => e.Login(It.IsAny<LoginUserDto>()))
                .Returns("jwtToken");


            //act
            var response = await _client.PostAsJsonAsync("/api/account/login", loginDto);


            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);      

        }



    }

}
