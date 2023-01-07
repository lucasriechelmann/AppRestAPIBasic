using System.Collections.Generic;
using AppRestAPIBasic.Business.Notifications;

namespace AppRestAPIBasic.Business.Interfaces
{
    public interface INotifier
    {
        bool IsThereNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
    }
}
