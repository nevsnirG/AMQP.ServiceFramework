﻿using AMQP.ServiceFramework.Activation;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Xunit;

namespace AMQP.ServiceFramework.Tests
{
    [ExcludeFromCodeCoverage]
    public class CommandHandlerActivatorTests
    {
        [Fact]
        public void Create_NullInput_ArgumentNullException()
        {
            //Arrange
            var activator = new CommandHandlerActivator(null);

            //Act
            try
            {
                activator.Create(null);

                //Assert
                Assert.True(false, "The method was supposed to throw an ArgumentNullException.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.Equal("commandHandlerContext", ex.ParamName);
            }
        }
    }
}