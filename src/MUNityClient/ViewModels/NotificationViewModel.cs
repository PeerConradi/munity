using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.ViewModels
{
    public class NotificationViewModel
    {
        public enum ENotificationTypes
        {
            Info,
            Warning,
            Error
        }

        public ENotificationTypes Type { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
    }
}
