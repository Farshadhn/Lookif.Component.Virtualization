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
    let isLoading = false;
    $(window).scroll(function () {
        if (isLoading) return; // already loading, do nothing

        if ($(window).scrollTop() + $(window).height() > $(document).height() - 100) {

            isLoading = true;

            dotNetHelper.invokeMethodAsync('ScrollTrigger').then(() => {
                isLoading = false;
            }).catch(() => {
                isLoading = false;
            });
        }
    });



}


function removeItemOnce(arr, identity) {
    window.LFVirtualizations = window.LFVirtualizations.filter(function (item) { return item.identity != identity })
}