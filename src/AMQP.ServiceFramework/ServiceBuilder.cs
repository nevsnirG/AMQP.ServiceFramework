using System;
using System.Collections.Generic;
using System.Text;

namespace AMQP.ServiceFramework
{
    public sealed class ServiceBuilder
    {
        private readonly object _lock;

        private bool _initialized;
        
        public ServiceBuilder()
        {
            _lock = new object();
        }

        public void EnsureInitialization()
        {
            if (!_initialized)
            {
                lock (_lock)
                {
                    if (!_initialized)
                    {
                        //TODO - Initialize.
                        _initialized = true;
                    }
                }
            }
        }
    }
}