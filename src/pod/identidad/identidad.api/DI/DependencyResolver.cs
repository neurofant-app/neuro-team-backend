namespace identidad.api;

public class DependencyResolver : IDependencyResolver
{
    private readonly IServiceProvider _serviceProdiver;

    public DependencyResolver(IServiceProvider serviceProvider)
    {
        _serviceProdiver = serviceProvider;
    }

    public T GetService<T>()
    {
        return _serviceProdiver.GetService<T>();
    }

}
