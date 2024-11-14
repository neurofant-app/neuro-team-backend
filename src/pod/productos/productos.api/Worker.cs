
using comunes.interservicio.primitivas;

namespace productos.api;

public class Worker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        //await using var scope = _serviceProvider.CreateAsyncScope();

        //var manager = scope.ServiceProvider.GetRequiredService<IProxySeguridad>();
        //ConfiguracionSeguridad configuracionSeguridad = new();
        //var apps = await configuracionSeguridad.ObtieneApliaciones();
        //await manager.ActualizaSeguridad(apps);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
