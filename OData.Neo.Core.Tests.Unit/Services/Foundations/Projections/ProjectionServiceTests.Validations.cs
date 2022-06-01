﻿//-----------------------------------------------------------------------
// Copyright (c) .NET Foundation and Contributors. All rights reserved.
// See License.txt in the project root for license information.
//-----------------------------------------------------------------------

using System;
using FluentAssertions;
using OData.Neo.Core.Models.ProjectedTokens;
using OData.Neo.Core.Models.ProjectedTokens.Exceptions;
using Xunit;

namespace OData.Neo.Core.Tests.Unit.Services.Foundations.Projections
{
    public partial class ProjectionServiceTests
    {
        [Fact]
        public void ShouldThrowValidationExceptionOnProjectIfTokensIsNull()
        {
            // given
            ProjectedToken[] nullProjectedTokens = null;

            var nullProjectedTokenException =
                new NullProjectedTokenException();

            var projectedTokenValidationException =
                new ProjectedTokenValidationException(
                    nullProjectedTokenException);

            // when
            Action projectTokensAction = () =>
                this.projectionService.ProjectTokens(
                    nullProjectedTokens);

            // then
            ProjectedTokenValidationException actualProjectedTokenValidationException =
                Assert.Throws<ProjectedTokenValidationException>(
                    projectTokensAction);

            actualProjectedTokenValidationException.InnerException.Should()
                .BeOfType<NullProjectedTokenException>();
        }
    }
}
