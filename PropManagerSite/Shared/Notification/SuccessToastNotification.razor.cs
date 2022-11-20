using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Notifications;

namespace PropManagerSite.Shared.Notification
{
    public class SuccessToastNotificationBase:ComponentBase
    {
        protected static SfToast? successToast;
        public static async Task ShowAsync(string message, string? title = null)
        {
            successToast!.Content = message;
            successToast.Title = title;
            await successToast!.ShowAsync();
        }
    }
}
