using System.Collections.Generic;
using Volo.Abp;

namespace ITE.Bookify.Validations;
public class ValidationsException : BusinessException
{
    public ValidationsException(List<string> errors) : base(BookifyDomainErrorCodes.ValidationErrors)
    {
        WithData("Errors", errors);
    }
}

