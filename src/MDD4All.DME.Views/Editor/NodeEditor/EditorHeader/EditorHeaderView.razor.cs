using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Threading.Tasks;
using MDD4All.DME.ViewModels.Editor;
using MDD4All.DME.ViewModels.Editor.Settings;

namespace MDD4All.DME.Views.Editor
{
    public partial class EditorHeaderView
    {
        [Parameter]
        public EventCallback<EditorAction> OnAction { get; set; }

        [Parameter]
        public ObjectEditorViewModel DataContext { get; set; } = null!;

        [Inject]
        public EditorAppearanceSettingsViewModel Settings { get; set; } = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Settings.PropertyChanged += OnSettingsPropertyChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            Settings.PropertyChanged -= OnSettingsPropertyChanged;
        }

        private void OnSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        protected async Task Notify(EditorAction action)
        {
            await OnAction.InvokeAsync(action);
        }

        // Clicking the title is the only way to make a node the root of the editor, and nothing
        // on the card says so. An object that does not exist yet cannot be selected, so it says
        // why instead.
        private string SelectLabelTooltip
        {
            get
            {
                string result = L["Tooltip.SelectAsRoot"];

                if (DataContext.IsNull)
                {
                    result = L["Tooltip.NotCreatedYet"];
                }

                return result;
            }
        }

        private async Task OnSelectLabel()
        {
            // The action only runs when the object is not null
            if (!DataContext.IsNull)
            {
                await Notify(EditorAction.Select);
            }
        }
    }
}