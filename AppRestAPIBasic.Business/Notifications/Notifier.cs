using AppRestAPIBasic.Business.Interfaces;

namespace AppRestAPIBasic.Business.Notifications
{
    public class Notifier : INotifier
    {
        readonly List<Notification> _notifications;
        public Notifier()
        {
            _notifications = new List<Notification>();
        }
        public List<Notification> GetNotifications() => _notifications;
        public void Handle(Notification notification) => _notifications.Add(notification);
        public bool IsThereNotification() => _notifications.Any();
    }
}
