using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.User;

namespace MUNity.Database.Model.User
{
    public class UserNotification
    {
        public int UserNotificationId { get; set; }

        public MunityUser User { get; set; }

        public string Title { get; set; }

        public UserNotificationCategory Category { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsRead { get; set; }
    }
}
