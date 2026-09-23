using Microsoft.AspNetCore.Components;
using MDD4All.DME.ViewModels.Editor;
using MDD4All.ObjectGraph.Access;

namespace MDD4All.DME.Views.Editor
{
    public partial class IndexedCollectionBody
    {
        [Parameter] 
        public IndexedCollectionEditorViewModel ViewModel { get; set; } = null!;

        [Parameter] 
        public int MaxDepth { get; set; }

        [Parameter]
        public int CurrentDepth { get; set; }

        [Parameter] public bool IsCompact { get; set; } = false;

        // A child carries its place in the collection with it; ListAccess and ArrayAccess are
        // both an IndexedAccess.
        private static int IndexOf(ObjectEditorViewModel childVm)
        {
            int result = -1;

            if (childVm.Access is IndexedAccess indexedAccess)
            {
                result = indexedAccess.Index;
            }

            return result;
        }

        private bool IsFirst(ObjectEditorViewModel childVm)
        {
            return IndexOf(childVm) <= 0;
        }

        private bool IsLast(ObjectEditorViewModel childVm)
        {
            return IndexOf(childVm) >= ViewModel.Children.Count - 1;
        }

        private void OnDeleteChild(ObjectEditorViewModel childVm)
        {
            int index = IndexOf(childVm);

            // Run the delete command once a valid index was found
            if (index != -1 && ViewModel.DeleteAtIndexCommand.CanExecute(index))
            {
                ViewModel.DeleteAtIndexCommand.Execute(index);
            }
        }

        private void OnMoveChild(ObjectEditorViewModel childVm, bool moveUp)
        {
            int index = IndexOf(childVm);

            if (index != -1)
            {
                if (moveUp)
                {
                    ViewModel.MoveItemUpCommand.Execute(index);
                }
                else
                {
                    ViewModel.MoveItemDownCommand.Execute(index);
                }
            }
        }
    }
}