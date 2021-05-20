using System.Threading.Tasks;
using Zero.Authorization.Users;

namespace Zero.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
