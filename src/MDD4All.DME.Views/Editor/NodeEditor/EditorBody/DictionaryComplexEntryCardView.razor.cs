using MDD4All.DME.ViewModels.Editor;
using Microsoft.AspNetCore.Components;

namespace MDD4All.DME.Views.Editor
{
    public partial class DictionaryComplexEntryCardView
    {
        [Parameter]
        public DictionaryEntryViewModel DataContext { get; set; } = null!;

        [Parameter]
        public int MaxDepth { get; set; }

        [Parameter]
        public int CurrentDepth { get; set; }

        // The nested ObjectEditorView only sets these in its own OnInitialized, which runs after
        // these headers were rendered once - setting them here keeps the expanders correct from
        // the first render on.
        protected override void OnParametersSet()
        {
            if (DataContext.KeyEditor != null)
            {
                DataContext.KeyEditor.EditorState.CurrentDepth = CurrentDepth + 1;
                DataContext.KeyEditor.EditorState.MaxDepth = MaxDepth;
            }

            if (DataContext.ValueEditor != null)
            {
                DataContext.ValueEditor.EditorState.CurrentDepth = CurrentDepth + 1;
                DataContext.ValueEditor.EditorState.MaxDepth = MaxDepth;
            }
        }

        private bool ShowKeyExpander
        {
            get
            {
                if (DataContext.KeyEditor == null)
                {
                    return false;
                }

                return DataContext.KeyEditor.EditorState.ShowExpander;
            }
        }

        private bool IsKeyExpanded
        {
            get
            {
                if (DataContext.KeyEditor == null)
                {
                    return false;
                }

                return DataContext.KeyEditor.EditorState.IsExpanded;
            }
        }

        private bool ShowValueExpander
        {
            get
            {
                if (DataContext.ValueEditor == null)
                {
                    return false;
                }

                return DataContext.ValueEditor.EditorState.ShowExpander;
            }
        }

        private bool IsValueExpanded
        {
            get
            {
                if (DataContext.ValueEditor == null)
                {
                    return false;
                }

                return DataContext.ValueEditor.EditorState.IsExpanded;
            }
        }

        private void ToggleKey()
        {
            if (DataContext.KeyEditor != null && ShowKeyExpander)
            {
                DataContext.KeyEditor.EditorState.IsExpanded = !DataContext.KeyEditor.EditorState.IsExpanded;
            }
        }

        private void ToggleValue()
        {
            if (DataContext.ValueEditor != null && ShowValueExpander)
            {
                DataContext.ValueEditor.EditorState.IsExpanded = !DataContext.ValueEditor.EditorState.IsExpanded;
            }
        }

        // Deletes key and value together - a dictionary entry without its key cannot exist.
        private void OnDeleteEntry()
        {
            if (DataContext.DeleteItemCommand != null && DataContext.DeleteItemCommand.CanExecute(null))
            {
                DataContext.DeleteItemCommand.Execute(null);
            }
        }
    }
}
