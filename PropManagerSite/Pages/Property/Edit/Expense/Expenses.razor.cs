using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using PropManagerSite.GraphQL;
using PropManagerSite.Shared;
using PropManagerSite.Shared.Notification;

namespace PropManagerSite.Pages.Property.Edit.Expense
{
    public class ExpensesBase: PropComponentBase
    {

        [Parameter]
        public IPropertyDetails? PropertyDetails { get; set; }
              

        [Parameter]
        public EventCallback OnExpenseDeleted { get; set; }

        protected void NewExpense()
        {
            this.NavManager.NavigateTo($"property/{this.PropertyDetails!.Id}/expense/new");

        }

        protected void EditExpense(IEditProperty_EditProperty_Property_Expenses_Expense expense)
        {
            this.NavManager.NavigateTo($"property/{this.PropertyDetails!.Id}/expense/{expense.Id}");
        }

        protected async Task DeleteExpense(IEditProperty_EditProperty_Property_Expenses_Expense expense)
        {
            this.IsBusy = true;
            try
            {
                var result = await PropManagerSiteQL.DeleteExpense.ExecuteAsync(new() { Id = expense.Id });

                if (result.Errors.IsNullOrEmpty())
                {
                    await NotificationsService.ToastSuccess("Deleted!");
                    await this.OnExpenseDeleted.InvokeAsync();
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
