using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Notifications;

namespace PropManagerSite.Shared.Notification
{
    public class WarningToastNotificationBase: ComponentBase
    {
        protected static SfToast? warningToast;
        public static async Task ShowAsync(string title, string message)
        {
            warningToast!.Content = message;
            await warningToast!.ShowAsync();
        }
    }
}
