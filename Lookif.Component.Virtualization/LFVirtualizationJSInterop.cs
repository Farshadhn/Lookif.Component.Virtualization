 
using Microsoft.JSInterop;

namespace Lookif.Component.Virtualization;


public class LFVirtualizationJSInterop<TItem> : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public LFVirtualizationJSInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Lookif.Component.Virtualization/LFVirtualization.js").AsTask());
    }
    public async ValueTask SetOrUnsetInstance(DotNetObjectReference<VirtualizationBase<TItem>> dotNetObjectReference, Guid identity, bool IsItSet)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("SetOrUnsetInstance", dotNetObjectReference, identity, IsItSet);
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
