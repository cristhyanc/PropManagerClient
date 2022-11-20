using Microsoft.AspNetCore.Components;

namespace PropManagerSite.Shared
{
    public class MainSeccionComponentBase : ComponentBase
    {
        bool isBusy;

        [Parameter]
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                StateHasChanged();
            }
        }

        [Parameter]
        public RenderFragment ChildContent { get; set; } = default!;
    }
}
