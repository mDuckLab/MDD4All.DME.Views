using MDD4All.DME.ViewModels.DataManager;
using Microsoft.AspNetCore.Components;

namespace MDD4All.DME.Views.Dialogs
{
    public partial class ComplexKeyWarningDialog
    {
        [Inject]
        public DataManagerFileViewModel DataContext { get; set; } = null!;

        // Closing by the x or the cancel button both mean no, which is the safe answer here.
        private void OnDialogClose(bool writeAnyway)
        {
            DataContext.AnswerComplexKeyWarning(writeAnyway);
        }
    }
}
