namespace MUNity.BlazorServer.BServices
{
    public class ScopedSessionNotificationService
    {
        public event EventHandler<Notification> NotificationCreated;

        public void Notify(string title, string text)
        {
            NotificationCreated?.Invoke(this, new Notification(title, text));
        }
    }

    public class Notification
    {
        public string Title { get; private set; }

        public string Text { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }

        public Notification(string title, string text)
        {
            this.Title = title;
            this.Text = text;
            Timestamp = DateTimeOffset.Now;
        }
    }
}
