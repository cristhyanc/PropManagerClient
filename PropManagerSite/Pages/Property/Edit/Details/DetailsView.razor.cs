using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using PropManagerSite.GraphQL;
using PropManagerSite.Shared;
using PropManagerSite.Shared.Notification;
using Syncfusion.Blazor.Notifications;

namespace PropManagerSite.Pages.Property.Edit.Details
{
    public class DetailsViewBase: PropComponentBase
    {              
        public EditPropertyInput formEditInput = new();

        [Parameter]
        public IPropertyDetails PropertyDetails { get; set; } = default!;

        [Parameter]
        public EventCallback<IPropertyDetails> OnPropertySaved { get; set; }

        public string[] EnumValues = Enum.GetNames(typeof(PropertyTypes));
      
        protected override void OnParametersSet()
        {
            if(PropertyDetails is not null)
            {
                formEditInput.Name = PropertyDetails.Name!;
                formEditInput.Address = PropertyDetails.Address;
                formEditInput.PropertyType = PropertyDetails.PropertyType;
                formEditInput.PurchasePrice = PropertyDetails.PurchasePrice;
                formEditInput.StampDuty = PropertyDetails.StampDuty;
                formEditInput.RegistrationTransferFee = PropertyDetails.RegistrationTransferFee;
                formEditInput.Rooms = PropertyDetails.Rooms;
                formEditInput.Bathrooms = PropertyDetails.Bathrooms;
                formEditInput.Carpark = PropertyDetails.Carpark;
                formEditInput.LandSize = PropertyDetails.LandSize;
                formEditInput.Id=PropertyDetails.Id;
            }
        }

        protected  void Cancel()
        {
            this.NavManager.NavigateTo("properties");
        }

        protected async Task Save()
        {
            this.IsBusy = true;
            try
            {
                var result = await PropManagerSiteQL.EditProperty.ExecuteAsync(formEditInput);
                if (result.Errors.IsNullOrEmpty())
                {
                    await OnPropertySaved.InvokeAsync(result.Data!.EditProperty.Property);
                    await NotificationsService.ToastSuccess("Saved!");
                }
                else
                {
                    await NotificationsService.ToastError(result.Errors);
                }
            }
            catch (Exception ex)
            {
                await NotificationsService.ToastError(ex.Message);
                throw;
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
