using System.Threading.Tasks;

namespace Zero.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}