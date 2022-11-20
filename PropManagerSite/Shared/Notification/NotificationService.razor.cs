using Microsoft.AspNetCore.Components;
using StrawberryShake;
using Syncfusion.Blazor.Notifications;

namespace PropManagerSite.Shared.Notification
{
    public class NotificationsService: ComponentBase
    {        

        public static async Task ToastError(string message)
        {           
            await ErrorToastNotificationBase.ShowAsync(message);
        }

        public static async Task ToastError(IReadOnlyList<IClientError> errors)
        {
            var message=string.Join(", ", errors.Select(e => e.Message));
            await ErrorToastNotificationBase.ShowAsync(message);
        }
        
        public static async Task ToastWarning(string title, string message)
        {           
            await WarningToastNotificationBase.ShowAsync(title, message);
        }

        public static async Task ToastSuccess(string message, string? title=null)
        {
            await SuccessToastNotificationBase.ShowAsync( message, title);
        }
    }
}
