using MDD4All.DME.ViewModels.DataManager;
using Microsoft.AspNetCore.Components;

namespace MDD4All.DME.Views.StartPage
{
    public partial class StartPageView
    {
        [Inject]
        public DataManagerSettingsViewModel Settings { get; set; } = null!;

        [Inject]
        public DataManagerModelViewModel Model { get; set; } = null!;

        [Inject]
        public DataManagerFileViewModel DataFile { get; set; } = null!;

        [Parameter]
        public EventCallback OnSettingsRequested { get; set; }

        [Parameter]
        public bool CanReturnToEditor { get; set; }

        [Parameter]
        public EventCallback OnEditorRequested { get; set; }
    }
}