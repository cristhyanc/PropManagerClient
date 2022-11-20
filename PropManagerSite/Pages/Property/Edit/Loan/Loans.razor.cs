using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using PropManagerSite.GraphQL;
using PropManagerSite.Shared;
using PropManagerSite.Shared.Notification;

namespace PropManagerSite.Pages.Property.Edit.Loan
{
    public class LoansBase: PropComponentBase
    {

        [Inject]
        NavigationManager NavManager { get; set; } = default!;

        [Parameter]
        public IPropertyDetails? PropertyDetails { get; set; }

        [Inject]
        PropManagerSiteQL PropManagerSiteQL { get; set; } = default!;

        [Parameter]
        public EventCallback OnLoanDeleted { get; set; }

        protected void NewLoan()
        {
            this.NavManager.NavigateTo($"property/{this.PropertyDetails!.Id}/loan/new");
            
        }

        protected void EditLoan(IEditProperty_EditProperty_Property_Loans loan)
        {
            this.NavManager.NavigateTo($"property/{this.PropertyDetails!.Id}/loan/{loan.Id}");
        }

        protected async Task DeleteLoan(IEditProperty_EditProperty_Property_Loans loan)
        {
            this.IsBusy = true;
            try
            {
                var result = await PropManagerSiteQL.DeleteLoan.ExecuteAsync(new() { Id = loan.Id });

                if (result.Errors.IsNullOrEmpty())
                {
                    await NotificationsService.ToastSuccess("Deleted!");
                    await this.OnLoanDeleted.InvokeAsync();
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
