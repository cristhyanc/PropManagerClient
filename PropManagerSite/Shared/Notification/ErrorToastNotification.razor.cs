using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Notifications;

namespace PropManagerSite.Shared.Notification
{
    public class ErrorToastNotificationBase: ComponentBase
    {
        protected static SfToast? errorToast;

        public static async Task ShowAsync(string message)
        {           
            errorToast!.Content = message;
            await errorToast!.ShowAsync();
        }
    }
}
