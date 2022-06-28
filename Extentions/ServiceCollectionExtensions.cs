using System.Net.Http.Headers;
using System.Text;

namespace PlayMobile.Extentions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPlaymobileClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IPlaymobileClient, PlaymobileClient>(client =>
        {
            var options = configuration.GetSection("Messaging:Playmobile").Get<Options>();

            client.BaseAddress = new Uri(options.BaseUrl ?? throw new ArgumentNullException("Not"));

            var authString = $"{options.Login}:{options.Password}";
            var authBase64String = Convert.ToBase64String(Encoding.ASCII.GetBytes(authString));

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authBase64String);
        });
        return services;
    }
}
