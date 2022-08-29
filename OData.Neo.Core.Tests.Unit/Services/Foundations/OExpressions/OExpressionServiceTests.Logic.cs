﻿//-----------------------------------------------------------------------
// Copyright (c) .NET Foundation and Contributors. All rights reserved.
// See License.txt in the project root for license information.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OData.Neo.Core.Brokers.Expressions;
using OData.Neo.Core.Models.OExpressions;
using OData.Neo.Core.Models.OTokens;
using OData.Neo.Core.Models.ProjectedTokens;
using OData.Neo.Core.Services.Foundations.OExpressions;
using Xunit;

namespace OData.Neo.Core.Tests.Unit.Services.Foundations.OExpressions
{
    public partial class OExpressionServiceTests
    {
        [Fact]
        public async Task ShouldGenerateOExpressionAsync()
        {
            // given
            var inputOExpression = new OExpression
            {
                OToken = new OToken
                {
                    Type = OTokenType.Root,

                    Children = new List<OToken>
                    {
                        new OToken
                        {
                            RawValue = "$select",
                            Type = OTokenType.Select,
                            ProjectedType = ProjectedTokenType.Keyword,

                            Children = new List<OToken>
                            {
                                new OToken
                                {
                                    ProjectedType = ProjectedTokenType.Property,
                                    RawValue = "Name",
                                    Type = OTokenType.Property
                                }
                            }
                        }
                    }
                }
            };

            string expectedLinqQuery = "Select(obj => new {obj.Name})";
            Expression generatedExpression = Expression.Constant(value: default);

            var expectedOExpression = new OExpression
            {
                Expression = generatedExpression,
                OToken = inputOExpression.OToken
            };

            this.expressionBrokerMock.Setup(broker =>
                broker.GenerateExpressionAsync<object>(expectedLinqQuery))
                    .ReturnsAsync(generatedExpression);

            // when
            OExpression actualOExpression =
                await this.oExpressionService.GenerateOExpressionAsync<object>(
                    inputOExpression);

            // then
            actualOExpression.Should().BeEquivalentTo(expectedOExpression);

            this.expressionBrokerMock.Verify(broker =>
                broker.GenerateExpressionAsync<object>(expectedLinqQuery),
                    Times.Once);

            this.expressionBrokerMock.VerifyNoOtherCalls();
        }
    }
}
