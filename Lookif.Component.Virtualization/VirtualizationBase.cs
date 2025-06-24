using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Lookif.Component.Virtualization;
public class VirtualizationBase<TItem> : ComponentBase
{
    [Parameter] public ScrollTrigger PerformScrollTrigger { get; set; }
    [Parameter, AllowNull] public int Length { get; set; } = 50;
    [Parameter] public RenderFragment<TItem>? ItemTemplate { get; set; }
    [Parameter] public List<TItem> Items { get; set; }




    public delegate Task<IEnumerable<TItem>> ScrollTrigger(int page, int length);
    public readonly Guid Identity;
    public DotNetObjectReference<VirtualizationBase<TItem>> objRef { get; set; }
    public LFVirtualizationJSInterop<TItem> _lFVirtualizationJSInterop { get; set; }


    [Inject] IJSRuntime _jSRuntime { get; set; }

    int currentPage = 0;

    public VirtualizationBase()
    {
        Identity = Guid.NewGuid();
    }
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && _lFVirtualizationJSInterop is null)
        {
            objRef = DotNetObjectReference.Create(this);
            if (_jSRuntime is not null)
            {
                _lFVirtualizationJSInterop = new LFVirtualizationJSInterop<TItem>(_jSRuntime);
                await _lFVirtualizationJSInterop.SetOrUnsetInstance(objRef, Identity, true);
            }
        }
    }
    [JSInvokable("ScrollTrigger")]
    public async Task ScrollTriggerFunction()
    {
        currentPage++;
        var newItems = await PerformScrollTrigger.Invoke(currentPage, Length);
        Items.AddRange(newItems.ToList()); //ToDo Check for duplicates
        StateHasChanged();
    }

    #region ...IDisposable...
    public void Dispose()
    {
        objRef?.Dispose();
    }
    #endregion
}
