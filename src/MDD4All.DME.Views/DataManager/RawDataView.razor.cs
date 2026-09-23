using MDD4All.DME.ViewModels.DataManager;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace MDD4All.DME.Views.DataManager
{
    public partial class RawDataView
    {
        [Inject] private IJSRuntime _js { get; set; } = null!;

        // The text comes from the file manager, which knows both the object and the settings it
        // has to be written with.
        [Parameter]
        public DataManagerFileViewModel DataContext { get; set; } = null!;

        // Only for the JSON/XML switch.
        [Inject] private EditorViewModel Editor { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _js.InvokeVoidAsync("highlightSnippet");
        }
    }
}
