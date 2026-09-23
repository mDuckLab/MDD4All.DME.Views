using MDD4All.Localization.Contracts;
using System.Globalization;
using System.Resources;

namespace MDD4All.DME.Views.Localization
{
    // Looks a text up in Resources/AppTexts.resx for the language the user picked.
    //
    // Deliberately not IStringLocalizer. That one resolves against CultureInfo.CurrentUICulture,
    // which lives in an AsyncLocal - it belongs to an execution flow, and the flow the renderer
    // runs in cannot be written to from anywhere else. Measured, repeatedly: the picked language
    // arrives everywhere except there, so the texts never followed.
    //
    // The culture is handed over explicitly instead. ILanguageSetter always knows which one was
    // picked, so there is nothing to guess and nothing that can be out of reach.
    public class AppTextProvider
    {
        private readonly ResourceManager _resources;

        private readonly ILanguageSetter _languageSetter;

        public AppTextProvider(ILanguageSetter languageSetter)
        {
            _languageSetter = languageSetter;

            // The name is the one the compiler gives the embedded resource: the assembly, then
            // the folder, then the file.
            _resources = new ResourceManager("MDD4All.DME.Views.Resources.AppTexts",
                                             typeof(AppTexts).Assembly);
        }

        // Missing keys give back the key itself. A screen with "Toolbar.Save" on a button is
        // ugly, but it says what is missing - an empty button says nothing.
        public string this[string key]
        {
            get
            {
                string result = key;

                CultureInfo culture = _languageSetter.CurrentCulture;

                string? text = _resources.GetString(key, culture);

                if (text != null)
                {
                    result = text;
                }

                return result;
            }
        }
    }
}
