using AMQP.ServiceFramework.Core.Resolvers;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace AMQP.ServiceFramework.Tests
{
    [ExcludeFromCodeCoverage]
    public class TopicClientTypeResolverTests
    {
        [Fact]
        public void Constructor_NullInput_ArgumentNullException()
        {
            //Act
            try
            {
                new TopicClientTypeResolver(null);

                //Assert
                Assert.True(false, "The constructor was supposed to throw an ArgumentNullException.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.Equal("attributeResolverFactory", ex.ParamName);
            }
        }
    }
}