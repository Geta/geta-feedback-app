using Microsoft.AspNetCore.Components.Forms;

namespace Feedback.Blazor.Utilities;

public class BootstrapValidationClassProvider : FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

        if (editContext.IsModified(fieldIdentifier))
        {
            return isValid ? "modified is-valid" : "modified is-invalid";
        }

        return isValid ? "" : "is-invalid";
    }
}
