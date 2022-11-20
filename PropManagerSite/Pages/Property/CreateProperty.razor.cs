using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using PropManagerSite.GraphQL;
using PropManagerSite.Shared;
using PropManagerSite.Shared.Notification;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace PropManagerSite.Pages.Property
{
    public class CreatePropertyBase : PropComponentBase
    {
        [Inject]
        PropManagerSiteQL PropManagerSiteQL { get; set; } = default!;

        protected CreatePropertyModel addPropertyModel = new();

        [Inject]
        NavigationManager NavManager { get; set; } = default!;

        public string[] EnumValues = Enum.GetNames(typeof(PropertyTypes));
        
      
        protected void Cancel()
        {
            this.NavManager.NavigateTo("properties");
        }

        protected async Task Save()
        {
            this.IsBusy = true;
            try
            {
                var newProperty = new AddPropertyInput();
                newProperty.Name = addPropertyModel.Name;
                newProperty.Carpark = addPropertyModel.Carpark;
                newProperty.Bathrooms = addPropertyModel.Bathrooms;
                newProperty.Rooms = addPropertyModel.Rooms;
                newProperty.LandSize = addPropertyModel.LandSize;
                newProperty.PropertyType = addPropertyModel.PropertyType;
                newProperty.StampDuty = addPropertyModel.StampDuty;
                newProperty.RegistrationTransferFee = addPropertyModel.RegistrationTransferFee;
                newProperty.PurchasePrice = addPropertyModel.PurchasePrice;
                newProperty.Address = addPropertyModel.Address;

                var result = await PropManagerSiteQL.CreateProperty.ExecuteAsync(newProperty);
                if (result.Errors.IsNullOrEmpty())
                {                    
                    await NotificationsService.ToastSuccess("Saved!");
                    this.NavManager.NavigateTo($"property/{result.Data?.AddProperty?.Property?.Id}");
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

        protected record CreatePropertyModel
        {
            [Required]
            public string Address { get; set; } = null!;
            [Required]
            public string Name { get; set; } = null!;
            public decimal? StampDuty { get; set; } = default;
            public decimal? PurchasePrice { get; set; } = default;
            public decimal? RegistrationTransferFee { get; set; } = default;
            public decimal? Rooms { get; set; } = default;
            public decimal? Bathrooms { get; set; } = default;
            public decimal? Carpark { get; set; } = default;
            public decimal? LandSize { get; set; } = default;
            public PropertyTypes? PropertyType { get; set; }
        }
    }
  
}
