﻿using OData.Neo.Core.Models.OTokens;
using OData.Neo.Core.Models.OTokens.Exceptions;

namespace OData.Neo.Core.Services.Foundations.OTokenizations
{
    public partial class OTokenizationService
    {
        private delegate OToken ReturningOTokenFunction();

        private OToken TryCatch(ReturningOTokenFunction returningOTokenFunction)
        {
            try
            {
                return returningOTokenFunction();
            }
            catch (NullOTokensException nullOTokensException)
            {
                throw new OTokenValidationException(nullOTokensException);
            }
            catch (NullOTokenException nullOTokenException)
            {
                throw new OTokenValidationException(nullOTokenException);
            }
        }
    }
}