using MDD4All.Localization.Contracts;
using Microsoft.AspNetCore.Components;
using System;

namespace MDD4All.DME.Views.Localization
{
    // Base for every component that puts translated text on screen. It listens for a language
    // change and redraws itself.
    //
    // Each one has to do this for itself, because a parent redraw does not carry down. Blazor
    // skips a child whose parameters have not changed - that is a deliberate optimisation in its
    // diff, and a component without parameters therefore never redraws from above. Measured on
    // 2026-08-27: the switch reached Index, Index redrew, and nothing below it did.
    //
    // Only the components that show text inherit from this, so a language change costs exactly
    // the redraws it needs and not one more.
    public class LocalizedComponent : ComponentBase, IDisposable
    {
        [Inject]
        protected ILanguageSetter LanguageSetter { get; set; } = null!;

        // Derived components that need their own must call base.OnInitialized().
        protected override void OnInitialized()
        {
            LanguageSetter.CultureChanged += OnCultureChanged;
        }

        private void OnCultureChanged(object? sender, EventArgs e)
        {
            // The event does not have to arrive on the renderer's thread.
            InvokeAsync(StateHasChanged);
        }

        // Derived components that need their own must call base.Dispose().
        public virtual void Dispose()
        {
            LanguageSetter.CultureChanged -= OnCultureChanged;
        }
    }
}
