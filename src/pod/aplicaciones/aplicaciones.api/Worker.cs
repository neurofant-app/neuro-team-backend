
using aplicaciones.api.seguridad;
using comunes.interservicio.primitivas;
using OpenIddict.Abstractions;

namespace aplicaciones.api;

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
        await manager.ActualizaSeguridad(ConfiguracionSeguridad.ObtieneAplicaciones());
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
