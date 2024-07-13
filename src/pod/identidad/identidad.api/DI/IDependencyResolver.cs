namespace identidad.api;

public interface IDependencyResolver
{
    T GetService<T>();
}
