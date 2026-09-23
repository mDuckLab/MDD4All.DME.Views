using MDD4All.DME.ViewModels.DataManager;
using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;

namespace MDD4All.DME.Views.DataManager
{
    public partial class StatusBarView : IDisposable
    {
        [Parameter]
        public DataManagerFileViewModel DataContext { get; set; } = null!;

        // The line changes without anything above it changing, so a parent render never reaches
        // it - saving a file leaves the rest of the screen exactly as it was.
        protected override void OnInitialized()
        {
            DataContext.PropertyChanged += OnDataContextPropertyChanged;
        }

        private void OnDataContextPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DataManagerFileViewModel.StatusText))
            {
                InvokeAsync(StateHasChanged);
            }
        }

        public void Dispose()
        {
            DataContext.PropertyChanged -= OnDataContextPropertyChanged;
        }
    }
}
