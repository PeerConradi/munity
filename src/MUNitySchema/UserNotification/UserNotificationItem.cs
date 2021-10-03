using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.UserNotification
{
    public class UserNotificationItem
    {
        public int NotificationId { get; set; }

        public string Title { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
