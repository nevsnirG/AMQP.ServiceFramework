namespace AMQP.ServiceFramework
{
    public abstract class MessageParser
    {
        public abstract object Parse(byte[] body);
    }
}