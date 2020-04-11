using System;
using System.Runtime.Serialization;

namespace AMQP.ServiceFramework.Exceptions
{
    [Serializable]
    public class ServiceBuilderException : Exception
    {
        public ServiceBuilderException()
        {
        }

        public ServiceBuilderException(string message) : base(message)
        {
        }

        public ServiceBuilderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ServiceBuilderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}