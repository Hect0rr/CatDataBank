using System.Net.Http;
using System.Threading.Tasks;
using CatDataBank.FuncTest.WebAppFactory;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace CatDataBank.FuncTest.Controller
{
    class AuthResponse
    {
        public string username { get; set; }
        public string token { get; set; }
    }
    class AuthFailResponse
    {
        public string message { get; set; }
    }

    public class CatDataBankFuncTest : IClassFixture<CatdataBankWebAppFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public CatDataBankFuncTest(CatdataBankWebAppFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Auth_Success()
        {
            // Arrange
            var user = new { email = "test@test.com", password = "123456" };
            var body = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(body);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/v1/auth", httpContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var returned = await response.Content.ReadAsStringAsync();
            var desReturned = JsonConvert.DeserializeObject<AuthResponse>(returned);
            desReturned.username.Should().Be(user.email);
            desReturned.token.Should().NotBeNull();
        }

        [Fact]
        public async Task Auth_Fail()
        {
            // Arrange
            var user = new { email = "test2@test2.com", password = "123456" };
            var body = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(body);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/v1/auth", httpContent);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            var returned = await response.Content.ReadAsStringAsync();
            var desReturned = JsonConvert.DeserializeObject<AuthFailResponse>(returned);
            desReturned.message.Should().Be("Les informations fournies sont incorrects");
        }

        [Fact]
        public async Task Cat_Success()
        {
            var client = _factory.CreateClient();

            var user = new { email = "test@test.com", password = "123456" };
            var body = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(body);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync("/api/v1/auth", httpContent);
            var returned = await response.Content.ReadAsStringAsync();
            var desReturned = JsonConvert.DeserializeObject<AuthResponse>(returned);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", desReturned.token);

            var cats = await client.GetAsync("/api/v1/cat");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Cat_NoAccess()
        {
            var client = _factory.CreateClient();

            var user = new { email = "test@test.com", password = "123456" };
            var body = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(body);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync("/api/v1/auth", httpContent);
            var returned = await response.Content.ReadAsStringAsync();
            var desReturned = JsonConvert.DeserializeObject<AuthResponse>(returned);

            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", desReturned.token);

            var cats = await client.GetAsync("/api/v1/cat");

            // Assert
            cats.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}