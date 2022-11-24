using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using PropManagerSite.GraphQL;
using PropManagerSite.Shared;
using PropManagerSite.Shared.Notification;
using System.ComponentModel.DataAnnotations;

namespace PropManagerSite.Pages.Property.Edit.Loan
{
    public class EditLoanBase: PropComponentBase
    {
        protected record TypeItem(string Display, LoanTypes Value);

        [Parameter]
        public string PropertyId { get; set; } = default!;

        [Parameter]
        public string LoanId { get; set; } = default!;

        protected EditCreateLoan editLoanForm = new();

        protected IGetLoan_Loans? loanDetails;       

        protected List<TypeItem> EnumValues = new() { (new TypeItem("Interest Only", LoanTypes.InterestOnly)), (new TypeItem("Principal And Interest", LoanTypes.PrincipalAndInterest)) };

        protected override async Task OnInitializedAsync()
        {
            this.IsBusy = true;
            try
            {    
                if (!LoanId.IsNullOrEmpty())
                {
                    var id = Guid.Parse(this.LoanId);
                    var result = await PropManagerSiteQL.GetLoan.ExecuteAsync(id);
                    if (result.Errors.IsNullOrEmpty())
                    {
                        if (result.Data?.Loans.Count > 0)
                        {
                            loanDetails = result.Data.Loans.First();
                            editLoanForm.Amount = loanDetails.Amount is null? 0: loanDetails.Amount.Value;
                            editLoanForm.Interest = loanDetails.Interest;
                            editLoanForm.LenderName = loanDetails.LenderName;
                            editLoanForm.LMI = loanDetails.LMI;
                            editLoanForm.LoanType = loanDetails.LoanType;
                            editLoanForm.Years = loanDetails.Years;
                            editLoanForm.DateOfLoan = loanDetails.DateOfLoan;

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
                var input = new EditLoanInput()
                {
                    Id = this.loanDetails!.Id,
                    Amount = editLoanForm.Amount,
                    Interest = editLoanForm.Interest,
                    LenderName = editLoanForm.LenderName,
                    LMI = editLoanForm.LMI,
                    LoanType = editLoanForm.LoanType,
                    Years = editLoanForm.Years,
                    DateOfLoan=editLoanForm.DateOfLoan
                    
                };

                var result = await PropManagerSiteQL.EddLoan.ExecuteAsync(input);
                if (result.Errors.IsNullOrEmpty())
                {
                    await NotificationsService.ToastSuccess("Saved!");
                    this.NavManager.NavigateTo($"property/{this.PropertyId}/loans");
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
            this.NavManager.NavigateTo($"property/{this.PropertyId}/loans");
        }

    }

    public record EditCreateLoan
    {
        [Required]
        public string LenderName { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public decimal? Interest { get; set; }
        public LoanTypes LoanType { get; set; }
        public decimal? LMI { get; set; }
        public int? Years { get; set; }
        public DateTimeOffset? DateOfLoan { get; set; }

    }
}
