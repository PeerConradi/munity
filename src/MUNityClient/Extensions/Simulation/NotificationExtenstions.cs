using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityClient.ViewModels;

namespace MUNityClient.Extensions.Simulation
{
    public static class NotificationExtenstions
    {
        public static void ShowInfo(this SimulationViewModel viewModel, string title, string text)
        {
            var mdl = new NotificationViewModel()
            {
                Type = NotificationViewModel.ENotificationTypes.Info,
                Text = text,
                Title = title
            };
            viewModel.CurrentNotification = mdl;
        }

        public static void ShowInfo(this SimulationViewModel viewModel, string text)
        {
            var mdl = new NotificationViewModel()
            {
                Type = NotificationViewModel.ENotificationTypes.Info,
                Text = text,
                Title = "MUNity"
            };
            viewModel.CurrentNotification = mdl;
        }

        public static void ShowError(this SimulationViewModel viewModel, string title, string text)
        {
            var mdl = new NotificationViewModel()
            {
                Type = NotificationViewModel.ENotificationTypes.Error,
                Text = text,
                Title = title
            };
            viewModel.CurrentNotification = mdl;
        }
    }
}
