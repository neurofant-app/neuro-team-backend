using comunes.interservicio.primitivas;
using espaciotrabajo.api.seguridad;

namespace espaciotrabajo.api;

public class Worker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var manager = scope.ServiceProvider.GetRequiredService<IProxySeguridad>();
        ConfiguracionSeguridad configuracionSeguridad = new();
        await manager.ActualizaSeguridad(await configuracionSeguridad.ObtieneApliaciones());
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
