using article_jwt_auth.Models;

namespace article_jwt_auth.Services;

public static class AppSettingsService
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
