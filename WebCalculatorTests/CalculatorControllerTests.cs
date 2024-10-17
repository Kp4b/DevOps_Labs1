using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace WebCalculatorTests
{
    public class CalculatorControllerTests : IClassFixture<WebApplicationFactory<WebCalculator.Program>>
    {
        private readonly HttpClient _client;

        public CalculatorControllerTests(WebApplicationFactory<WebCalculator.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Index_ReturnsSuccessStatusCode()
        {
            // Act  
            var response = await _client.GetAsync("/Calculator/Index");

            // Assert  
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Calculator", responseString);
        }

        [Fact]
        public async Task Calculate_ValidExpression_ReturnsResult()
        {
            // Arrange  
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("expression", "1+1")
            });

            // Act  
            var response = await _client.PostAsync("/Calculator/Calculate", content);

            // Assert  
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Проверка наличия результата в HTML-ответе
            Assert.Contains("2", responseString);
        }



        [Fact]
        public async Task CalculateFromFile_ValidFile_ReturnsResultFilePath()
        {
            // Arrange  
            var fileContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("1+1"));
            var content = new MultipartFormDataContent
               {
                   { fileContent, "file", "test.txt" }
               };

            // Act  
            var response = await _client.PostAsync("/Calculator/CalculateFromFile", content);

            // Assert  
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Result File Path:", responseString);
        }
    }
}
