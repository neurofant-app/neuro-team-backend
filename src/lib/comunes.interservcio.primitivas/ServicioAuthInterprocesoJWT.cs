using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace comunes.interservicio.primitivas;

public class ServicioAuthInterprocesoJWT: IServicioAutenticacionJWT
{
    private readonly ConfiguracionAPI configuracionAPI;
    private IHttpClientFactory _clientFactory;
    private readonly IDistributedCache _cache;
    private HttpClient _client;
    private AutenticacionJWT? _authConfigJWT;
    public ServicioAuthInterprocesoJWT(IOptions<ConfiguracionAPI> options, IHttpClientFactory clientFactory, IDistributedCache cache )
    {
        configuracionAPI = options.Value;
        _clientFactory = clientFactory;
        _cache = cache;
        _client = _clientFactory.CreateClient();
    }

    /// <summary>
    /// OBtiene un token de interproceso, si no se envía la clave de configuración toma la definida por 'auth_default'
    /// </summary>
    /// <param name="claveConfiguracion"></param>
    /// <returns></returns>
    public async Task<TokenJWT?> TokenInterproceso(string claveConfiguracion = "auth_default")
    {
        var token = new TokenJWT();
        var tokenCache = _cache.GetString( claveConfiguracion );

        // REMOVER ESTE COMENATRIO AL IMPLEMENTAR
        // SI TOKENCACHE <> null entonces ebe validarse la fecha e expiracion
        // EN CASO DE QUE HAYA SISO EXPIRADO 
        // 1. REMOVERL DEL CACHE
        // 2. hacer tokenCache = null

        if (tokenCache == null)
        {

            _authConfigJWT = configuracionAPI.AuthConfigJWT.FirstOrDefault(_ => _.Clave == claveConfiguracion);
            if (_authConfigJWT != null)
            {

                Dictionary<string, string> dict = new()
                            {
                                {"grant_type", "client_credentials"},
                                { "client_id", _authConfigJWT.ClientId },
                                { "client_secret", _authConfigJWT.Secret },
                            };

                var result = await _client.PostAsync(new Uri($"{_authConfigJWT.UrlToken}/connect/token"), new FormUrlEncodedContent(dict));
                if (result.IsSuccessStatusCode)
                {
                    string payload = await result.Content.ReadAsStringAsync();
                    token = JsonConvert.DeserializeObject<TokenJWT>(payload);
                    _cache.SetString(claveConfiguracion,JsonConvert.SerializeObject(token), 
                        new DistributedCacheEntryOptions() { 
                            SlidingExpiration = TimeSpan.FromSeconds(token!.expires_in - (token.expires_in*.1))
                        });
                }
            }
        }
        else
        {
            token = JsonConvert.DeserializeObject<TokenJWT>(tokenCache);
        }
         return token;
    }

    public async Task<TokenJWT?> GetToken(string user, string password, string claveConfiguracion = "auth_default")
    {
        var token = new TokenJWT();
        var tokenCache = _cache.GetString(claveConfiguracion);
        if (tokenCache == null)
        {

            _authConfigJWT = configuracionAPI.AuthConfigJWT.FirstOrDefault(_ => _.Clave == claveConfiguracion);
            if (_authConfigJWT != null)
            {

                Dictionary<string, string> dict = new()
                            {
                                {"grant_type", "password"},
                                { "username", user },
                                { "password", password},
                            };

                var result = await _client.PostAsync(new Uri(_authConfigJWT.UrlToken), new FormUrlEncodedContent(dict));
                if (result.IsSuccessStatusCode)
                {
                    string payload = await result.Content.ReadAsStringAsync();
                    token = JsonConvert.DeserializeObject<TokenJWT>(payload);
                    _cache.SetString(claveConfiguracion, JsonConvert.SerializeObject(token),
                        new DistributedCacheEntryOptions()
                        {
                            SlidingExpiration = TimeSpan.FromSeconds(token!.expires_in - (token.expires_in * .1))
                        });
                }
            }
        }
        else
        {
            token = JsonConvert.DeserializeObject<TokenJWT>(tokenCache);
        }
        return token;
    }
}
