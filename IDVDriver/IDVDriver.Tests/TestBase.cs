using Microsoft.Extensions.Configuration;

namespace IDVDriver.Tests
{
    public abstract class TestBase
    {
        protected string ConnectionString = InitConfiguration()["connectionString"];

        protected abstract void SetUp();

        protected abstract void Dispose();

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }
    }
}