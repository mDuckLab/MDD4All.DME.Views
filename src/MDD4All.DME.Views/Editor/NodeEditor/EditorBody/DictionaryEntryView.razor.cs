using Microsoft.AspNetCore.Components;
using MDD4All.DME.ViewModels.Editor;

namespace MDD4All.DME.Views.Editor
{
    public partial class DictionaryEntryView
    {
        [Parameter]
        public DictionaryEntryViewModel DataContext { get; set; } = null!;

        [Parameter]
        public bool DeleteMode { get; set; } = false;

        [Parameter] public int MaxDepth { get; set; }
        [Parameter] public int CurrentDepth { get; set; }

        /// <summary>
        /// Runs the delete command for this one dictionary entry.
        /// </summary>
        private void OnDeleteEntry()
        {
            if (DataContext.DeleteItemCommand != null && DataContext.DeleteItemCommand.CanExecute(null))
            {
                DataContext.DeleteItemCommand.Execute(null);
            }
        }
    }
}