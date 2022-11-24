using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using PropManagerSite.GraphQL;
using PropManagerSite.Shared;
using PropManagerSite.Shared.Notification;

namespace PropManagerSite.Pages
{
    public  class PropertiesBase: PropComponentBase
    {
        public List<IGetProperties_Properties> Properties { get; set; } = new List<IGetProperties_Properties>();

        protected override async Task OnInitializedAsync()
        {
            await LoadProperties();
        }

        async Task LoadProperties()
        {
            this.IsBusy = true;
            try
            {
                var data = await PropManagerSiteQL.GetProperties.ExecuteAsync();
                if (data.Data is not null)
                {
                    this.Properties = data.Data.Properties.ToList();
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

        protected void AddNewProperty()
        {
            this.NavManager.NavigateTo("property/new");
        }

        public async Task DeleteProperty(IGetProperties_Properties args)
        {
            this.IsBusy = true;
            try
            {
                var result = await PropManagerSiteQL.DeleteProperty.ExecuteAsync(new() { Id= args.Id });
                if (result.Errors.IsNullOrEmpty())
                {                  
                    await NotificationsService.ToastSuccess("Deleted!");
                    await LoadProperties();
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
        public void EditProperty(IGetProperties_Properties args)
        {
            this.NavManager.NavigateTo($"property/{args.Id}");
        }
    }
}
