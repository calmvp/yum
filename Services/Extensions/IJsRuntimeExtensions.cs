using Microsoft.JSInterop;

namespace Yum.Services.Extensions
{
    public static class IJsRuntimeExtensions
    {
        public static async Task ToastrSuccess(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("showToastr", "success", message);
        }

        public static async Task ToastrError(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("showToastr", "error", message);
        }

        public static async Task OpenModal(this IJSRuntime jsRuntime, string modalId)
        {
            await jsRuntime.InvokeVoidAsync("openModal", modalId);
        }

        public static async Task CloseModal(this IJSRuntime jsRuntime, string modalId)
        {
            await jsRuntime.InvokeVoidAsync("closeModal", modalId);
        }
    }
}
