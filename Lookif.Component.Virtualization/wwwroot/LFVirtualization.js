export function SetOrUnsetInstance(dotNetHelper, identity, IsItSet) { 
    if (window.LFVirtualizations == undefined)
        window.LFVirtualizations = [];
    var obj = { "instance": dotNetHelper, "identity": identity };

    if (IsItSet) {
        window.LFVirtualizations.push(obj);
    }

    else {
        removeItemOnce(window.LFVirtualizations, identity);
    }

    $(window).scroll(function () { 
        if ($(window).scrollTop() + $(window).height() > $(document).height() - 100) {
            dotNetHelper.invokeMethodAsync('ScrollTrigger');
        }
    });
    


}


function removeItemOnce(arr, identity) {
    window.LFVirtualizations = window.LFVirtualizations.filter(function (item) { return item.identity != identity })
}