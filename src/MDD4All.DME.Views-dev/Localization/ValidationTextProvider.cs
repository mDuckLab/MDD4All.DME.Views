using System.ComponentModel.DataAnnotations;

namespace MDD4All.DME.Views.Localization
{
    // Puts a broken rule into words, in the language the user picked.
    //
    // The attributes can word themselves - RangeAttribute.FormatErrorMessage says exactly this.
    // But it reads its sentence out of the framework's resources through
    // CultureInfo.CurrentUICulture, and that value is the one a Blazor Hybrid host cannot reach.
    // Measured three times now: the localizer, the Display labels, and this.
    //
    // So the sentences come from AppTexts.resx like every other text in the application, where
    // the culture is handed over rather than read. The attribute only supplies the numbers.
    public class ValidationTextProvider
    {
        private readonly AppTextProvider _texts;

        public ValidationTextProvider(AppTextProvider texts)
        {
            _texts = texts;
        }

        public string Describe(ValidationAttribute rule, string fieldName)
        {
            string result;

            if (rule is RangeAttribute range)
            {
                result = string.Format(_texts["Validation.Range"],
                                       fieldName, range.Minimum, range.Maximum);
            }
            else if (rule is StringLengthAttribute length)
            {
                result = string.Format(_texts["Validation.StringLength"],
                                       fieldName, length.MinimumLength, length.MaximumLength);
            }
            else if (rule is MinLengthAttribute minLength)
            {
                result = string.Format(_texts["Validation.MinLength"], fieldName, minLength.Length);
            }
            else if (rule is MaxLengthAttribute maxLength)
            {
                result = string.Format(_texts["Validation.MaxLength"], fieldName, maxLength.Length);
            }
            else if (rule is RequiredAttribute)
            {
                result = string.Format(_texts["Validation.Required"], fieldName);
            }
            else if (rule is EmailAddressAttribute)
            {
                result = string.Format(_texts["Validation.Email"], fieldName);
            }
            else if (rule is UrlAttribute)
            {
                result = string.Format(_texts["Validation.Url"], fieldName);
            }
            else if (rule is RegularExpressionAttribute)
            {
                result = string.Format(_texts["Validation.Pattern"], fieldName);
            }
            else
            {
                // A rule this editor has never heard of, brought along by a data model. It says
                // so itself - not necessarily in the picked language, but saying something beats
                // saying nothing.
                result = rule.FormatErrorMessage(fieldName);
            }

            return result;
        }
    }
}
