using CRC.Common.Notifications.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CRC.Common.Notifications.Extensions;


/// Extension methods for registering CRC notification services
public static class ServiceCollectionExtensions
{
   
    /// Adds CRC notification services to the dependency injection container
    public static IServiceCollection AddCRCNotifications(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
        return services;
    }
}
