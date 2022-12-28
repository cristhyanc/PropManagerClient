using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using PropManagerSite.GraphQL;
using PropManagerSite.Pages.Property.Edit.Loan;
using PropManagerSite.Shared;
using PropManagerSite.Shared.Notification;
using System.ComponentModel.DataAnnotations;
using static PropManagerSite.Pages.Property.Edit.Expense.CreateExpenseBase;

namespace PropManagerSite.Pages.Property.Edit.Expense
{
    public class EditExpenseBase : PropComponentBase
    {
        [Parameter]
        public string PropertyId { get; set; } = default!;

        [Parameter]
        public string ExpenseId { get; set; } = default!;

        protected EditExpenseModel ExpenseModel { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            this.IsBusy = true;
            try
            {
                if (!ExpenseId.IsNullOrEmpty())
                {
                    var id = Guid.Parse(this.ExpenseId);
                    var result = await PropManagerSiteQL.GetExpense.ExecuteAsync(id);
                    if (result.Errors.IsNullOrEmpty())
                    {
                        if (result.Data?.Expenses.Count > 0)
                        {
                            var expenseDetails = result.Data.Expenses.First();                            
                            ExpenseModel.Title = expenseDetails.Title;
                            ExpenseModel.Price = expenseDetails.Price;
                            ExpenseModel.ExpenseDate = expenseDetails.ExpenseDate;
                            ExpenseModel.Description = expenseDetails.Description;
                            ExpenseModel.TotalDeductable = expenseDetails.TotalDeductable;
                            ExpenseModel.DueDate = expenseDetails.DueDate;
                            ExpenseModel.Paid = expenseDetails.Paid;
                            ExpenseModel.CompanyName = expenseDetails.CompanyName;
                            ExpenseModel.Reference = expenseDetails.Reference;
                            
                        }
                    }
                    else
                    {
                        await NotificationsService.ToastError(result.Errors);
                    }
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

        protected async Task Save()
        {
            this.IsBusy = true;
            try
            {
                var input = new EditExpenseInput()
                {
                    Price = ExpenseModel.Price,
                    Title = ExpenseModel.Title,
                    Description = ExpenseModel.Description,
                    ExpenseDate = ExpenseModel.ExpenseDate!.Value,
                    TotalDeductable = ExpenseModel.TotalDeductable,
                    CompanyName = ExpenseModel.CompanyName,
                    Paid = ExpenseModel.Paid,
                    DueDate = ExpenseModel.DueDate,
                    Reference= ExpenseModel.Reference,
                    Id =Guid.Parse(this.ExpenseId)
                };

                var result = await PropManagerSiteQL.EditExpense.ExecuteAsync(input);
                if (result.Errors.IsNullOrEmpty())
                {
                    await NotificationsService.ToastSuccess("Saved!");
                    this.NavManager.NavigateTo($"property/{PropertyId}/expenses");
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
            this.NavManager.NavigateTo($"property/{PropertyId}/expenses");
        }
        public record EditExpenseModel
        {

            [Required]
            public string Title { get; set; } = default!;
            public string? Description { get; set; }
            public decimal? Price { get; set; }
            public decimal TotalDeductable { get; set; } = 100;
            [Required]
            public DateTimeOffset? ExpenseDate { get; set; }
            public string? CompanyName { get; set; }
            public string? Reference { get; set; }
            public DateTimeOffset? DueDate { get; set; }
            public bool Paid { get; set; }
        }

    }
}
