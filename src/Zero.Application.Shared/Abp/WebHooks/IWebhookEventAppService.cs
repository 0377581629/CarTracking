using System.Threading.Tasks;
using Abp.Webhooks;

namespace Zero.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
