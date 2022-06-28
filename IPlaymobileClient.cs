using PlayMobile.Models;

namespace PlayMobile
{
    public interface IPlaymobileClient
    {
        Task<Response> SendSmsAsync(string phone, string message);
    }
}
