namespace AMQP.ServiceFramework.Registries
{
    public abstract class MessageParser
    {
        public abstract object Parse(byte[] body);
    }
}