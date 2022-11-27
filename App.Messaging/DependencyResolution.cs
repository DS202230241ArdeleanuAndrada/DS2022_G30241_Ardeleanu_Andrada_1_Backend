using Microsoft.Extensions.DependencyInjection;

namespace App.Messaging
{
    public static class DependencyResolution
    {
        public static void RegisterMessaging(this IServiceCollection services)
        {
            services.AddHostedService<MessagingService>();
        } 
    }
}
