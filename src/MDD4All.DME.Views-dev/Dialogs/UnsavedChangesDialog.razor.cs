using MDD4All.DME.ViewModels.DataManager;
using MDD4All.UI.BlazorComponents.Dialog;
using Microsoft.AspNetCore.Components;

namespace MDD4All.DME.Views.Dialogs
{
    public partial class UnsavedChangesDialog
    {
        [Inject]
        public DataManagerFileViewModel DataContext { get; set; } = null!;

        // Closing by the x counts as cancel - the answer that changes nothing.
        private void OnDialogResult(ModalDialog.ModalDialogResult result)
        {
            UnsavedChangesAnswer answer = UnsavedChangesAnswer.Cancel;

            if (result == ModalDialog.ModalDialogResult.Confirm)
            {
                answer = UnsavedChangesAnswer.Save;
            }
            else if (result == ModalDialog.ModalDialogResult.Discard)
            {
                answer = UnsavedChangesAnswer.Discard;
            }

            DataContext.AnswerUnsavedChanges(answer);
        }
    }
}
