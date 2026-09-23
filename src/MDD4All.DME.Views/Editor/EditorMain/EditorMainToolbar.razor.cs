using MDD4All.DME.ViewModels.DataManager;
using Microsoft.AspNetCore.Components;

namespace MDD4All.DME.Views.Editor
{
    public partial class EditorMainToolbar
    {
        [Inject]
        public MainViewModel Navigation { get; set; } = null!;

        [Inject]
        public EditorViewModel Editor { get; set; } = null!;

        [Inject]
        public DataManagerFileViewModel DataFile { get; set; } = null!;

    }
}
