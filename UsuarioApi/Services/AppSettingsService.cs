using UsuarioApi.Models;

namespace UsuarioApi.Services;

public class AppSettingsService
{
    public static JwtSettings JwtSettings { get; }

    static AppSettingsService()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        JwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();
    }
}