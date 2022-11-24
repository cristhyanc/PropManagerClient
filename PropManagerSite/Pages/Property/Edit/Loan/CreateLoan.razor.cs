using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using PropManagerSite.GraphQL;
using PropManagerSite.Shared;
using PropManagerSite.Shared.Notification;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PropManagerSite.Pages.Property.Edit.Loan
{
    public class CreateLoanBase: PropComponentBase
    {
        protected record TypeItem(string Display, LoanTypes Value);

        protected AddCreateLoan addCreateLoan = new();
              
        [Parameter]
        public string Id { get; set; }

        protected Guid PropertyId
        {
            get { return Guid.Parse(Id); }
        }

        protected List<TypeItem> EnumValues = new() { (new TypeItem("Interest Only", LoanTypes.InterestOnly)), (new TypeItem("Principal And Interest", LoanTypes.PrincipalAndInterest)) };
       
        protected async Task Save()
        {
            this.IsBusy = true;
            try
            {
                var input = new AddLoanInput()
                {
                    Amount = addCreateLoan.Amount,
                    Interest = addCreateLoan.Interest,
                    LenderName = addCreateLoan.LenderName,
                    LMI = addCreateLoan.LMI,
                    LoanType = addCreateLoan.LoanType,
                    Years   = addCreateLoan.Years,
                    PropertyId = this.PropertyId,
                    DateOfLoan = addCreateLoan.DateOfLoan
                };

               var result= await PropManagerSiteQL.AddLoan.ExecuteAsync(input);
                if (result.Errors.IsNullOrEmpty())
                {
                    await NotificationsService.ToastSuccess("Saved!");
                    this.NavManager.NavigateTo($"property/{Id}/loans");
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
            this.NavManager.NavigateTo($"property/{Id}/loans");
        }
    }

    public record AddCreateLoan
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
