using Microsoft.AspNetCore.Components;
using MDD4All.DME.ViewModels.Editor;
using MDD4All.DME.ViewModels.Editor.Settings;
using MDD4All.DME.Views.Localization;
using System.ComponentModel.DataAnnotations;
using System;
using System.Globalization;
using System.Linq;
using System.ComponentModel;

namespace MDD4All.DME.Views.Editor
{
    public partial class PrimitivePropertyEditorView
    {
        #region Parameters
        [Parameter]
        public PrimitivePropertyViewModel ViewModel { get; set; } = null!;

        [Parameter]
        public bool IsCompact { get; set; } = false;

        [Parameter]
        public bool ShowTitle { get; set; } = true;
        #endregion

        [Inject]
        public EditorAppearanceSettingsViewModel Settings { get; set; } = null!;

        [Inject]
        public ValidationTextProvider ValidationTexts { get; set; } = null!;

        #region Private Fields
        private string? _localValue;

        // Whether what is currently in the field would be taken. Purely a display state - the
        // model still holds the last value that was.
        private bool _localValueIsValid = true;
        #endregion

        #region Lifecycle and Event Subscription
        protected override void OnInitialized()
        {
            base.OnInitialized();

            Settings.PropertyChanged += OnSettingsPropertyChanged;
        }

        private void OnSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        protected override void OnParametersSet()
        {
            if (this.ViewModel != null)
            {
                // Initialize the local UI state from the ViewModel
                this._localValue = this.ViewModel.Item?.ToString();

                // Subscribe to property changes for snap-back support
                this.ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            if (this.ViewModel != null)
            {
                // Unsubscribe to avoid memory leaks
                this.ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }
            Settings.PropertyChanged -= OnSettingsPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PrimitivePropertyViewModel.BrokenRules))
            {
                this.InvokeAsync(this.StateHasChanged);
            }

            if (e.PropertyName == nameof(PrimitivePropertyViewModel.Item))
            {
                // If the ViewModel item changes (e.g. reverted by the Dictionary logic),
                // we must update our local shadow variable and refresh the UI.
                this._localValue = this.ViewModel.Item?.ToString();
                this._localValueIsValid = true;
                this.InvokeAsync(this.StateHasChanged);
            }
        }
        #endregion

        #region Event Handlers
        // Which of the three date controls the browser should show.
        private string DateInputType()
        {
            string result = "datetime-local";

            if (this.ViewModel.DataTypeAnnotation == "Date") { result = "date"; }
            if (this.ViewModel.DataTypeAnnotation == "Time") { result = "time"; }

            return result;
        }

        // Each of the three wants its own shape, and a value in the wrong one is ignored by the
        // browser - the field would simply come up empty.
        private string DateInputValue()
        {
            DateTime value = (DateTime)(this.ViewModel.Item ?? DateTime.Now);

            string result = value.ToString("yyyy-MM-ddTHH:mm");

            if (this.ViewModel.DataTypeAnnotation == "Date") { result = value.ToString("yyyy-MM-dd"); }
            if (this.ViewModel.DataTypeAnnotation == "Time") { result = value.ToString("HH:mm"); }

            return result;
        }

        // Worded on every render, so a language switch reaches the reason as well.
        private string Describe(ValidationAttribute rule)
        {
            return ValidationTexts.Describe(rule, this.ViewModel.Title);
        }

        private void OnInput(ChangeEventArgs e)
        {
            // The field keeps whatever is being typed - snapping back on every keystroke would
            // make a number below the lower bound impossible to reach at all.
            this._localValue = e.Value?.ToString();

            // Reddens the field at the first character that breaks a rule. No sentence yet -
            // that comes when the field is left, along with the value going back.
            this._localValueIsValid = this.ViewModel.IsValid(this._localValue);

            // Whatever the last attempt was told off for no longer applies.
            this.ViewModel.ClearBrokenRules();
        }

        private void OnCommit(ChangeEventArgs e)
        {
            string? newValue = e.Value?.ToString();

            // Push the change to the ViewModel
            this.ViewModel.Item = newValue;

            // Re-sync local value immediately. If the ViewModel rejected the change,
            // this restores the previous valid state - which is valid by definition, so the
            // field stops being red and the message explains what happened to the input.
            this._localValue = this.ViewModel.Item?.ToString();
            this._localValueIsValid = true;
        }

        private void OnBlur()
        {
            // Final sync when the field loses focus
            this._localValue = this.ViewModel.Item?.ToString();
            this.StateHasChanged();
        }

        private void OnIntegerChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            // Remove non-digit characters except for a leading minus sign
            var cleanedValue = new string(value.Where((c, index) =>
                char.IsDigit(c) || (index == 0 && c == '-')
            ).ToArray());

            if (cleanedValue == "-" || string.IsNullOrEmpty(cleanedValue)) return;

            if (long.TryParse(cleanedValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out long result))
            {
                try
                {
                    // Convert to the specific target type (int, long, sbyte, etc.)
                    this.ViewModel.Item = Convert.ChangeType(result, this.ViewModel.Type!);
                }
                catch (OverflowException)
                {
                    // Value is too large for the target type
                }
            }

            // Back to whatever the model holds - the entered value if it was taken, the previous
            // one if a rule turned it away.
            this._localValue = this.ViewModel.Item?.ToString();
            this._localValueIsValid = true;

            this.StateHasChanged();
        }

        private void OnDecimalChanged(string? value)
        {
            if (!string.IsNullOrWhiteSpace(value) &&
                decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal parsed))
            {
                try
                {
                    this.ViewModel.Item = Convert.ChangeType(parsed, this.ViewModel.Type!,
                                                             CultureInfo.InvariantCulture);
                }
                catch (OverflowException)
                {
                    // Value is too large for the target type
                }
            }

            this._localValue = this.ViewModel.Item?.ToString();
            this._localValueIsValid = true;

            this.StateHasChanged();
        }

        private void OnDateTimeChanged(string? value)
        {
            if (DateTime.TryParse(value, out DateTime dt))
            {
                this.ViewModel.Item = dt;
                this.StateHasChanged();
            }
        }
        #endregion
    }
}