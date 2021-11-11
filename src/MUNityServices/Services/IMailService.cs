namespace MUNity.Services
{
    public interface IMailService
    {
        void SendMail(string target, string title, string content);
    }
}