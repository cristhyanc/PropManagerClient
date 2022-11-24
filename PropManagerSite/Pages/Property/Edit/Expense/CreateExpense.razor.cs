using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using PropManagerSite.GraphQL;
using PropManagerSite.Shared;
using PropManagerSite.Shared.Notification;
using System.ComponentModel.DataAnnotations;

namespace PropManagerSite.Pages.Property.Edit.Expense
{
    public class CreateExpenseBase: PropComponentBase
    {
        [Parameter]
        public string Id { get; set; }

        protected AddExpenseModel ExpenseModel { get; set; } = new();

        protected Guid PropertyId
        {
            get { return Guid.Parse(Id); }
        }

        protected async Task Save()
        {
            this.IsBusy = true;
            try
            {
                var input = new AddExpenseInput()
                {
                    Price = ExpenseModel.Price,
                    Title = ExpenseModel.Title,
                    Description = ExpenseModel.Description,
                    ExpenseDate = ExpenseModel.ExpenseDate!.Value,
                    TotalDeductable= ExpenseModel.TotalDeductable,
                    PropertyId=this.PropertyId
                };

                var result = await PropManagerSiteQL.AddExpense.ExecuteAsync(input);
                if (result.Errors.IsNullOrEmpty())
                {
                    await NotificationsService.ToastSuccess("Saved!");
                    this.NavManager.NavigateTo($"property/{Id}/expenses");
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

        protected void Cancel()
        {
            this.NavManager.NavigateTo($"property/{Id}/expenses");
        }

        public record AddExpenseModel
        {
            
            [Required]
            public string Title { get; set; } = default!;
            public string? Description { get; set; }
            public decimal? Price { get; set; }
            public decimal TotalDeductable { get; set; } = 100;
            [Required]
            public DateTimeOffset? ExpenseDate { get; set; }
        }
    }
}
