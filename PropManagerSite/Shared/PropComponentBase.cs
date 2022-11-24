using Microsoft.AspNetCore.Components;
using PropManagerSite.GraphQL;

namespace PropManagerSite.Shared
{
    public class PropComponentBase: ComponentBase
    {
        protected bool IsBusy { get; set; }
        [Inject]
        protected PropManagerSiteQL PropManagerSiteQL { get; set; } = default!;

        [Inject]
        protected NavigationManager NavManager { get; set; } = default!;
    }
}
