using System;
using System.Threading.Tasks;
using RestEase;
using U.Common.Subscription;

namespace U.NotificationService.Application.Common.Clients
{
    public interface ISubscriptionService
    {
        [Post("api/subscription/signalr/{userId}/bind")]
        [AllowAnyStatusCode]
        Task BindConnectionToUserAsync([Path] Guid userId, string connectionId);

        [Post("api/subscription/signalr/{userId}/unbind")]
        [AllowAnyStatusCode]
        Task UnbindConnectionToUserAsync([Path] Guid userId, string connectionId);

        [Get("api/subscription/preferences/{userId}")]
        [AllowAnyStatusCode]
        Task<Preferences> GetMyPreferencesAsync([Path] Guid userId);
    }
}