namespace AMQP.ServiceFramework
{
    public interface ISubscription<in T>
    {
        void Handle(T input);
    }
}