namespace MDD4All.DME.Views
{
    // Nothing but a name. IStringLocalizer&lt;T&gt; builds the resource file's name out of the type it
    // is given, so this class exists solely to point at Resources/AppTexts.resx.
    //
    // It sits in the project root on purpose: the name is assembled as
    // {assembly}.{ResourcesPath}.{type without the assembly prefix}, so a type in a subfolder
    // namespace would want the resx in a matching subfolder underneath Resources.
    //
    // One file for the whole application rather than one per component. The components here do
    // not travel anywhere on their own, so there is nothing to keep apart - and a text needed in
    // three places would otherwise be written three times.
    public class AppTexts
    {
    }
}
