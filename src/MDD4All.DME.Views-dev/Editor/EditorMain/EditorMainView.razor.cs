using MDD4All.DME.ViewModels.DataManager;
using MDD4All.DME.ViewModels.Editor;
using MDD4All.DME.ViewModels.Editor.Settings;
using MDD4All.UI.DataModels.Tree;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MDD4All.DME.Views.Editor
{
    public partial class EditorMainView
    {
        [Inject] private IJSRuntime JSRuntime { get; set; } = null!;


        [Inject]
        public MainViewModel Navigation { get; set; } = null!;

        [Inject]
        public EditorViewModel Editor { get; set; } = null!;

        [Inject]
        public DataManagerFileViewModel DataFile { get; set; } = null!;

        [Inject]
        public EditorAppearanceSettingsViewModel EditorSettings { get; set; } = null!;

        [Inject]
        public ExplorerSettingsViewModel ExplorerSettings { get; set; } = null!;

        #region Lifecycle
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.Editor.PropertyChanged += this.OnEditorPropertyChanged;
            EditorSettings.PropertyChanged += OnEditorSettingsPropertyChanged;
            ExplorerSettings.PropertyChanged += OnExplorerSettingsPropertyChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ApplyTintIntensity();
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Editor.PropertyChanged -= this.OnEditorPropertyChanged;
            EditorSettings.PropertyChanged -= OnEditorSettingsPropertyChanged;
            ExplorerSettings.PropertyChanged -= OnExplorerSettingsPropertyChanged;
        }
        #endregion

        #region Event Handlers
        private void OnEditorPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.InvokeAsync(this.StateHasChanged);
        }

        private void OnTreeSelectionChange(ITreeNode node)
        {
            if (this.Editor.TreeViewModel != null)
            {
                this.Editor.TreeViewModel.SelectedNode = node;
            }
        }

        private async Task StartResizing(MouseEventArgs e)
        {
            await JSRuntime.InvokeVoidAsync("initResizer", "workbench-container");
        }

        private async void OnEditorSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EditorAppearanceSettingsViewModel.TintEnabled))
            {
                await ApplyTintIntensity();
            }

            await InvokeAsync(StateHasChanged);
        }

        private void OnExplorerSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        private async Task ApplyTintIntensity()
        {
            string intensity = EditorSettings.TintEnabled ? "6%" : "0%";
            await JSRuntime.InvokeVoidAsync("setTintIntensity", intensity);
        }

        private string GetTypeSymbol(ITreeNode node)
        {
            string result = "";

            if (node is ObjectEditorViewModel objectEditorViewModel)
            {
                result = objectEditorViewModel.TypeSymbol;
            }

            return result;
        }

        #endregion
    

    }
}