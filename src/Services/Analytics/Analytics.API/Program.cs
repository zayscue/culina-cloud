using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using CulinaCloud.Analytics.Infrastructure.Persistence;
using CulinaCloud.BuildingBlocks.WebHost.Customization;

namespace CulinaCloud.Analytics.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDbContext<ApplicationDbContext>()
                .Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static async Task PredictHandler(HttpContext http)
        {
            // Get PredictionEnginePool service
            var predictionEnginePool = http.RequestServices
                .GetRequiredService<PredictionEnginePool<
                    CollaborativeFilteringRecipeRecommendations.ModelInput,
                    CollaborativeFilteringRecipeRecommendations.ModelOutput>>();

            // Deserialize HTTP request JSON body
            var body = http.Request.Body;
            var input = await JsonSerializer
                .DeserializeAsync<CollaborativeFilteringRecipeRecommendations.ModelInput>(body);

            // Predict
            var prediction = predictionEnginePool.Predict(input);

            // Return prediction as response
            await http.Response.WriteAsJsonAsync(prediction);
        }
    }
}