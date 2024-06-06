using aplicaciones.api.seguridad;

namespace aplicaciones.api;

public class Worker : IHostedService
{
    private readonly IProxySeguridad proxySeguridad;

    public Worker(IProxySeguridad proxySeguridad)
    {
        this.proxySeguridad = proxySeguridad;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await proxySeguridad.ActualizaSeguridad(ConfiguracionSeguridad.ObtieneAplicaciones());
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
