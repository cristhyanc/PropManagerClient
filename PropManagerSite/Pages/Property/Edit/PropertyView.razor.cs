using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using PropManagerSite.GraphQL;
using PropManagerSite.Shared;
using PropManagerSite.Shared.Notification;

namespace PropManagerSite.Pages.Property.Edit
{
    public class PropertyViewBase : PropComponentBase
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Tab { get; set; }


        [Inject]
        PropManagerSiteQL PropManagerSiteQL { get; set; } = default!;

        public IPropertyDetails PropertyDetails { get; set; } = default!;

        protected int SelectedTab = 0;

        protected override async Task OnInitializedAsync()
        {
            this.IsBusy = true;
            try
            {
                switch (Tab)
                {
                    case "loans":
                        {
                            SelectedTab = 1;
                            break;
                        }
                    default:
                        break;
                }
                await LoanProperty();


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

        protected async Task LoanProperty()
        {
            if (!Id.IsNullOrEmpty())
            {
                var propertyId = Guid.Parse(Id);
                var result = await PropManagerSiteQL.GetProperty.ExecuteAsync(propertyId);
                if (result.Data?.Properties.Count > 0)
                {
                    PropertyDetails = result.Data.Properties.First();
                }

            }
        }

        protected void PropertySaved(IPropertyDetails details)
        {
            this.PropertyDetails = details;
        }
    }
}
